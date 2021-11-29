namespace BetterTimerApp.Abstractions
{
    public interface IPublisher
    {
        void StartPublishing();
        void StopPublishing();
    }
}