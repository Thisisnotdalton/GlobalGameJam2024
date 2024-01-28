using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public interface IWindow
    {
        void ChangeWindowState(WindowState windowState);
        Control GetWindow();
        WindowState GetState();
    }
}
