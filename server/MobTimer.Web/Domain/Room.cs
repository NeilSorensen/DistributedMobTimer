using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR;
using MobTimer.Web.Hubs;

namespace MobTimer.Web.Domain
{
    public interface IRoom {
        void Start();
        void JoinRoom(Member newMember);
        List<Member> GetMembers();
    }

    public class Room : IRoom, IDisposable
    {
        private readonly IMob mob;
        private readonly ITimer timer;
        private readonly IHubContext<TimerHub> hubContext;

        public Room(IMob mob, ITimer timer, IHubContext<TimerHub> hubContext)
        {
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

        public void JoinRoom(Member member) 
        {
            mob.Join(member);
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
