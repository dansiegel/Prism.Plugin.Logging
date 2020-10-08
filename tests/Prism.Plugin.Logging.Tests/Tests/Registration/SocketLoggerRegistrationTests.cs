using System;
using System.Collections.Generic;
using Moq;
using Prism.Ioc;
using Prism.Logging;
using Prism.Logging.Sockets;
using Xunit;

namespace Prism.Plugin.Logging.Tests.Registration
{
    public class SocketLoggerRegistrationTests
    {
        [Fact]
        public void RegistersSocketLoggerWithHostAndPort()
        {
            var mockContainer = new Mock<IContainerExtension>();
            mockContainer.Object.RegisterSocketLogger("localhost", 8080);

            mockContainer.Verify(c => c.RegisterInstance(typeof(ISocketLoggerOptions), It.IsAny<SocketLoggerOptions>()));

            mockContainer.Verify(c => c.RegisterManySingleton(typeof(SocketLogger),
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger)));
        }

        [Fact]
        public void RegistersSocketLoggerWithDelegateConfiguration()
        {
            var mockContainer = new Mock<IContainerExtension>();
            mockContainer.Object.RegisterSocketLogger(o =>
            {
                o.HostOrIp = "localhost";
                o.Port = 8080;
            });

            mockContainer.Verify(c => c.RegisterInstance(typeof(ISocketLoggerOptions), It.IsAny<SocketLoggerOptions>()));

            mockContainer.Verify(c => c.RegisterManySingleton(typeof(SocketLogger),
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger)));
        }

        [Fact]
        public void SocketLoggerRegisteredWithAggregateLogger()
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
            mockContainer.Object.RegisterSocketLogger("localhost", 8080);

            mockContainer.Verify(c => c.RegisterInstance(typeof(ISocketLoggerOptions), It.IsAny<SocketLoggerOptions>()), Times.Once);
            mockContainer.Verify(c => c.RegisterSingleton(typeof(IAggregableLogger), typeof(SocketLogger)), Times.Once);

            mockContainer.Verify(c => c.RegisterSingleton(typeof(AggregateLogger), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(ILogger), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(IAnalyticsService), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(ICrashesService), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(IAggregateLogger), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
        }

        [Fact]
        public void AggregateLoggerReplacesSocketLogger()
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

            mockContainer.Object.RegisterSocketLogger("localhost", 8080);
            mockContainer.Object.UseAggregateLogger();

            mockContainer.Verify(c => c.RegisterInstance(typeof(ISocketLoggerOptions), It.IsAny<SocketLoggerOptions>()), Times.Once);
            mockContainer.Verify(c => c.RegisterManySingleton(typeof(SocketLogger),
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
