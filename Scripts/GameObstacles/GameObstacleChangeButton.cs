using Godot;

namespace GlobalGameJam2024.Scripts.GameObstacles
{
    public class GameObstacleChangeButton : Button
    {
        [Export] private bool _removeObstacle = true;
        [Export] private GameObstacleID _obstacleId = null;

        public override void _Pressed()
        {
            base._Pressed();
            if (_obstacleId != null)
            {
                if (_removeObstacle)
                {
                    GameObstacleProgressTracker.Instance.RemoveObstacle(_obstacleId);
                }
                else
                {
                    GameObstacleProgressTracker.Instance.AddObstacle(_obstacleId);
                }
            }
        }
    }
}