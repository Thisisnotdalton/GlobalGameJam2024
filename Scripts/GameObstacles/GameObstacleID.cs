using Godot;

namespace GlobalGameJam2024.Scripts.GameObstacles
{
    public class GameObstacleID : Resource
    {
        public static string GetObstacleID(GameObstacleID obstacle)
        {
            return obstacle.ResourcePath;
        }
    }
}