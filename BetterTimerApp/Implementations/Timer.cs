using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using BetterTimerApp.Abstractions;

namespace BetterTimerApp.Implementations
{
    public class Timer : ITimer
    {
        private Dictionary<TimerIntervalElapsedEventHandler, List<ElapsedEventHandler>> m_Handlers = new();
        private bool m_IsDisposed;
        private System.Timers.Timer m_Timer;

        public Timer()
        {
            m_Timer = new System.Timers.Timer();
        }

        public event TimerIntervalElapsedEventHandler TimerIntervalElapsed
        {
            add
            {
                var internalHandler =
                    (ElapsedEventHandler)((sender, args) => { value.Invoke(sender, args.SignalTime); });

                if (!m_Handlers.ContainsKey(value))
                {
                    m_Handlers.Add(value, new List<ElapsedEventHandler>());
                }

                m_Handlers[value].Add(internalHandler);

                m_Timer.Elapsed += internalHandler;
            }

            remove
            {
                m_Timer.Elapsed -= m_Handlers[value].Last();

                m_Handlers[value].RemoveAt(m_Handlers[value].Count - 1);

                if (!m_Handlers[value].Any())
                {
                    m_Handlers.Remove(value);
                }
            }
        }

        public bool Enabled
        {
            get => m_Timer.Enabled;
            set => m_Timer.Enabled = value;
        }

        public double Interval
        {
            get => m_Timer.Interval;
            set => m_Timer.Interval = value;
        }

        public void Start()
        {
            m_Timer.Start();
        }

        public void Stop()
        {
            m_Timer.Stop();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (m_IsDisposed) return;

            if (disposing && m_Handlers.Any())
            {
                foreach (var internalHandlers in m_Handlers.Values)
                {
                    if (internalHandlers?.Any() ?? false)
                    {
                        internalHandlers.ForEach(handler => m_Timer.Elapsed -= handler);
                    }
                }

                m_Timer.Dispose();
                m_Timer = null;
                m_Handlers.Clear();
                m_Handlers = null;
            }

            m_IsDisposed = true;
        }

        ~Timer()
        {
            Dispose(false);
        }
    }
}