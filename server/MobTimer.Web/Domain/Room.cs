using System;

namespace MobTimer.Web.Domain
{
    public class Room : IDisposable
    {
        private readonly IMob mob;
        private readonly ITimer timer;

        public Room(IMob mob, ITimer timer)
        {
            this.mob = mob;
            this.timer = timer;
            timer.Elapsed += TimerFinished;
        }

        private void TimerFinished()
        {
            var nextMobber = mob.AdvanceDriver();
            NextTurn?.Invoke(nextMobber);
        }

        public event Action<Member> NextTurn;

        public void Dispose()
        {
            timer.Elapsed -= TimerFinished;
        }
    }
}
