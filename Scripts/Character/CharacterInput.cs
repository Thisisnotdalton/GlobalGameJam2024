using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.Character
{
    public class CharacterInput : Node, IResettable
    {
        private Vector2 _direction = Vector2.Zero;
        public Vector2 Direction => _direction;

        public bool Jumping => Input.IsActionPressed("jump");

        public void Reset()
        {
            _direction = Vector2.Zero;
        }

        public override void _Ready()
        {
            base._Ready();
            PauseMode = PauseModeEnum.Process;
        }

        public override void _Process(float delta)
        {
            _direction = Vector2.Zero;
            if (Input.IsActionPressed("move_right")) _direction.x += 1;
            if (Input.IsActionPressed("move_left")) _direction.x -= 1;
            if (Input.IsActionPressed("move_back")) _direction.y += 1;
            if (Input.IsActionPressed("move_forward")) _direction.y -= 1;
            if (Input.IsActionJustPressed("ui_cancel")) PauseManager.Instance.TogglePaused();
            if (_direction != Vector2.Zero) _direction = _direction.Normalized();
        }
    }
}