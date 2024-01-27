using System;
using GlobalGameJam2024.Scripts.Core;
using GlobalGameJam2024.Scripts.Level;
using Godot;

namespace GlobalGameJam2024.Scripts.Character
{
    public class PlatformerCharacter : KinematicBody, IResettable, IRepulsable
    {
        [Export] private readonly float _fallAcceleration = 75;
        [Export] private readonly float _jumpImpulse = 20;
        [Export] private readonly float _repulsionDecay = 0.95f;
        [Export] private readonly float _repulsionMaxSpeed = 25;
        [Export] private readonly float _speed = 14;
        private CharacterAnimationPlayer _animationPlayer;
        private CharacterInput _input;

        private Vector3 _repulsionVelocity = Vector3.Zero;
        private Respawnable _respawnable;
        private Vector3 _velocity = Vector3.Zero;

        public Vector3 GetPosition()
        {
            return GlobalTranslation;
        }

        public void Repulse(Vector3 impulse)
        {
            _repulsionVelocity += impulse;
            if (_repulsionVelocity.Length() > _repulsionMaxSpeed)
                _repulsionVelocity = _repulsionVelocity / _repulsionVelocity.Length() * _repulsionMaxSpeed;
            _velocity += _repulsionVelocity;
        }

        public void Reset()
        {
            _repulsionVelocity = Vector3.Zero;
            _velocity = Vector3.Zero;
        }

        public override void _Ready()
        {
            base._Ready();
            if (_input == null) _input = SearchNodeType.FindChildOfType<CharacterInput>(this);
            if (_animationPlayer == null)
                _animationPlayer = SearchNodeType.FindChildOfType<CharacterAnimationPlayer>(this);

            if (_respawnable == null) _respawnable = SearchNodeType.FindChildOfType<Respawnable>(this);
            if (_respawnable.Connect("Respawned", this, "Respawn") != Error.Ok)
                throw new Exception("Failed to connect PlatformerCharacter to Respawnable Respawned signal!");
        }

        public override void _PhysicsProcess(float delta)
        {
            base._PhysicsProcess(delta);
            if (_respawnable != null && _respawnable.IsDead) return;
            var animation = CharacterAnimationPlayer.AnimationCategory.Idle;
            if (IsOnFloor())
            {
                if (_input.Jumping)
                {
                    _velocity.y = _jumpImpulse;
                    animation = CharacterAnimationPlayer.AnimationCategory.Jump;
                }
            }
            else
            {
                animation = CharacterAnimationPlayer.AnimationCategory.Jump;
            }

            var direction = new Vector3(_input.Direction.x, 0, _input.Direction.y);
            if (_input.Direction != Vector2.Zero)
            {
                GetNode<Spatial>("Pivot").LookAt(Translation + direction, Vector3.Up);
                if (animation == CharacterAnimationPlayer.AnimationCategory.Idle)
                    animation = CharacterAnimationPlayer.AnimationCategory.Run;
            }

            if (_repulsionVelocity.Length() > 0) _repulsionVelocity *= Mathf.Pow(_repulsionDecay, 1 - delta);

            _velocity.x = direction.x * _speed + _repulsionVelocity.x;
            _velocity.z = direction.z * _speed + _repulsionVelocity.z;
            _velocity.y -= _fallAcceleration * delta;
            _velocity = MoveAndSlide(_velocity, Vector3.Up);
            _animationPlayer.PlayCharacterAnimation(animation);
        }


        private void MoveToSpawnPoint(ISpawnPoint spawnPoint)
        {
            GlobalTranslation = spawnPoint.GetPosition();
        }

        private void Respawn(Respawnable respawnable)
        {
            if (respawnable != null) MoveToSpawnPoint(respawnable.SpawnPoint);
        }
        
        public void ResetToSpawnPoint(ISpawnPoint spawnPoint)
        {
            if (_respawnable != null)
            {
                _respawnable.SpawnPoint = spawnPoint;
                Respawn(_respawnable);
            }
            else
            {
                MoveToSpawnPoint(spawnPoint);
            }
        }
    }
}