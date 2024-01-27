using System;
using System.Collections.Generic;
using GlobalGameJam2024.Scripts.Character;
using Godot;

namespace GlobalGameJam2024.Scripts.Level
{
    public class Repulsion : Area
    {
        [Export] private readonly bool _constantRepulsionDirection = true;
        [Export] private readonly float _cooldownSeconds = 1;

        [Export] private readonly float _force = 40;
        private readonly Dictionary<IRepulsable, bool> _repelBodies = new Dictionary<IRepulsable, bool>();

        private readonly Dictionary<IRepulsable, float> _repelCooldown = new Dictionary<IRepulsable, float>();
        [Export] private readonly float _verticalForce = 30;

        private Vector3 _forward = Vector3.Zero;

        private Vector3 Forward
        {
            get
            {
                if (_forward == Vector3.Zero) _forward = GlobalTransform.basis.z;
                return _forward;
            }
        }

        public override void _Ready()
        {
            if (Connect("body_entered", this, "OnBodyEntered") != Error.Ok ||
                Connect("body_exited", this, "OnBodyExited") != Error.Ok)
                throw new Exception("Failed to bind body entered and exited signals!");
        }

        private void OnBodyEntered(Node body)
        {
            if (body is IRepulsable repulsable) _repelBodies[repulsable] = true;
        }

        private void OnBodyExited(Node body)
        {
            if (body is IRepulsable repulsable) _repelBodies.Remove(repulsable);
        }

        private void UpdateRepelCooldown(float delta)
        {
            foreach (IRepulsable repulsable in new List<IRepulsable>(_repelCooldown.Keys))
            {
                _repelCooldown[repulsable] -= delta;
            }
        }

        private void RepelBody(IRepulsable repulsable)
        {
            Vector3 displacement;
            if (_constantRepulsionDirection)
            {
                displacement = -Forward;
            }
            else
            {
                displacement = repulsable.GetPosition() - GlobalTranslation;
                if (displacement.Dot(Forward) < 0) displacement *= -1;
                displacement.y = 0;
            }

            var repulsion = displacement.Normalized() * _force + Vector3.Up * _verticalForce;
            repulsable.Repulse(repulsion);
        }

        private void TryRepelBody(IRepulsable repulsable)
        {
            float cooldown;
            if (!_repelCooldown.TryGetValue(repulsable, out cooldown)) cooldown = 0;
            if (cooldown <= 0)
            {
                RepelBody(repulsable);
                _repelCooldown[repulsable] = _cooldownSeconds;
            }
        }

        private void RepelBodies()
        {
            foreach (var repulsable in _repelBodies.Keys) TryRepelBody(repulsable);
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
            UpdateRepelCooldown(delta);
            RepelBodies();
        }
    }
}