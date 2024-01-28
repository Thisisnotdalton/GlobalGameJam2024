using System;
using System.Collections.Generic;
using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public class Desktop : Control
    {
        [Export] private NodePath _desktopWindowContainerPath = new NodePath(".");
        private Control _desktopWindowContainer;
        private readonly Dictionary<IWindow, IWindow> _windows = new Dictionary<IWindow, IWindow>();

        public override void _Ready()
        {
            base._Ready();
            _desktopWindowContainer = GetNode<Control>(_desktopWindowContainerPath);
            if (_desktopWindowContainer == null)
            {
                throw new Exception($"Failed to find TaskNodeContainer control for {nameof(Desktop)} {Name}!");
            }
        }

        public void AddWindow(IWindow window)
        {
            if (_windows.ContainsKey(window))
            {
                return;
            }

            _windows[window] = window;
            ReplaceParent.Replace(window.GetWindow(), _desktopWindowContainer);
        }

        public void RemoveWindow(IWindow window)
        {
            if (_windows.Remove(window))
            {
                ReplaceParent.Replace(window.GetWindow(), null);
                window.ChangeWindowState(WindowState.Closed);
            }
        }

        public void OnWindowStateChanged(IWindow window)
        {
        }
    }
}
