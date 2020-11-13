using System;
using System.Threading.Tasks;
using Cache.Tests.Infrastructure;
using CacheInterceptor.Installers;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Moq;
using NUnit.Framework;

namespace Cache.Tests
{
    [TestFixture]
    public class CacheableObjectTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
        }

        [Test]
        [TestCase(true, 0, 0)]
        [TestCase(true, 10, 1)]
        [TestCase(true, 20, 1)]
        [TestCase(false, 0, 0)]
        [TestCase(false, 10, 1)]
        [TestCase(false, 50, 1)]
        public async Task Should_call_provider_exactly_N_times_async_with_class_method_attribute(bool hasValueInCache, int timesToCallProvider, int expectedCallsCount)
        {
            var container = new WindsorContainer();
            container.Install(new CacheManagerInstaller());
            container.Register(Component.For<ITestProvider>().ImplementedBy<TestProvider>());

            var guid = Guid.NewGuid().ToString();
            var valueStorageMock = new Mock<IValueStorage>();
            RegisterMockObject(container, valueStorageMock);

            if (hasValueInCache)
            {
                valueStorageMock.Setup(m => m.GetValueAsync<int>(It.Is<string>(i => i == guid)))
                    .Returns(Task.FromResult(1));
            }

            var testProvider = container.Resolve<ITestProvider>();
            for (var i = 0; i < timesToCallProvider; i++)
            {
                var result = await testProvider.GetClassWithAttributeAsync(guid);
            }

            valueStorageMock.Verify(p => p.GetValueAsync<int>(It.Is<string>(i => i == guid)), Times.Exactly(expectedCallsCount));
        }

        [Test]
        [TestCase(true, 0, 0)]
        [TestCase(true, 10, 10)]
        [TestCase(true, 20, 20)]
        [TestCase(false, 0, 0)]
        [TestCase(false, 10, 10)]
        [TestCase(false, 50, 50)]
        public async Task Should_call_provider_exactly_N_times_async_without_class_method_attribute(bool hasValueInCache, int timesToCallProvider, int expectedCallsCount)
        {
            var container = new WindsorContainer();
            container.Install(new CacheManagerInstaller());
            container.Register(Component.For<ITestProvider>().ImplementedBy<TestProvider>());

            var guid = Guid.NewGuid().ToString();
            var valueStorageMock = new Mock<IValueStorage>();
            RegisterMockObject(container, valueStorageMock);

            if (hasValueInCache)
            {
                valueStorageMock.Setup(m => m.GetValueAsync<int>(It.Is<string>(i => i == guid)))
                    .Returns(Task.FromResult(1));
            }

            var testProvider = container.Resolve<ITestProvider>();
            for (var i = 0; i < timesToCallProvider; i++)
            {
                var result = await testProvider.GetClassWithoutAttributeAsync(guid);
            }

            valueStorageMock.Verify(p => p.GetValueAsync<int>(It.Is<string>(i => i == guid)), Times.Exactly(expectedCallsCount));
        }

        [Test]
        [TestCase(true, 0, 0)]
        [TestCase(true, 10, 1)]
        [TestCase(true, 20, 1)]
        [TestCase(false, 0, 0)]
        [TestCase(false, 10, 1)]
        [TestCase(false, 50, 1)]
        public async Task Should_call_provider_exactly_N_times_async_with_interface_method_attribute(bool hasValueInCache, int timesToCallProvider, int expectedCallsCount)
        {
            var container = new WindsorContainer();
            container.Install(new CacheManagerInstaller());
            container.Register(Component.For<ITestProvider>().ImplementedBy<TestProvider>());

            var guid = Guid.NewGuid().ToString();
            var valueStorageMock = new Mock<IValueStorage>();
            RegisterMockObject(container, valueStorageMock);

            if (hasValueInCache)
            {
                valueStorageMock.Setup(m => m.GetValueAsync<int>(It.Is<string>(i => i == guid)))
                    .Returns(Task.FromResult(1));
            }

            var testProvider = container.Resolve<ITestProvider>();
            for (var i = 0; i < timesToCallProvider; i++)
            {
                var result = await testProvider.GetInterfaceWithAttributeAsync(guid);
            }

            valueStorageMock.Verify(p => p.GetValueAsync<int>(It.Is<string>(i => i == guid)), Times.Exactly(expectedCallsCount));
        }

        [Test]
        [TestCase(true, 0, 0)]
        [TestCase(true, 10, 10)]
        [TestCase(true, 20, 20)]
        [TestCase(false, 0, 0)]
        [TestCase(false, 10, 10)]
        [TestCase(false, 50, 50)]
        public async Task Should_call_provider_exactly_N_times_async_without_interface_method_attribute(bool hasValueInCache, int timesToCallProvider, int expectedCallsCount)
        {
            var container = new WindsorContainer();
            container.Install(new CacheManagerInstaller());
            container.Register(Component.For<ITestProvider>().ImplementedBy<TestProvider>());

            var guid = Guid.NewGuid().ToString();
            var valueStorageMock = new Mock<IValueStorage>();
            RegisterMockObject(container, valueStorageMock);

            if (hasValueInCache)
            {
                valueStorageMock.Setup(m => m.GetValueAsync<int>(It.Is<string>(i => i == guid)))
                    .Returns(Task.FromResult(1));
            }

            var testProvider = container.Resolve<ITestProvider>();
            for (var i = 0; i < timesToCallProvider; i++)
            {
                var result = await testProvider.GetInterfaceWithoutAttributeAsync(guid);
            }

            valueStorageMock.Verify(p => p.GetValueAsync<int>(It.Is<string>(i => i == guid)), Times.Exactly(expectedCallsCount));
        }

        [Test]
        [TestCase(true, 0, 0)]
        [TestCase(true, 10, 1)]
        [TestCase(true, 20, 1)]
        [TestCase(false, 0, 0)]
        [TestCase(false, 10, 1)]
        [TestCase(false, 50, 1)]
        public void Should_call_provider_exactly_N_times_with_class_method_attribute(bool hasValueInCache, int timesToCallProvider, int expectedCallsCount)
        {
            var container = new WindsorContainer();
            container.Install(new CacheManagerInstaller());
            container.Register(Component.For<ITestProvider>().ImplementedBy<TestProvider>());

            var guid = Guid.NewGuid().ToString();
            var valueStorageMock = new Mock<IValueStorage>();
            RegisterMockObject(container, valueStorageMock);

            if (hasValueInCache)
            {
                valueStorageMock.Setup(m => m.GetValue<int>(It.Is<string>(i => i == guid)))
                    .Returns(1);
            }

            var testProvider = container.Resolve<ITestProvider>();
            for (var i = 0; i < timesToCallProvider; i++)
            {
                var result = testProvider.GetClassWithAttribute(guid);
            }

            valueStorageMock.Verify(p => p.GetValue<int>(It.Is<string>(i => i == guid)), Times.Exactly(expectedCallsCount));
        }

        [Test]
        [TestCase(true, 0, 0)]
        [TestCase(true, 10, 10)]
        [TestCase(true, 20, 20)]
        [TestCase(false, 0, 0)]
        [TestCase(false, 10, 10)]
        [TestCase(false, 50, 50)]
        public void Should_call_provider_exactly_N_times_without_class_method_attribute(bool hasValueInCache, int timesToCallProvider, int expectedCallsCount)
        {
            var container = new WindsorContainer();
            container.Install(new CacheManagerInstaller());
            container.Register(Component.For<ITestProvider>().ImplementedBy<TestProvider>());

            var guid = Guid.NewGuid().ToString();
            var valueStorageMock = new Mock<IValueStorage>();
            RegisterMockObject(container, valueStorageMock);

            if (hasValueInCache)
            {
                valueStorageMock.Setup(m => m.GetValue<int>(It.Is<string>(i => i == guid)))
                    .Returns(1);
            }

            var testProvider = container.Resolve<ITestProvider>();
            for (var i = 0; i < timesToCallProvider; i++)
            {
                var result = testProvider.GetClassWithoutAttribute(guid);
            }

            valueStorageMock.Verify(p => p.GetValue<int>(It.Is<string>(i => i == guid)), Times.Exactly(expectedCallsCount));
        }

        [Test]
        [TestCase(true, 0, 0)]
        [TestCase(true, 10, 1)]
        [TestCase(true, 20, 1)]
        [TestCase(false, 0, 0)]
        [TestCase(false, 10, 1)]
        [TestCase(false, 50, 1)]
        public void Should_call_provider_exactly_N_times_with_interface_method_attribute(bool hasValueInCache, int timesToCallProvider, int expectedCallsCount)
        {
            var container = new WindsorContainer();
            container.Install(new CacheManagerInstaller());
            container.Register(Component.For<ITestProvider>().ImplementedBy<TestProvider>());

            var guid = Guid.NewGuid().ToString();
            var valueStorageMock = new Mock<IValueStorage>();
            RegisterMockObject(container, valueStorageMock);

            if (hasValueInCache)
            {
                valueStorageMock.Setup(m => m.GetValue<int>(It.Is<string>(i => i == guid)))
                    .Returns(1);
            }

            var testProvider = container.Resolve<ITestProvider>();
            for (var i = 0; i < timesToCallProvider; i++)
            {
                var result = testProvider.GetInterfaceWithAttribute(guid);
            }

            valueStorageMock.Verify(p => p.GetValue<int>(It.Is<string>(i => i == guid)), Times.Exactly(expectedCallsCount));
        }

        [Test]
        [TestCase(true, 0, 0)]
        [TestCase(true, 10, 10)]
        [TestCase(true, 20, 20)]
        [TestCase(false, 0, 0)]
        [TestCase(false, 10, 10)]
        [TestCase(false, 50, 50)]
        public void Should_call_provider_exactly_N_times_without_interface_method_attribute(bool hasValueInCache, int timesToCallProvider, int expectedCallsCount)
        {
            var container = new WindsorContainer();
            container.Install(new CacheManagerInstaller());
            container.Register(Component.For<ITestProvider>().ImplementedBy<TestProvider>());

            var guid = Guid.NewGuid().ToString();
            var valueStorageMock = new Mock<IValueStorage>();
            RegisterMockObject(container, valueStorageMock);

            if (hasValueInCache)
            {
                valueStorageMock.Setup(m => m.GetValue<int>(It.Is<string>(i => i == guid)))
                    .Returns(1);
            }

            var testProvider = container.Resolve<ITestProvider>();
            for (var i = 0; i < timesToCallProvider; i++)
            {
                var result = testProvider.GetInterfaceWithoutAttribute(guid);
            }

            valueStorageMock.Verify(p => p.GetValue<int>(It.Is<string>(i => i == guid)), Times.Exactly(expectedCallsCount));
        }

        private static void RegisterMockObject(IWindsorContainer container, IMock<IValueStorage> valueStorageMock)
        {
            var storage = new ValueStorageWrapper();
            storage.SetMock(valueStorageMock.Object);
            container.Register(Component.For<IValueStorage>().Instance(storage));
        }
    }
}