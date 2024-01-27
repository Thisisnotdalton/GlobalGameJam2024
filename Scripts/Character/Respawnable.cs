using System;
using GlobalGameJam2024.Scripts.Core;
using GlobalGameJam2024.Scripts.Level;
using Godot;

namespace GlobalGameJam2024.Scripts.Character
{
    public class Respawnable : Timer
    {
        private CharacterAnimationPlayer _animationPlayer;
        private Invincible _invincible;
        private ResettableBehaviours _resettableBehaviours;
        public ISpawnPoint SpawnPoint;
        public bool IsDead { get; private set; }

        public override void _Ready()
        {
            base._Ready();
            if (_animationPlayer == null)
                _animationPlayer = SearchNodeType.FindChildOfType<CharacterAnimationPlayer>(GetParent());

            if (_animationPlayer != null)
                if (_animationPlayer.Connect("FinishDeath", this, "InitiateRespawnCountdown") != Error.Ok)
                    throw new Exception("Unable to connect FinishDeath signal to InitiateRespawnCountdown!");

            if (_invincible == null) _invincible = SearchNodeType.FindChildOfType<Invincible>(GetParent());
            if (_resettableBehaviours == null)
                _resettableBehaviours = SearchNodeType.FindChildOfType<ResettableBehaviours>(GetParent());
            if (Connect("timeout", this, "InitiateRespawn") != Error.Ok)
                throw new Exception("Unable to connect timeout signal to InitiateRespawn!");
            OneShot = true;
        }

        public void Kill()
        {
            if (_invincible != null && _invincible.IsInvincible) return;
            IsDead = true;
            if (_animationPlayer != null)
                _animationPlayer.PlayCharacterAnimation(CharacterAnimationPlayer.AnimationCategory.Death);
            else
                InitiateRespawnCountdown();
        }

        private void InitiateRespawnCountdown()
        {
            Start();
        }

        private void InitiateRespawn()
        {
            _resettableBehaviours?.Reset();
            IsDead = false;
            EmitSignal("Respawned", this);
        }

        [Signal]
        private delegate void Respawned(Respawnable respawnable);
    }
}