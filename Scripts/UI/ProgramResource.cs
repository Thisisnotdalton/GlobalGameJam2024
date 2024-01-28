using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public class ProgramResource : Resource
    {
        [Export] public string ProgramName;
        [Export] public string WindowTitle;
        [Export] public PackedScene WindowScene;
        [Export] public PackedScene TaskBarItemScene;
        [Export] public Texture Icon;
    }
}