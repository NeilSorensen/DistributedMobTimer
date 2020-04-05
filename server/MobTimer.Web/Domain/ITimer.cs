using System;

namespace MobTimer.Web.Domain
{
    public interface ITimer
    {
        public event Action Elapsed;
        int LengthInMilliseconds { get; }

        public void Start();
        public void Pause();
        public void Reset();

        public void SetLength(int minutes);
    }

    public class Timer : ITimer
    {
        public event Action Elapsed;

        private int timerLengthMinutes;
        private System.Timers.Timer backingTimer;

        public Timer()
        {
            timerLengthMinutes = 1;
            backingTimer = new System.Timers.Timer
            {
                AutoReset = false,
                Enabled = false,
                Interval = LengthInMilliseconds
            };
        }

        public int LengthInMilliseconds => timerLengthMinutes * 60 * 1000;

        public void Start()
        {
            backingTimer.Enabled = true;
        }

        public void Pause()
        {
            backingTimer.Enabled = false;
        }

        public void Reset()
        {
            backingTimer.Stop();
            backingTimer.Start();
        }

        public void SetLength(int minutes)
        {
            timerLengthMinutes = minutes;
            backingTimer.Interval = LengthInMilliseconds;
        }
    }
}
