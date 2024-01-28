using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public interface IWindow
    {
        void ChangeWindowState(WindowState windowState);
        Control GetWindow();
        WindowState GetState();
        void SetTitle(string title);
        void BindStateChanged(Node listener, string methodName);
        void SetMinimizedControlNode(Control minimizedTarget);
    }
}
