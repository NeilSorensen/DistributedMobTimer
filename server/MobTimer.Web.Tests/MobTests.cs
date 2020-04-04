using System.Collections.Generic;
using MobTimer.Web.Domain;
using NUnit.Framework;

namespace MobTimer.Web.Tests
{
    public class MobTests
    {
        [Test]
        public void A_member_should_be_able_to_join_a_mob()
        {
            var classUnderTest = new Mob();

            var tonyStark = new Member {DisplayName = "IronMan"};
            var clintBarton = new Member {DisplayName = "Hawkeye"};
            var natashaRomanov = new Member {DisplayName = "Black Widow"};
            classUnderTest.Join(tonyStark);
            classUnderTest.Join(clintBarton);
            classUnderTest.Join(natashaRomanov);

            var members = classUnderTest.GetMembers();
            Assert.That(members, Is.EquivalentTo(new []{tonyStark, clintBarton, natashaRomanov}));
        }

        [Test]
        public void All_members_should_get_a_chance_to_drive()
        {
            var classUnderTest = new Mob();

            var tonyStark = new Member {DisplayName = "IronMan"};
            var clintBarton = new Member {DisplayName = "Hawkeye"};
            var natashaRomanov = new Member {DisplayName = "Black Widow"};
            classUnderTest.Join(tonyStark);
            classUnderTest.Join(clintBarton);
            classUnderTest.Join(natashaRomanov);

            var electedDrivers = new List<Member>
            {
                classUnderTest.AdvanceDriver(),
                classUnderTest.AdvanceDriver(),
                classUnderTest.AdvanceDriver()
            };

            Assert.That(electedDrivers, Does.Contain(tonyStark));
            Assert.That(electedDrivers, Does.Contain(clintBarton));
            Assert.That(electedDrivers, Does.Contain(natashaRomanov));
        }

        [Test]
        public void Advancing_drivers_should_never_reach_the_end()
        {
            var classUnderTest = new Mob();

            var tonyStark = new Member {DisplayName = "IronMan"};
            var clintBarton = new Member {DisplayName = "Hawkeye"};
            var natashaRomanov = new Member {DisplayName = "Black Widow"};
            classUnderTest.Join(tonyStark);
            classUnderTest.Join(clintBarton);
            classUnderTest.Join(natashaRomanov);

            for (int i = 0; i < 100; i++)
            {
                classUnderTest.AdvanceDriver();
            }

            Assert.That(classUnderTest, Is.Not.Null);
        }

        [Test]
        public void Afk_members_should_not_be_the_driver()
        {
            var classUnderTest = new Mob();

            var tonyStark = new Member {DisplayName = "IronMan"};
            var clintBarton = new Member {DisplayName = "Hawkeye"};
            var natashaRomanov = new Member {DisplayName = "Black Widow"};
            classUnderTest.Join(tonyStark);
            classUnderTest.Join(clintBarton);
            classUnderTest.Join(natashaRomanov);

            classUnderTest.SetAfk(tonyStark);

            var electedDrivers = new List<Member>
            {
                classUnderTest.AdvanceDriver(),
                classUnderTest.AdvanceDriver(),
                classUnderTest.AdvanceDriver()
            };

            Assert.That(electedDrivers, Does.Not.Contain(tonyStark));
        }

        [Test]
        public void Everyone_going_afk_should_not_cause_an_infinite_loop()
        {
            var classUnderTest = new Mob();

            var tonyStark = new Member {DisplayName = "IronMan"};
            var clintBarton = new Member {DisplayName = "Hawkeye"};
            var natashaRomanov = new Member {DisplayName = "Black Widow"};
            classUnderTest.Join(tonyStark);
            classUnderTest.Join(clintBarton);
            classUnderTest.Join(natashaRomanov);

            classUnderTest.SetAfk(tonyStark);
            classUnderTest.SetAfk(clintBarton);
            classUnderTest.SetAfk(natashaRomanov);

            var electedDrivers = new List<Member>
            {
                classUnderTest.AdvanceDriver(),
                classUnderTest.AdvanceDriver(),
                classUnderTest.AdvanceDriver()
            };

            Assert.That(electedDrivers, Is.Not.Empty);
        }
    }
}
