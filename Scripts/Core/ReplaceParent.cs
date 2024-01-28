using Godot;

namespace GlobalGameJam2024.Scripts.Core
{
    public abstract class ReplaceParent
    {
        public static void Replace(Node node, Node newParent)
        {
            Node previousParent = node.GetParent();
            if (previousParent != newParent && previousParent != null)
            {
                previousParent.RemoveChild(node);        
            }
            if (newParent != null)
            {
                newParent.AddChild(node);
            }
        }
    }
}
