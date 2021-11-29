using System;
using System.Collections.Generic;
using BetterTimerApp.Abstractions;

namespace BetterTimerApp.Tests.Stubs
{
    public enum Action
    {
        Start = 1,
        Stop = 2,
        Triggered = 3,
        Enabled = 4,
        Disabled = 5,
        IntervalSet = 6
    }

    public class ActionLog
    {
        public Action Action { get; }
        public string Message { get; }

        public ActionLog(Action action, string message)
        {
            Action = action;
            Message = message;
        }
    }

    public class TimerStub : ITimer
    {
        private bool m_Enabled;
        private double m_Interval;

        public event TimerIntervalElapsedEventHandler TimerIntervalElapsed;

        public Dictionary<int, ActionLog> Log = new();

        public bool Enabled
        {
            get => m_Enabled;
            set
            {
                m_Enabled = value;

                Log.Add(Log.Count + 1,
                    new ActionLog(value ? Action.Enabled : Action.Disabled, value ? "Enabled" : "Disabled"));
            }
        }

        public double Interval
        {
            get => m_Interval;
            set
            {
                m_Interval = value;
                Log.Add(Log.Count + 1, new ActionLog(Action.IntervalSet, m_Interval.ToString("G17")));
            }
        }

        public void Start()
        {
            Log.Add(Log.Count + 1, new ActionLog(Action.Start, "Started"));
        }

        public void Stop()
        {
            Log.Add(Log.Count + 1, new ActionLog(Action.Stop, "Stopped"));
        }

        public void TriggerTimerIntervalElapsed(DateTime dateTime)
        {
            OnTimerIntervalElapsed(dateTime);
            Log.Add(Log.Count + 1, new ActionLog(Action.Triggered, "Triggered"));
        }

        protected void OnTimerIntervalElapsed(DateTime dateTime)
        {
            TimerIntervalElapsed?.Invoke(this, dateTime);
        }

        public void Dispose()
        {
            Log.Clear();
            Log = null;
        }
    }
}