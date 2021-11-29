using BetterTimerApp.Abstractions;
using BetterTimerApp.Implementations;

namespace BetterTimerApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            var timer = new Timer();
            IPublisher publisher = new Publisher(timer, new Implementations.Console());
            publisher.StartPublishing();
            System.Console.ReadLine();
            publisher.StopPublishing();
            timer.Dispose();
        }
    }
}