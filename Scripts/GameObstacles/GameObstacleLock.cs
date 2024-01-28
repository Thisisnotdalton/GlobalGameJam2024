using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.GameObstacles
{
    public class GameObstacleLock : Node
    {
        [Export] private GameObstacleSet _requiredSatisfied = null;
        [Export] private GameObstacleSet _requiredUnsatisfied = null;

        [Signal]
        private delegate void Locked(bool locked);

        private const string LockedSignalName = "Locked";

        public void BindLockedSignal(Node listener, string methodName)
        {
            ConnectSignal.Check(this, LockedSignalName, listener, methodName);
            CheckObstacles(null);
        }

        private bool RequirementsMet()
        {
            bool satisfiedRequirementsMet = _requiredSatisfied == null || _requiredSatisfied.Obstacles.Length == 0 ||
                                            GameObstacleProgressTracker.Instance.AllSatisfied(_requiredSatisfied);
            bool unsatisfiedRequirementsMet = _requiredUnsatisfied == null ||
                                              _requiredUnsatisfied.Obstacles.Length == 0 ||
                                              GameObstacleProgressTracker.Instance.NoneSatisfied(_requiredUnsatisfied);
            GD.Print($"{Name} satisfied: {satisfiedRequirementsMet}\tunsatisfied: {unsatisfiedRequirementsMet}");
            return satisfiedRequirementsMet && unsatisfiedRequirementsMet;
        }

        public override void _Ready()
        {
            base._Ready();
            GameObstacleProgressTracker.Instance.ConnectSatisfied(this, "CheckObstacles");
            GameObstacleProgressTracker.Instance.ConnectUnsatisfied(this, "CheckObstacles");
            CheckObstacles(null);
        }

        private void CheckObstacles(GameObstacleID obstacleID)
        {
            bool locked = !RequirementsMet();
            EmitSignal("Locked", locked);
        }
    }
}