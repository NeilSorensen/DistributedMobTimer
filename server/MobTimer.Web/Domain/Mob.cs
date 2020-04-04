using System;
using System.Collections.Generic;
using System.Linq;

namespace MobTimer.Web.Domain
{
    public class Mob
    {
        private List<MemberStatus> members;
        private object memberLock;
        private int currentDriver;

        public Mob()
        {
            members = new List<MemberStatus>();
            memberLock = new object();
            currentDriver = -1;
        }

        public void Join(Member newMember)
        {
            lock (memberLock)
            {
                members.Add(new MemberStatus(newMember, Status.Mobbing));
            }
        }

        public List<Member> GetMembers()
        {
            return members.Select(x => x.Member).ToList();
        }

        public Member AdvanceDriver()
        {
            lock (memberLock)
            {
                if (!IsActive())
                {
                    throw new InvalidOperationException("The mob has no members, and so cannot be advanced");
                }
                do
                {
                    currentDriver++;
                    if (currentDriver == members.Count)
                    {
                        currentDriver = 0;
                    }
                } while (CurrentDriverIsInvalidAndAValidDriverIsInTheMob());

                return members[currentDriver].Member;
            }
        }

        private bool CurrentDriverIsInvalidAndAValidDriverIsInTheMob()
        {
            return members[currentDriver].Status == Status.Afk && members.Any(x => x.Status != Status.Afk);
        }

        public void SetAfk(Member member)
        {
            lock (memberLock)
            {
                members.Single(x => Equals(x.Member, member)).Status = Status.Afk;
            }
        }

        public void Leave(Member leavingMember)
        {
            lock (memberLock)
            {
                var matching = members.SingleOrDefault(x => Equals(x.Member, leavingMember));
                members.Remove(matching);
            }
        }

        public bool IsActive()
        {
            return members.Any();
        }
    }
}
