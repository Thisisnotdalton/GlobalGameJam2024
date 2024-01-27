using System;
using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public class WindowButton : Button
    {
        [Export] private readonly WindowState _changeToState = WindowState.Closed;

        private Window _window;

        public override void _Ready()
        {
            base._Ready();
            _window = SearchNodeType.FindParentOfType<Window>(this);
            if (_window == null) throw new Exception($"No {nameof(Window)} could be found as parent of {Name}!");
        }

        public override void _Pressed()
        {
            base._Pressed();
            if (_window.State == WindowState.Opened || _window.State == WindowState.Maximized)
            {
                _window.State = _changeToState;
            }
        }
    }
}