using Godot;

namespace GlobalGameJam2024.Scripts.GameObstacles
{
    public class GameObstacleInitializer : Node
    {
        [Export] private GameObstacleSet _initialObstacles = null;

        public override void _Ready()
        {
            if (_initialObstacles != null)
            {
                GD.Print($"Adding obstacles from {Name}");
                GameObstacleProgressTracker.Instance.AddObstacles(_initialObstacles);
            }

            QueueFree();
        }
    }
}