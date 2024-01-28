using System;
using System.Diagnostics;
using GlobalGameJam2024.Scripts.Core;
using GlobalGameJam2024.Scripts.UI;
using Godot;
using Godot.Collections;

namespace GlobalGameJam2024.Scripts.GameObstacles
{
    public class GameObstacleProgressTracker : Singleton<GameObstacleProgressTracker>
    {
        private Dictionary<GameObstacleID, bool> _remainingObstacles = null;


        [Signal]
        public delegate void Satisfied(GameObstacleID obstacleId);

        private const string SatisfiedSignalName = "Satisfied";

        public void ConnectSatisfied(Node listener, string methodName)
        {
            ConnectSignal.Check(this, SatisfiedSignalName, listener, methodName);
        }


        [Signal]
        public delegate void Unsatisfied(GameObstacleID obstacleId);

        private const string UnSatisfiedSignalName = "Unsatisfied";

        public void ConnectUnsatisfied(Node listener, string methodName)
        {
            ConnectSignal.Check(this, UnSatisfiedSignalName, listener, methodName);
        }


        private Dictionary<GameObstacleID, bool> RemainingObstacles
        {
            get
            {
                if (_remainingObstacles == null)
                {
                    Initialize();
                }

                return _remainingObstacles;
            }
        }


        private void Initialize()
        {
            _remainingObstacles = new Dictionary<GameObstacleID, bool>();
        }

        public void AddObstacles(GameObstacleSet obstacleSet)
        {
            foreach (GameObstacleID obstacle in obstacleSet.Obstacles)
            {
                AddObstacle(obstacle);
            }
        }

        public void AddObstacle(GameObstacleID obstacle)
        {
            GD.Print($"Adding obstacle with id {GameObstacleID.GetObstacleID(obstacle)}");
            RemainingObstacles[obstacle] = true;
            EmitSignal(UnSatisfiedSignalName, obstacle);
        }

        public void RemoveObstacle(GameObstacleID obstacle)
        {
            GD.Print($"Removing obstacle with id {GameObstacleID.GetObstacleID(obstacle)}");
            RemainingObstacles[obstacle] = false;
            EmitSignal(SatisfiedSignalName, obstacle);
        }

        public bool SatisfiedObstacle(GameObstacleID obstacleId)
        {
            return RemainingObstacles.TryGetValue(obstacleId, out bool remaining) && !remaining;
        }

        public bool CheckObstacles(GameObstacleSet obstacles, bool expected)
        {
            foreach (GameObstacleID obstacle in obstacles.Obstacles)
            {
                GD.Print($"{GameObstacleID.GetObstacleID(obstacle)}\tsatisfied:{SatisfiedObstacle(obstacle)}");
                if (SatisfiedObstacle(obstacle) != expected)
                {
                    return false;
                }
            }
            return true;
        }

        public bool AllSatisfied(GameObstacleSet obstacles)
        {
            return CheckObstacles(obstacles, true);
        }
        
        public bool NoneSatisfied(GameObstacleSet obstacles)
        {
            return CheckObstacles(obstacles, false);
        }
    }
}