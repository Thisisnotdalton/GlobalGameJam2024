using System.Numerics;
using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public class Window : Control
    {
        private static readonly Vector4 MinimizedPosition = new Vector4(0,0,0,0);
        private static readonly Vector4 MaximizedPosition = new Vector4(0,0,1,1);
        private Vector4 _lastOpenPosition = new Vector4(0,0,1,1);
        private Vector4 _lastPosition = new Vector4(0,0,1,1);
        private Vector4 _targetPosition = Vector4.Zero;


        [Export(PropertyHint.Range, "0,5")]
        private float _transitionTime = 1;

        private float _transitionTimeRemaining = 0;
        
        private WindowState _state = WindowState.Opened;

        public WindowState State
        {
            get => _state;
            set => ChangeWindowState(value);
        }

        public string Title { get; set; }

        [Signal]
        public delegate void Opened(string windowTitle);

        [Signal]
        public delegate void Closed(string windowTitle);

        [Signal]
        public delegate void Minimized(string windowTitle);

        [Signal]
        public delegate void Maximized(string windowTitle);

        private void ChangeWindowState(WindowState newState)
        {
            ChangeWindowState(newState, _state != newState);
        }

        private void ChangeWindowState(WindowState newState, bool signalStateChange)
        {
            _transitionTimeRemaining = 0;
            if (signalStateChange)
            {
                switch (newState)
                {
                    case WindowState.Opened:
                        EmitSignal("Opened", Title);
                        _targetPosition = _lastOpenPosition;
                        break;
                    case WindowState.Closed:
                        EmitSignal("Closed", Title);
                        _targetPosition = MinimizedPosition;
                        break;
                    case WindowState.Minimized:
                        EmitSignal("Minimized", Title);
                        _targetPosition = MinimizedPosition;
                        break;
                    case WindowState.Maximized:
                        EmitSignal("Maximized", Title);
                        _targetPosition = MaximizedPosition;
                        break;
                }

                _transitionTimeRemaining = _transitionTime;
            }
            GD.Print($"Changing state of {nameof(Window)} {Name} to {newState}");
            _state = newState;
        }

        public override void _Process(float delta)
        {
            base._Process(delta);
            if (_transitionTimeRemaining > 0)
            {
                _transitionTimeRemaining = Mathf.Max(0, _transitionTimeRemaining - delta);
                LerpAnchor(_lastPosition, _targetPosition, 1 - (_transitionTimeRemaining/_transitionTime));
            }
        }

        private void LerpAnchor(Vector4 startPosition, Vector4 endPosition, float t)
        {
            Vector4 newPosition = Vector4.Lerp(startPosition, endPosition, t);
            AnchorLeft = newPosition.X;
            AnchorTop = newPosition.Y;
            AnchorRight = newPosition.Z;
            AnchorBottom = newPosition.W;
        }
        
    }
}