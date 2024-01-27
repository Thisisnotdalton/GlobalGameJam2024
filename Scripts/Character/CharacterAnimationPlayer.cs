using System;
using System.Collections.Generic;
using Godot;

namespace GlobalGameJam2024.Scripts.Character
{
    public class CharacterAnimationPlayer : AnimationPlayer, IResettable
    {
        public enum AnimationCategory
        {
            Idle,
            Run,
            Jump,
            Attack,
            Death
        }

        private readonly Dictionary<AnimationCategory, Animation> _animations =
            new Dictionary<AnimationCategory, Animation>();

        [Export] private readonly string _attackAnimation = "attack";

        [Export] private readonly string _deathAnimation = "death";

        [Export] private readonly string _idleAnimation = "idle";

        [Export] private readonly string _jumpAnimation = "jump";

        [Export] private readonly string _runAnimation = "run";

        private AnimationCategory _state;

        public AnimationCategory Idle => AnimationCategory.Idle;
        public AnimationCategory Run => AnimationCategory.Run;
        public AnimationCategory Jump => AnimationCategory.Jump;
        public AnimationCategory Attack => AnimationCategory.Attack;
        public AnimationCategory Death => AnimationCategory.Death;

        public void Reset()
        {
            PlayCharacterAnimation(AnimationCategory.Idle);
        }

        private static string _signal_name_suffix(AnimationCategory animationCategory)
        {
            switch (animationCategory)
            {
                case AnimationCategory.Attack:
                    return "Attack";
                case AnimationCategory.Death:
                    return "Death";
                default:
                    return "";
            }
        }

        private static bool _looping_animation(AnimationCategory animationCategory)
        {
            switch (animationCategory)
            {
                case AnimationCategory.Attack:
                    return false;
                case AnimationCategory.Death:
                    return false;
                default:
                    return true;
            }
        }

        private string _animation_names(AnimationCategory animationCategory)
        {
            switch (animationCategory)
            {
                case AnimationCategory.Attack:
                    return _attackAnimation;
                case AnimationCategory.Death:
                    return _deathAnimation;
                case AnimationCategory.Idle:
                    return _idleAnimation;
                case AnimationCategory.Run:
                    return _runAnimation;
                case AnimationCategory.Jump:
                    return _jumpAnimation;
                default:
                    return "";
            }
        }


        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            if (Connect("animation_started", this, "AnimationStartedResponse") != Error.Ok ||
                Connect("animation_finished", this, "AnimationFinishedResponse") != Error.Ok)
                throw new Exception("Failed to connect AnimationPlayer signals!");
            foreach (var animationCategoryAndName in new Dictionary<AnimationCategory, string>
                     {
                         { AnimationCategory.Idle, _idleAnimation },
                         { AnimationCategory.Run, _runAnimation },
                         { AnimationCategory.Jump, _jumpAnimation },
                         { AnimationCategory.Attack, _attackAnimation },
                         { AnimationCategory.Death, _deathAnimation }
                     })
                if (HasAnimation(animationCategoryAndName.Value))
                    _animations[animationCategoryAndName.Key] = GetAnimation(animationCategoryAndName.Value);
        }

        public void PlayCharacterAnimation(AnimationCategory animationCategory)
        {
            if (_state == animationCategory && IsPlaying()) return;

            _state = animationCategory;
            if (_animations.TryGetValue(_state, out var animation))
            {
                animation.Loop = _looping_animation(_state);
                Play(_animation_names(_state));
            }
        }

        private void AnimationStartedResponse(string animationName)
        {
            var signalSuffix = _signal_name_suffix(_state);
            if (signalSuffix.Length > 0) EmitSignal($"Start{signalSuffix}");
        }

        private void AnimationFinishedResponse(string animationName)
        {
            var signalSuffix = _signal_name_suffix(_state);
            if (signalSuffix.Length > 0) EmitSignal($"Finish{signalSuffix}");
        }

        [Signal]
        private delegate void StartAttack();

        [Signal]
        private delegate void FinishAttack();

        [Signal]
        private delegate void StartDeath();

        [Signal]
        private delegate void FinishDeath();
    }
}