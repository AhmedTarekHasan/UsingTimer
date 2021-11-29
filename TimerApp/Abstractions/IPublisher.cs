namespace TimerApp.Abstractions
{
    public interface IPublisher
    {
        void StartPublishing();
        void StopPublishing();
    }
}