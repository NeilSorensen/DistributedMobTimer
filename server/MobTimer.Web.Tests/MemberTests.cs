﻿using MobTimer.Web.Domain;
using NUnit.Framework;

namespace MobTimer.Web.Tests
{
    public class MemberTests
    {
        [Test]
        public void Members_with_the_same_display_name_are_equal()
        {
            var memberOne = new Member("John Smith");
            var memberTwo = new Member("John Smith");

            Assert.That(memberOne, Is.EqualTo(memberTwo));
            Assert.That(memberTwo, Is.EqualTo(memberOne));
        }

        [Test]
        public void Equal_members_should_have_equal_hashcodes()
        {
            var memberOne = new Member("John Smith");
            var memberTwo = new Member("John Smith");

            Assert.That(memberOne.GetHashCode(), Is.EqualTo(memberTwo.GetHashCode()));
        }
    }
}
