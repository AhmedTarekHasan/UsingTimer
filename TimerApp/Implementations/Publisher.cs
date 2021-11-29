using System.Timers;
using TimerApp.Abstractions;

namespace TimerApp.Implementations
{
    public class Publisher : IPublisher
    {
        private readonly Timer m_Timer;
        private readonly IConsole m_Console;

        public Publisher(IConsole console)
        {
            m_Timer = new Timer();
            m_Timer.Enabled = true;
            m_Timer.Interval = 1000;
            m_Timer.Elapsed += Handler;

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

        private void Handler(object sender, ElapsedEventArgs args)
        {
            m_Console.WriteLine(args.SignalTime);
        }
    }
}