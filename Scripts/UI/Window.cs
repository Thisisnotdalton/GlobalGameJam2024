using System;
using System.Numerics;
using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
	public class Window : Control, IWindow
	{
		private static readonly Vector4 MaximizedPosition = new Vector4(0, 0, 1, 1);
		private Control _minimizedPositionNode;
		private Vector4 _lastOpenPosition = new Vector4(0, 0, 0, 0);
		private Vector4 _lastPosition = new Vector4(0, 0, 1, 1);
		private Vector4 _targetPosition = Vector4.Zero;
		private const string TransitionSignalName = "FinishedTransition";

		[Signal]
		public delegate void Opened(Window window);

		private const string OpenedSignalName = "Opened";

		[Signal]
		public delegate void Closed(Window window);

		private const string ClosedSignalName = "Closed";

		[Signal]
		public delegate void Minimized(Window window);

		private const string MinimizedSignalName = "Minimized";

		[Signal]
		public delegate void Maximized(Window window);

		private const string MaximizedSignalName = "Maximized";

		[Signal]
		public delegate void FinishedTransition(Window window);

		[Export(PropertyHint.Range, "0,5")] private float _transitionTime = 1;

		private float _transitionTimeRemaining = 0;

		private WindowState _state = WindowState.Minimized;
		[Export] private NodePath _windowLabelPath = new NodePath(".");
		private Label _windowLabel;

		private Label WindowLabel
		{
			get
			{
				if (_windowLabel == null)
				{
					_windowLabel = GetNode<Label>(_windowLabelPath);
					if (_windowLabel == null)
					{
						throw new Exception(
							$"Failed to fetch {nameof(Label)} for window title from node path {_windowLabelPath}!");
					}
				}

				return _windowLabel;
			}
		}

		private Vector4 GetMinimizedPosition()
		{
			return new Vector4(0, 1, 0, 1);
		}

		public override void _Ready()
		{
			base._Ready();
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
			WindowLabel.Text = title;
		}

		private void BindSignal(string signalName, Node listener, string methodName)
		{
			if (!IsConnected(signalName, listener, methodName) &&
				Connect(signalName, listener, methodName) != Error.Ok)
			{
				throw new Exception(
					$"Failed to bind {signalName} of {nameof(Window)} {Name} to {listener} {listener.Name}!");
			}
		}

		public void BindStateChanged(Node listener, string methodName)
		{
			BindSignal(TransitionSignalName, listener, methodName);
		}

		public void BindOpened(Node listener, string methodName)
		{
			BindSignal(OpenedSignalName, listener, methodName);
		}

		public void BindClosed(Node listener, string methodName)
		{
			BindSignal(ClosedSignalName, listener, methodName);
		}

		public void BindMinimized(Node listener, string methodName)
		{
			BindSignal(MinimizedSignalName, listener, methodName);
		}

		public void BindMaximized(Node listener, string methodName)
		{
			BindSignal(MaximizedSignalName, listener, methodName);
		}

		public void SetMinimizedControlNode(Control minimizedTarget)
		{
			_minimizedPositionNode = minimizedTarget;
		}

		public string Title => WindowLabel.Text;

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

				_transitionTimeRemaining = _state == newState ? 0 : _transitionTime;
			}

			GD.Print($"Changing state of {nameof(Window)} {Name} to {newState}");
			_state = newState;

			if (signalStateChange)
			{
				switch (_state)
				{
					case WindowState.Opened:
						EmitSignal(OpenedSignalName, this);
						_targetPosition = _lastOpenPosition;
						break;
					case WindowState.Closed:
						EmitSignal(ClosedSignalName, this);
						_targetPosition = GetMinimizedPosition();
						break;
					case WindowState.Minimized:
						EmitSignal(MinimizedSignalName, this);
						_targetPosition = GetMinimizedPosition();
						break;
					case WindowState.Maximized:
						EmitSignal(MaximizedSignalName, this);
						_targetPosition = MaximizedPosition;
						break;
				}
			}
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
