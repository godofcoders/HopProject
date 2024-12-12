
namespace Ball.States
{
    public interface IBallState 
    {
        void HandleInput();
        void UpdateState();
    }
}
