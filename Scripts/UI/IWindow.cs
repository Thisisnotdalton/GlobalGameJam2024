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
        void BindOpened(Node listener, string methodName);
        void BindClosed(Node listener, string methodName);
        void BindMinimized(Node listener, string methodName);
        void BindMaximized(Node listener, string methodName);
        void SetMinimizedControlNode(Control minimizedTarget);
    }
}
