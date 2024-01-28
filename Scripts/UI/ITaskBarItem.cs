using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public interface ITaskBarItem
    {
        Control GetTaskIcon();
        void Select();
        void DeSelect();
    }
}
