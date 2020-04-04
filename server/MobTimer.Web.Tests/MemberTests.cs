using MobTimer.Web.Domain;
using NUnit.Framework;

namespace MobTimer.Web.Tests
{
    public class MemberTests
    {
        [Test]
        public void Members_with_the_same_display_name_are_equal()
        {
            var memberOne = new Member {DisplayName = "John Smith"};
            var memberTwo = new Member {DisplayName = "John Smith"};

            Assert.That(memberOne, Is.EqualTo(memberTwo));
            Assert.That(memberTwo, Is.EqualTo(memberOne));
        }
    }
}
