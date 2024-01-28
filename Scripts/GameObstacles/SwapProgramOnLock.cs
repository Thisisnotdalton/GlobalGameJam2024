using GlobalGameJam2024.Scripts.Core;
using GlobalGameJam2024.Scripts.UI;
using Godot;

namespace GlobalGameJam2024.Scripts.GameObstacles
{
    public class SwapProgramOnLock : Node
    {
        [Export] private ProgramResource _lockedProgramResource = null;
        [Export] private ProgramResource _unlockedProgramResource = null;
        [Export] private NodePath _applicationIconPath = new NodePath(".");
        private ApplicationIcon _applicationIcon;

        public override void _Ready()
        {
            _applicationIcon = GetNode<ApplicationIcon>(_applicationIconPath);
            GameObstacleLock obstacleLock = SearchNodeType.FindChildOfType<GameObstacleLock>(this);
            obstacleLock.BindLockedSignal(this, "ToggleLocked");
        }

        private void ToggleLocked(bool locked)
        {
            _applicationIcon.Initialize(locked ? _lockedProgramResource : _unlockedProgramResource);
        }
    }
}