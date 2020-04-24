using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using MobTimer.Web.Hubs;

namespace MobTimer.Web.Domain
{
    public interface IRoom {
        void Start();
        void JoinRoom(Member newMember, string connectionId);
        void MemberDisconnected(string connectionId);
        List<Member> GetMembers();
    }

    public class Room : IRoom, IDisposable
    {
        private readonly IMob mob;
        private readonly ITimer timer;
        private readonly IHubContext<TimerHub> hubContext;
        private readonly IDictionary<string, Member> memberIds;

        public Room(IMob mob, ITimer timer, IHubContext<TimerHub> hubContext)
        {
            memberIds = new Dictionary<string, Member>();
            this.mob = mob;
            this.timer = timer;
            this.hubContext = hubContext;
            timer.Elapsed += TimerFinished;
            timer.Tock += TimerTocked;
        }

        private void TimerTocked(Tick tick)
        {
            hubContext.Clients.All.SendAsync("Tock", tick);
        }

        public void Start()
        {
            timer.Start();
        }

        private void TimerFinished()
        {
            var nextMobber = mob.AdvanceDriver();
            hubContext.Clients.All.SendAsync("NextDriver", nextMobber);
        }

        public void JoinRoom(Member member, string connectionId) 
        {
            memberIds.Add(connectionId, member);
            mob.Join(member);
        }

        public void MemberDisconnected(string connectionId) 
        {
            if(memberIds.ContainsKey(connectionId))
            {
                mob.Leave(memberIds[connectionId]);
            }
        }

        public void Dispose()
        {
            timer.Tock -= TimerTocked;
            timer.Elapsed -= TimerFinished;
        }

        public List<Member> GetMembers()
        {
            return mob.GetMembers();
        }
    }
}
