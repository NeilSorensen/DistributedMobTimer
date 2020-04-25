using System.Threading;
using Microsoft.AspNetCore.SignalR;
using MobTimer.Web.Domain;
using MobTimer.Web.Hubs;
using Moq;
using NUnit.Framework;

namespace MobTimer.Web.Tests
{
    public class RoomTests
    {
        [Test]
        public void When_the_timer_elapses_the_driver_should_advance()
        {
            var mockTimer = new Mock<ITimer>();
            var mockMob = new Mock<IMob>();
            var mockHubContext = new Mock<IMobMessenger>();

            var classUnderTest = new Room(mockMob.Object, mockTimer.Object, mockHubContext.Object);

            Member nextMobber = new Member("Alan Wake");
            mockMob.Setup(x => x.AdvanceDriver()).Returns(nextMobber);

            mockTimer.Raise(x => x.Elapsed += null);

            mockHubContext.Verify(x => x.NextDriver(nextMobber));
            //mockHubContext.AssertSentToAll("NextDriver", nextMobber);
        }
    }

    public class MockHubContext<T> where T : Hub
    {
        Mock<IHubContext<T>> contextMock;
        Mock<IHubClients> hubClientsMock;

        Mock<IClientProxy> allClientsMock;

        public MockHubContext()
        {
            contextMock = new Mock<IHubContext<T>>();
            hubClientsMock = new Mock<IHubClients>();
            contextMock.Setup(x => x.Clients).Returns(hubClientsMock.Object);
            allClientsMock = new Mock<IClientProxy>();
            hubClientsMock.Setup(x => x.All).Returns(allClientsMock.Object);
        }

        public IHubContext<T> GetMockHubContext()
        {
            return contextMock.Object;
        }

        public void AssertSentToAll(string name, object data)
        {
            allClientsMock.Verify(x => x.SendCoreAsync(name, new[]{data}, It.IsAny<CancellationToken>()));
        }
    }
}
