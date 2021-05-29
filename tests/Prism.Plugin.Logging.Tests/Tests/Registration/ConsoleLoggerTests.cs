using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Prism.Ioc;
using Prism.Logging;
using Xunit;

namespace Prism.Plugin.Logging.Tests.Registration
{
    public class ConsoleLoggerTests
    {
        [Fact]
        public void RegistersConsoleLogger()
        {
            var mockContainer = new Mock<IContainerExtension>();
            mockContainer.Object.RegisterConsoleLogger();

            mockContainer.Verify(c => c.RegisterManySingleton(typeof(ConsoleLoggingService),
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger)));
        }

        [Fact]
        public void ServiceCollectionAddsConsoleLogger()
        {
            var services = new ServiceCollection();
            services.AddConsoleLogger();

            Assert.Contains(services, s =>
                s.Lifetime == ServiceLifetime.Singleton &&
                s.ImplementationType == typeof(ConsoleLoggingService));

            Assert.Contains(services, s =>
                s.Lifetime == ServiceLifetime.Singleton &&
                s.ServiceType == typeof(IAnalyticsService) &&
                s.ImplementationFactory != null);

            Assert.Contains(services, s =>
                s.Lifetime == ServiceLifetime.Singleton &&
                s.ServiceType == typeof(ICrashesService) &&
                s.ImplementationFactory != null);

            Assert.Contains(services, s =>
                s.Lifetime == ServiceLifetime.Singleton &&
                s.ServiceType == typeof(ILogger) &&
                s.ImplementationFactory != null);

            Assert.Contains(services, s =>
                s.Lifetime == ServiceLifetime.Singleton &&
                s.ServiceType == typeof(IAggregableLogger) &&
                s.ImplementationFactory != null);
        }

        [Fact]
        public void ConsoleLoggerRegisteredWithAggregateLogger()
        {
            var mockContainer = new Mock<IContainerExtension>();
            mockContainer.Setup(c => c.RegisterManySingleton(It.IsAny<Type>(), It.IsAny<Type[]>()))
                .Returns(mockContainer.Object);
            mockContainer.Setup(c => c.RegisterSingleton(It.IsAny<Type>(), It.IsAny<Func<IContainerProvider, object>>()))
                .Returns(mockContainer.Object);
            mockContainer.Setup(c => c.RegisterSingleton(It.IsAny<Type>(), It.IsAny<Type>()))
                .Returns(mockContainer.Object);
            mockContainer.Setup(c => c.Register(It.IsAny<Type>(), It.IsAny<Func<IContainerProvider, object>>()))
                .Returns(mockContainer.Object);
            mockContainer.Setup(c => c.Resolve(typeof(IEnumerable<IAggregableLogger>)))
                .Returns(Array.Empty<IAggregableLogger>());
            mockContainer.Setup(c => c.Resolve(typeof(AggregateLogger)))
                .Returns(new AggregateLogger());
            mockContainer.Setup(c => c.IsRegistered(typeof(ILogger)))
                .Returns(false);
            mockContainer.Setup(c => c.IsRegistered(typeof(IAggregateLogger)))
                .Returns(true);

            mockContainer.Object.UseAggregateLogger();
            mockContainer.Object.RegisterConsoleLogger();

            mockContainer.Verify(c => c.RegisterSingleton(typeof(IAggregableLogger), typeof(ConsoleLoggingService)), Times.Once);

            mockContainer.Verify(c => c.RegisterSingleton(typeof(AggregateLogger), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(ILogger), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(IAnalyticsService), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(ICrashesService), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(IAggregateLogger), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
        }

        [Fact]
        public void AggregateLoggerReplacesConsoleLogger()
        {
            var mockContainer = new Mock<IContainerExtension>();
            mockContainer.Setup(c => c.RegisterManySingleton(It.IsAny<Type>(), It.IsAny<Type[]>()))
                .Returns(mockContainer.Object);
            mockContainer.Setup(c => c.RegisterSingleton(It.IsAny<Type>(), It.IsAny<Func<IContainerProvider, object>>()))
                .Returns(mockContainer.Object);
            mockContainer.Setup(c => c.Register(It.IsAny<Type>(), It.IsAny<Func<IContainerProvider, object>>()))
                .Returns(mockContainer.Object);
            mockContainer.Setup(c => c.Resolve(typeof(IEnumerable<IAggregableLogger>)))
                .Returns(Array.Empty<IAggregableLogger>());
            mockContainer.As<IContainerProvider>().Setup(c => c.Resolve(typeof(AggregateLogger)))
                .Returns(new AggregateLogger());
            mockContainer.Setup(c => c.IsRegistered(typeof(ILogger)))
                .Returns(true);
            mockContainer.Setup(c => c.IsRegistered(typeof(IAggregateLogger)))
                .Returns(false);

            mockContainer.Object.RegisterConsoleLogger();
            mockContainer.Object.UseAggregateLogger();

            mockContainer.Verify(c => c.RegisterManySingleton(typeof(ConsoleLoggingService),
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger)));

            mockContainer.Verify(c => c.RegisterSingleton(typeof(AggregateLogger), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(ILogger), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(IAnalyticsService), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(ICrashesService), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(IAggregateLogger), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
        }
    }
}
