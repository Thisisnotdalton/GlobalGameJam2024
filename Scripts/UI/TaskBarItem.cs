using System;
using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public class TaskBarItem : Control, ITaskBarItem
    {
        private Window _window = null;
        public bool IsSelected { get; private set; }

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
            _window.Connect("FinishedTransition", this, "");
        }

        public void Select()
        {
            _window.State = WindowState.Opened;
            IsSelected = true;
        }

        public void DeSelect()
        {
            IsSelected = false;
        }

        private void OnWindowStateChanged(Window window)
        {
            bool windowShouldBeSelected = (window.State == WindowState.Opened || window.State == WindowState.Maximized);
            if (windowShouldBeSelected != IsSelected)
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
    }
}