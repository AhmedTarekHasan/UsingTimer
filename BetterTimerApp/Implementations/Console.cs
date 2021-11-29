using BetterTimerApp.Abstractions;

namespace BetterTimerApp.Implementations
{
    public class Console : IConsole
    {
        public void WriteLine(object? value)
        {
            System.Console.WriteLine(value);
        }
    }
}