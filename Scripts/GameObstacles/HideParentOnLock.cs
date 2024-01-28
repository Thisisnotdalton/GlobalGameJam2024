using System;
using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.GameObstacles
{
    public class HideParentOnLock : Node
    {
        [Export] private NodePath _unlockedNodeParentPath;

        private Node _unlockedNodeParent = null;

        private Node UnlockedNodeParent
        {
            get
            {
                if (_unlockedNodeParent == null)
                {
                    if (_unlockedNodeParentPath == null)
                    {
                        _unlockedNodeParentPath = new NodePath("../..");
                    }

                    _unlockedNodeParent = GetNode(_unlockedNodeParentPath);
                }

                return _unlockedNodeParent;
            }
        }

        public override void _Ready()
        {
            base._Ready();
            CallDeferred("Initialize");
        }

        private void Initialize()
        {
            GameObstacleLock obstacleLock = SearchNodeType.FindChildOfType<GameObstacleLock>(this);
            obstacleLock.BindLockedSignal(this, "HideItem");
        }

        private void HideItem(bool locked)
        {
            if (locked)
            {
                if (UnlockedNodeParent == null)
                {
                    throw new Exception(
                        $"Unable to determine destination for {nameof(HideParentOnLock)} {Name}");
                }

                GD.Print($"{Name} is locking {GetParent().Name}");
                ReplaceParent.Replace(GetParent(), null);
            }
            else
            {
                GD.Print($"{Name} is unlocking {GetParent().Name} to attach to {UnlockedNodeParent}");
                ReplaceParent.Replace(GetParent(), UnlockedNodeParent);
            }
        }
    }
}