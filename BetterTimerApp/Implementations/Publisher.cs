using System;
using BetterTimerApp.Abstractions;

namespace BetterTimerApp.Implementations
{
    public class Publisher : IPublisher
    {
        private readonly ITimer m_Timer;
        private readonly IConsole m_Console;

        public Publisher(ITimer timer, IConsole console)
        {
            m_Timer = timer;
            m_Timer.Enabled = true;
            m_Timer.Interval = 1000;
            m_Timer.TimerIntervalElapsed += Handler;

            m_Console = console;
        }

        public void StartPublishing()
        {
            m_Timer.Start();
        }

        public void StopPublishing()
        {
            m_Timer.Stop();
        }

        private void Handler(object sender, DateTime dateTime)
        {
            m_Console.WriteLine(dateTime);
        }
    }
}