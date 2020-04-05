using MobTimer.Web.Domain;
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

            var classUnderTest = new Room(mockMob.Object, mockTimer.Object);

            Member messagedDriver = null;
            classUnderTest.NextTurn += x => messagedDriver = x;

            Member nextMobber = new Member("Alan Wake");
            mockMob.Setup(x => x.AdvanceDriver()).Returns(nextMobber);

            mockTimer.Raise(x => x.Elapsed += null);

            Assert.That(messagedDriver, Is.EqualTo(nextMobber));
        }
    }
}
