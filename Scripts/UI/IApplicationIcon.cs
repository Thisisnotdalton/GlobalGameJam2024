using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public interface IApplicationIcon
    {
        void Select();
        void Deselect();
        void Launch();
        Control GetIcon();
    }
}
