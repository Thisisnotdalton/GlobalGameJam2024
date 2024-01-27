using System.Collections.Generic;
using Godot;

namespace GlobalGameJam2024.Scripts.Character
{
    public class ResettableBehaviours : Node, IResettable
    {
        private readonly Dictionary<IResettable, IResettable> _registeredBehaviours =
            new Dictionary<IResettable, IResettable>();

        public void Reset()
        {
            foreach (var behaviour in _registeredBehaviours.Values) behaviour.Reset();
        }

        private void RegisterBehaviour(IResettable behaviour)
        {
            _registeredBehaviours[behaviour] = behaviour;
        }

        private void DeregisterBehaviour(IResettable behaviour)
        {
            _registeredBehaviours.Remove(behaviour);
        }

        public override void _Ready()
        {
            base._Ready();
            foreach (Node child in GetParent().GetChildren())
            {
                if (child == this) continue;
                if (child is IResettable resettable) RegisterBehaviour(resettable);
            }
        }
    }
}