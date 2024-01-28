using System;
using System.Numerics;
using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public class Window : Control, IWindow
    {
        private static readonly Vector4 MaximizedPosition = new Vector4(0, 0, 1, 1);
        private Control _minimizedPositionNode;
        private Vector4 _lastOpenPosition = new Vector4(0, 0, 1, 1);
        private Vector4 _lastPosition = new Vector4(0, 0, 1, 1);
        private Vector4 _targetPosition = Vector4.Zero;
        private const string TransitionSignalName = "FinishedTransition";

        [Export(PropertyHint.Range, "0,5")] private float _transitionTime = 1;

        private float _transitionTimeRemaining = 0;

        private WindowState _state = WindowState.Opened;
        [Export] private NodePath _titleLabelPath = new NodePath(".");
        private Label _titleLabel;

        private Vector4 GetMinimizedPosition()
        {
            return new Vector4(_minimizedPositionNode.AnchorLeft, _minimizedPositionNode.AnchorTop,
                _minimizedPositionNode.AnchorRight, _minimizedPositionNode.AnchorBottom);
        }

        public override void _Ready()
        {
            base._Ready();
            _titleLabel = GetNode<Label>(_titleLabelPath);
            if (_titleLabel == null)
            {
                throw new Exception($"Failed to fetch {nameof(Label)} for window title from node path {_titleLabel}!");
            }

            _lastOpenPosition = new Vector4(AnchorLeft, AnchorTop, AnchorRight, AnchorBottom);
        }


        public WindowState State
        {
            get => _state;
            set => ChangeWindowState(value);
        }

        public WindowState GetState()
        {
            return State;
        }

        public void SetTitle(string title)
        {
            _titleLabel.Text = title;
        }

        public void BindStateChanged(Node listener, string methodName)
        {
            if (Connect(TransitionSignalName, listener, methodName) != Error.Ok)
            {
                throw new Exception(
                    $"Failed to bind {TransitionSignalName} of {nameof(Window)} {Name} to {listener} {listener.Name}!");
            }
        }

        public void SetMinimizedControlNode(Control minimizedTarget)
        {
            _minimizedPositionNode = minimizedTarget;
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

        [Signal]
        public delegate void FinishedTransition(Window window);

        public void ChangeWindowState(WindowState newState)
        {
            ChangeWindowState(newState, _state != newState);
        }

        private void ChangeWindowState(WindowState newState, bool signalStateChange)
        {
            _transitionTimeRemaining = 0;
            if (signalStateChange)
            {
                switch (_state)
                {
                    case WindowState.Opened:
                        _lastPosition = _lastOpenPosition;
                        break;
                    case WindowState.Closed:
                    case WindowState.Minimized:
                        _lastPosition = GetMinimizedPosition();
                        break;
                    case WindowState.Maximized:
                        _lastPosition = MaximizedPosition;
                        break;
                }

                switch (newState)
                {
                    case WindowState.Opened:
                        EmitSignal("Opened", Title);
                        _targetPosition = _lastOpenPosition;
                        break;
                    case WindowState.Closed:
                        EmitSignal("Closed", Title);
                        _targetPosition = GetMinimizedPosition();
                        break;
                    case WindowState.Minimized:
                        EmitSignal("Minimized", Title);
                        _targetPosition = GetMinimizedPosition();
                        break;
                    case WindowState.Maximized:
                        EmitSignal("Maximized", Title);
                        _targetPosition = MaximizedPosition;
                        break;
                }

                _transitionTimeRemaining = _state == newState ? 0 : _transitionTime;
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
                LerpAnchor(_lastPosition, _targetPosition, 1 - (_transitionTimeRemaining / _transitionTime));
                if (_transitionTimeRemaining <= 0)
                {
                    EmitSignal(TransitionSignalName, this);
                }
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

        public Control GetWindow()
        {
            return this;
        }
    }
}