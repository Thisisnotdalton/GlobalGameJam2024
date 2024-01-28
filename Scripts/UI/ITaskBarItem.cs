using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public interface ITaskBarItem
    {
        Control GetTaskIcon();
        void BindToWindow(Window window);
        void Select();
        void DeSelect();

        void BindClosedSignal(Node listener, string methodName);
    }
}
