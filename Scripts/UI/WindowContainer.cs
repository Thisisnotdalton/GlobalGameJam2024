using System;
using System.Collections.Generic;
using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public class WindowContainer : Control
    {
        [Export] private NodePath _applicationWindowContainerPath = new NodePath(".");
        private Control _applicationWindowContainer;
        private readonly Dictionary<IWindow, IWindow> _windows = new Dictionary<IWindow, IWindow>();

        public override void _Ready()
        {
            base._Ready();
            _applicationWindowContainer = GetNode<Control>(_applicationWindowContainerPath);
            if (_applicationWindowContainer == null)
            {
                throw new Exception(
                    $"Failed to find container for {nameof(WindowContainer)} {Name} at path {_applicationWindowContainerPath}!");
            }
        }

        public void AddWindow(IWindow window)
        {
            if (_windows.ContainsKey(window))
            {
                GD.Print($"{nameof(WindowContainer)} {Name} already contains {window}!");
                return;
            }

            _windows[window] = window;
            window.BindStateChanged(this, "OnWindowStateChanged");
            window.BindOpened(this, "OnWindowStateChanged");
            window.BindMaximized(this, "OnWindowStateChanged");
            DisplayWindow(window);
        }

        private void DisplayWindow(IWindow window)
        {
            ReplaceParent.Replace(window.GetWindow(), _applicationWindowContainer);
        }

        private void HideWindow(IWindow window)
        {
            ReplaceParent.Replace(window.GetWindow(), null);
        }

        private void RemoveWindow(IWindow window)
        {
            if (_windows.Remove(window))
            {
                HideWindow(window);
                window.ChangeWindowState(WindowState.Closed);
            }
        }

        private void MinimizeWindow(IWindow window)
        {
            if (!_windows.ContainsKey(window))
            {
                return;
            }

            HideWindow(window);
        }

        private void MaximizeWindow(IWindow window)
        {
            if (!_windows.ContainsKey(window))
            {
                return;
            }
            DisplayWindow(window);
        }

        private void OpenWindow(IWindow window)
        {
            if (!_windows.ContainsKey(window))
            {
                AddWindow(window);
            }
            DisplayWindow(window);
        }

        public void OnWindowStateChanged(Window window)
        {
            string windowName = window != null && window.GetWindow() != null ? window.GetWindow().Name : "";
            GD.Print(
                $"{nameof(WindowContainer)} {Name} received window state changed event for {nameof(IWindow)} {windowName} with state {window?.GetState()}");
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