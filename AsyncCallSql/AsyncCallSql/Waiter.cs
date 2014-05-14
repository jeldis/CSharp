using System;
using System.Threading;


namespace AsyncCallSql
{
    /// <summary>
    /// http://magnusmartensson.com/howto-wait-in-a-workerrole-using-system-timers-timer-and-system-threading-eventwaithandle-over-system-threading-thread-sleep
    /// </summary>
    public class Waiter
    {
        private readonly System.Timers.Timer _timer;

        private readonly EventWaitHandle _waitHandle;

        public Waiter(TimeSpan? interval = null)
        {
            _waitHandle = new AutoResetEvent(false);
            _timer = new System.Timers.Timer();
            _timer.Elapsed += (sender, args) => _waitHandle.Set();
            SetInterval(interval);
        }

        public TimeSpan Interval
        {
            set
            {
                _timer.Interval = value.TotalMilliseconds;
            }
        }

        public void Wait(TimeSpan? newInterval = null)
        {
            SetInterval(newInterval);
            _timer.Start();
            _waitHandle.WaitOne();
            _timer.Close();
            _waitHandle.Reset();
        }

        private void SetInterval(TimeSpan? newInterval)
        {
            if (newInterval.HasValue)
            {
                Interval = newInterval.Value;
            }
        }
    }
}