using System;
using System.Collections.Generic;
using MobTimer.Web.Domain;
using NUnit.Framework;

namespace MobTimer.Web.Tests
{
    public class MobTests
    {
        private Mob classUnderTest;
        private Member tonyStark;
        private Member clintBarton;
        private Member natashaRomanov;

        [SetUp]
        public void AvengersAssemble()
        {
            classUnderTest = new Mob();

            tonyStark = new Member {DisplayName = "IronMan"};
            clintBarton = new Member {DisplayName = "Hawkeye"};
            natashaRomanov = new Member {DisplayName = "Black Widow"};
            classUnderTest.Join(tonyStark);
            classUnderTest.Join(clintBarton);
            classUnderTest.Join(natashaRomanov);

        }

        [Test]
        public void A_member_should_be_able_to_join_a_mob()
        {
            var members = classUnderTest.GetMembers();

            Assert.That(members, Is.EquivalentTo(new []{tonyStark, clintBarton, natashaRomanov}));
        }

        [Test]
        public void All_members_should_get_a_chance_to_drive()
        {
            var electedDrivers = GetNextThreeDrivers();

            Assert.That(electedDrivers, Does.Contain(tonyStark));
            Assert.That(electedDrivers, Does.Contain(clintBarton));
            Assert.That(electedDrivers, Does.Contain(natashaRomanov));
        }

        [Test]
        public void Advancing_drivers_should_never_reach_the_end()
        {
            for (int i = 0; i < 100; i++)
            {
                classUnderTest.AdvanceDriver();
            }

            Assert.That(classUnderTest, Is.Not.Null);
        }

        [Test]
        public void Afk_members_should_not_be_the_driver()
        {
            classUnderTest.SetAfk(tonyStark);

            var electedDrivers = GetNextThreeDrivers();

            Assert.That(electedDrivers, Does.Not.Contain(tonyStark));
        }

        [Test]
        public void Everyone_going_afk_should_not_cause_an_infinite_loop()
        {
            classUnderTest.SetAfk(tonyStark);
            classUnderTest.SetAfk(clintBarton);
            classUnderTest.SetAfk(natashaRomanov);

            var electedDrivers = GetNextThreeDrivers();

            Assert.That(electedDrivers, Is.Not.Empty);
        }

        [Test]
        public void A_member_should_be_able_to_leave_the_mob()
        {
            classUnderTest.Leave(tonyStark);

            Assert.That(classUnderTest.GetMembers(), Is.EquivalentTo(new [] {clintBarton, natashaRomanov}));
        }

        [Test]
        public void When_all_members_leave_a_mob_it_is_no_longer_active()
        {
            classUnderTest.Leave(tonyStark);
            classUnderTest.Leave(clintBarton);
            classUnderTest.Leave(natashaRomanov);

            Assert.That(classUnderTest.IsActive(), Is.False);
        }

        [Test]
        public void An_inactive_mob_cannot_advance_drivers()
        {
            classUnderTest.Leave(tonyStark);
            classUnderTest.Leave(clintBarton);
            classUnderTest.Leave(natashaRomanov);

            Assert.Throws<InvalidOperationException>(() => classUnderTest.AdvanceDriver());
        }

        private List<Member> GetNextThreeDrivers()
        {
            return new List<Member>
            {
                classUnderTest.AdvanceDriver(),
                classUnderTest.AdvanceDriver(),
                classUnderTest.AdvanceDriver()
            };
        }
    }
}
