using System.Collections.Generic;
using Godot;

namespace GlobalGameJam2024.Scripts.Character
{
    public class Invincible : Node, IResettable
    {
        private readonly Dictionary<Node, Node> _grantsInvincibility = new Dictionary<Node, Node>();

        public bool IsInvincible => _grantsInvincibility.Count > 0;

        public void Reset()
        {
            _grantsInvincibility.Clear();
        }

        public void MakeInvincible(Node grantor)
        {
            _grantsInvincibility[grantor] = grantor;
        }

        public void StopMakingInvincible(Node grantor)
        {
            _grantsInvincibility.Remove(grantor);
        }
    }
}