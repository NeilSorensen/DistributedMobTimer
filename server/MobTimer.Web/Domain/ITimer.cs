using System;
using System.Timers;

namespace MobTimer.Web.Domain
{
    public interface ITimer
    {
        event Action Elapsed;
        event Action<Tick> Tock;

        int LengthInMilliseconds { get; }

        void Start();
        void Pause();
        void Reset();
        void SetLength(int minutes);
    }

    public class Timer : ITimer
    {
        public event Action Elapsed;
        public event Action<Tick> Tock;

        private int timerLengthMinutes;

        private int totalTicks;

        private int remainingTicks;

        private const int TickInterval = 500;

        private System.Timers.Timer backingTimer;

        public Timer()
        {
            SetLength(1);
            backingTimer = new System.Timers.Timer
            {
                AutoReset = true,
                Enabled = false,
                Interval = TickInterval,
            };
            backingTimer.Elapsed += TickFired;
        }

        private void TickFired(object sender, ElapsedEventArgs e)
        {
            remainingTicks--;
            var eventData = new Tick(TickInterval, remainingTicks, totalTicks);
            Tock?.Invoke(eventData);
            if (remainingTicks == 0)
            {
                Elapsed?.Invoke();
            }
        }

        public int LengthInMilliseconds => timerLengthMinutes * 60 * 1000;

        public void Start()
        {
            backingTimer.Start();
        }

        public void Pause()
        {
            backingTimer.Stop();
        }

        public void Reset()
        {
            backingTimer.Stop();
            remainingTicks = totalTicks;
            backingTimer.Start();
        }

        public void SetLength(int minutes)
        {
            timerLengthMinutes = minutes;
            totalTicks = timerLengthMinutes * 60000 / TickInterval;
            remainingTicks = totalTicks;
        }
    }

    public class Tick
    {
        public Tick(int tickInterval, int ticksRemaining, int totalTicks) 
        {
            Remaining = TimeSpan.FromMilliseconds(tickInterval * ticksRemaining);
            if (ticksRemaining >= 0)
                PercentElapsed = (decimal)(totalTicks - ticksRemaining) / totalTicks;
            else
                PercentElapsed = 1;
            PercentElapsed = PercentElapsed * 100;
        }
        public TimeSpan Remaining { get; }
        public decimal PercentElapsed { get; }
    }
}
