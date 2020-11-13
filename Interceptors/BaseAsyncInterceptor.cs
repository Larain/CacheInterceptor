using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using CacheInterceptor.Contracts.Attributes;
using Castle.DynamicProxy;

namespace CacheInterceptor.Interceptors
{
    public abstract class BaseAsyncInterceptor<TAttribute> : IInterceptor where TAttribute : BaseInterceptorAttribute
    {
        private static readonly MethodInfo HandleMethodInfo = typeof(BaseAsyncInterceptor<TAttribute>).GetMethod(nameof(InterceptInnerWithResult), BindingFlags.Instance | BindingFlags.NonPublic);
        private static readonly MethodInfo HandleAsyncMethodInfo = typeof(BaseAsyncInterceptor<TAttribute>).GetMethod(nameof(InterceptInnerWithResultAsync), BindingFlags.Instance | BindingFlags.NonPublic);
        protected abstract void InterceptInner(IInvocation invocation);
        protected abstract T InterceptInnerWithResult<T>(IInvocation invocation);
        protected abstract Task InterceptInnerAsync(IInvocation invocation);
        protected abstract Task<T> InterceptInnerWithResultAsync<T>(IInvocation invocation);

        protected static readonly List<Func<IInvocation, TAttribute>> AttributeSelectors = new List<Func<IInvocation, TAttribute>>
        {
            // (!) order matters
            invocation => invocation.MethodInvocationTarget.GetCustomAttribute<TAttribute>(),  // 1. if class method has attribute
            invocation => invocation.TargetType.GetCustomAttribute<TAttribute>(),              // 2. if whole class has attribute
            invocation => invocation.Method.GetCustomAttribute<TAttribute>(),                  // 3. if interface method has attribute
            invocation => invocation.Method.DeclaringType?.GetCustomAttribute<TAttribute>(),   // 4. if whole interface has attribute
        };

        protected static TAttribute FindAttribute(IInvocation invocation)
        {
            // try find method attribute
            // separate check for null here in case when
            // there is attribute that exclude method from being intercepted
            foreach (var selector in AttributeSelectors)
            {
                var attribute = selector(invocation);
                if (attribute == null) continue;
                return attribute; //get only first != null
            }

            return null;
        }

        public void Intercept(IInvocation invocation)
        {
            if (!ShouldBeIntercepted(invocation))
            {
                invocation.Proceed();
                return;
            };

            var delegateType = GetDelegateType(invocation);
            switch (delegateType)
            {
                case MethodType.Void:
                    InterceptInner(invocation);
                    break;
                case MethodType.Synchronous:
                    invocation.ReturnValue = ExecuteHandleWithResultUsingReflection(HandleMethodInfo, invocation.Method.ReturnType, invocation);
                    break;
                case MethodType.AsyncAction:
                    invocation.ReturnValue = InterceptInnerAsync(invocation);
                    break;
                case MethodType.AsyncFunction:
                    var resultType = invocation.Method.ReturnType.GetGenericArguments()[0];
                    invocation.ReturnValue = ExecuteHandleWithResultUsingReflection(HandleAsyncMethodInfo, resultType, invocation);
                    break;
            }
        }

        private static bool ShouldBeIntercepted(IInvocation invocation)
        {
            var attribute = FindAttribute(invocation);
            return attribute != null && attribute.Enabled;
        }

        private object ExecuteHandleWithResultUsingReflection(MethodInfo handleMethodInfo, Type resultType, IInvocation invocation)
        {
			try
			{
				var mi = handleMethodInfo.MakeGenericMethod(resultType);
				return mi.Invoke(this, new object[] {invocation});
			} 
			catch (TargetInvocationException ex) when(ex.InnerException != null)
			{
				ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
				throw;
			}
		}

        private static MethodType GetDelegateType(IInvocation invocation)
        {
            var returnType = invocation.Method.ReturnType;
            if (returnType == typeof(void))
                return MethodType.Void;
            if (returnType == typeof(Task))
                return MethodType.AsyncAction;
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(Task<>))
                return MethodType.AsyncFunction;
            return MethodType.Synchronous;
        }

        private enum MethodType
        {
            Void,
            Synchronous,
            AsyncAction,
            AsyncFunction
        }
    }
}