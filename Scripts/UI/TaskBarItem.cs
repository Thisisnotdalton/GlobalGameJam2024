using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public class TaskBarItem : Control, ITaskBarItem
    {
        private Window _window;
        
        
        
        public Control GetTaskIcon()
        {
            return this;
        }

        public void Select()
        {
            _window.State = WindowState.Opened;
        }

        public void DeSelect()
        {
            
        }
    }
}
