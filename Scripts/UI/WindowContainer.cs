using System;
using System.Collections.Generic;
using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public class WindowContainer : Control
    {
        [Export] private NodePath _applicationWindowContainerPath = new NodePath(".");
        private Container _applicationWindowContainer;
        private readonly Dictionary<IWindow, IWindow> _windows = new Dictionary<IWindow, IWindow>();

        public override void _Ready()
        {
            base._Ready();
            _applicationWindowContainer = GetNode<Container>(_applicationWindowContainerPath);
            if (_applicationWindowContainer == null)
            {
                throw new Exception($"Failed to find container for {nameof(WindowContainer)} {Name} at path {_applicationWindowContainerPath}!");
            }
        }

        public void AddWindow(IWindow window)
        {
            if (_windows.ContainsKey(window))
            {
                return;
            }

            _windows[window] = window;
            window.BindStateChanged(this, "OnWindowStateChanged");
        }

        private void RemoveWindow(IWindow window)
        {
            if (_windows.Remove(window))
            {
                ReplaceParent.Replace(window.GetWindow(), null);
                window.ChangeWindowState(WindowState.Closed);
            }
        }

        private void MinimizeWindow(IWindow window)
        {
            if (!_windows.ContainsKey(window))
            {
                return;
            }

            ReplaceParent.Replace(window.GetWindow(), null);
        }

        private void MaximizeWindow(IWindow window)
        {
            if (!_windows.ContainsKey(window))
            {
                return;
            }

            ReplaceParent.Replace(window.GetWindow(), null);
        }

        private void OpenWindow(IWindow window)
        {
            if (!_windows.ContainsKey(window))
            {
                return;
            }

            ReplaceParent.Replace(window.GetWindow(), _applicationWindowContainer);
        }

        public void OnWindowStateChanged(IWindow window)
        {
            switch (window.GetState())
            {
                case WindowState.Closed:
                    RemoveWindow(window);
                    break;
                case WindowState.Minimized:
                    MinimizeWindow(window);
                    break;
                case WindowState.Maximized:
                    MaximizeWindow(window);
                    break;
                case WindowState.Opened:
                    OpenWindow(window);
                    break;
            }
        }
    }
}