using System;
using System.Collections.Generic;
using Moq;
using Prism.Ioc;
using Prism.Logging;
using Prism.Logging.AppCenter;
using Xunit;

namespace Prism.Plugin.Logging.Tests.Registration
{
    public class AppCenterRegistrationTests
    {
        [Fact]
        public void RegistersAppCenterLoggerWithAppSecret()
        {
            var mockContainer = new Mock<IContainerExtension>();
            mockContainer.Object.RegisterAppCenterLogger(Guid.NewGuid().ToString());

            mockContainer.Verify(c => c.RegisterManySingleton(typeof(AppCenterLogger),
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger)));
        }

        [Fact]
        public void AppCenterLoggerRegisteredWithAggregateLogger()
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
            mockContainer.Setup(c => c.Resolve(typeof(AggregateLogger)))
                .Returns(new AggregateLogger());
            mockContainer.Setup(c => c.IsRegistered(typeof(ILogger)))
                .Returns(false);
            mockContainer.Object.RegisterAppCenterLogger(Guid.NewGuid().ToString());
            mockContainer.Object.UseAggregateLogger();

            mockContainer.Verify(c => c.RegisterManySingleton(typeof(AppCenterLogger), typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger)), Times.Once);

            mockContainer.Verify(c => c.RegisterSingleton(typeof(AggregateLogger), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(ILogger), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(IAnalyticsService), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(ICrashesService), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(IAggregateLogger), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
        }

        [Fact]
        public void AggregateLoggerReplacesAppCenterLogger()
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

            mockContainer.Object.RegisterAppCenterLogger(Guid.NewGuid().ToString());
            mockContainer.Object.UseAggregateLogger();

            mockContainer.Verify(c => c.RegisterManySingleton(typeof(AppCenterLogger),
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
