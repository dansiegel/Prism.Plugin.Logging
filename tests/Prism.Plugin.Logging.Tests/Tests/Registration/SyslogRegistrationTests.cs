using System;
using System.Collections.Generic;
using Moq;
using Prism.Ioc;
using Prism.Logging;
using Prism.Logging.Syslog;
using Xunit;

namespace Prism.Plugin.Logging.Tests.Registration
{
    public class SyslogRegistrationTests
    {
        [Fact]
        public void RegistersSyslogLoggerWithHostAndPort()
        {
            var mockContainer = new Mock<IContainerExtension>();
            mockContainer.Object.RegisterSyslogLogger("localhost", 8080);

            mockContainer.Verify(c => c.RegisterInstance(typeof(ISyslogOptions), It.IsAny<SyslogOptions>()));

            mockContainer.Verify(c => c.RegisterManySingleton(typeof(SyslogLogger),
                typeof(ISyslogLogger),
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger)));
        }

        [Fact]
        public void RegistersSyslogLoggerWithDelegateConfiguration()
        {
            var mockContainer = new Mock<IContainerExtension>();
            mockContainer.Object.RegisterSyslogLogger(o =>
            {
                o.AppNameOrTag = "UnitTest";
                o.Port = 8080;
            });

            mockContainer.Verify(c => c.RegisterInstance(typeof(ISyslogOptions), It.IsAny<SyslogOptions>()));

            mockContainer.Verify(c => c.RegisterManySingleton(typeof(SyslogLogger),
                typeof(ISyslogLogger),
                typeof(IAnalyticsService),
                typeof(ICrashesService),
                typeof(ILogger),
                typeof(IAggregableLogger)));
        }

        [Fact]
        public void SyslogLoggerRegisteredWithAggregateLogger()
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
            mockContainer.Object.RegisterSyslogLogger("localhost", 8080);

            mockContainer.Verify(c => c.RegisterInstance(typeof(ISyslogOptions), It.IsAny<SyslogOptions>()), Times.Once);
            mockContainer.Verify(c => c.RegisterSingleton(typeof(ISyslogLogger), typeof(SyslogLogger)), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(IAggregableLogger), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);

            mockContainer.Verify(c => c.RegisterSingleton(typeof(AggregateLogger), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(ILogger), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(IAnalyticsService), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(ICrashesService), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
            mockContainer.Verify(c => c.Register(typeof(IAggregateLogger), It.IsAny<Func<IContainerProvider, object>>()), Times.Once);
        }

        [Fact]
        public void AggregateLoggerReplacesSyslogLogger()
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

            mockContainer.Object.RegisterSyslogLogger("localhost", 8080);
            mockContainer.Object.UseAggregateLogger();

            mockContainer.Verify(c => c.RegisterInstance(typeof(ISyslogOptions), It.IsAny<SyslogOptions>()), Times.Once);
            mockContainer.Verify(c => c.RegisterManySingleton(typeof(SyslogLogger),
                typeof(ISyslogLogger),
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
