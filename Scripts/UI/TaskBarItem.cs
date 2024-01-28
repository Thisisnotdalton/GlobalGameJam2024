using System;
using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
	public class TaskBarItem : Button, ITaskBarItem
	{
		private Window _window = null;

		public Window Window => _window;
		public bool IsSelected { get; private set; }

		[Signal]
		public delegate void Closed(Node taskBarItem);

		private const string ClosedSignalName = "Closed";

		public Control GetTaskIcon()
		{
			return this;
		}

		public void BindToWindow(Window window)
		{
			if (_window == window)
			{
				return;
			}

			if (_window != null)
			{
				throw new Exception(
					$"{nameof(TaskBarItem)} {Name} already has already been bound to {nameof(Window)} {_window.Name}!");
			}

			_window = window;
			_window.SetMinimizedControlNode(this);
			_window.BindStateChanged(this, "OnWindowStateChanged");
			_window.BindClosed(this, "OnWindowStateChanged");
		}

		public void Select()
		{
			GD.Print($"Selecting window for task bar item {Name} with window {_window.Title}!");
			IsSelected = true;
			_window.State = WindowState.Maximized;
		}

		public void DeSelect()
		{
			IsSelected = false;
			_window.State = WindowState.Minimized;
		}

		public void BindClosedSignal(Node listener, string methodName)
		{
			if (!IsConnected(ClosedSignalName, listener, methodName) &&
				Connect(ClosedSignalName, listener, methodName) != Error.Ok)
			{
				throw new Exception(
					$"Failed to bind {ClosedSignalName} of {nameof(Window)} {Name} to {listener} {listener.Name}!");
			}
		}

		private void OnWindowStateChanged(Window window)
		{
			bool windowShouldBeSelected =
				(window.GetState() == WindowState.Opened || window.GetState() == WindowState.Maximized);
			if (window.GetState() == WindowState.Closed)
			{
				EmitSignal(ClosedSignalName, this);
			}
			else if (windowShouldBeSelected != IsSelected)
			{
				if (windowShouldBeSelected)
				{
					Select();
				}
				else
				{
					DeSelect();
				}
			}
		}

		public override void _Pressed()
		{
			base._Pressed();
			if (IsSelected)
			{
				DeSelect();
			}
			else
			{
				Select();
			}
		}
	}
}
