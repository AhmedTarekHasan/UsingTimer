using System;

namespace BetterTimerApp.Abstractions
{
    public delegate void TimerIntervalElapsedEventHandler(object sender, DateTime dateTime);

    public interface ITimer : IDisposable
    {
        event TimerIntervalElapsedEventHandler TimerIntervalElapsed;

        bool Enabled { get; set; }
        double Interval { get; set; }

        void Start();
        void Stop();
    }
}