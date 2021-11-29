using TimerApp.Abstractions;
using TimerApp.Implementations;

namespace TimerApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            IPublisher publisher = new Publisher(new Implementations.Console());
            publisher.StartPublishing();
            System.Console.ReadLine();
            publisher.StopPublishing();
        }
    }
}