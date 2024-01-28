using System.Collections.Generic;
using Godot;

namespace GlobalGameJam2024.Scripts.Core
{
    public abstract class SearchNodeType
    {
        public static T FindChildOfType<T>(Node node) where T : Node
        {
            foreach (Node child in node.GetChildren())
                if (child is T correctType)
                    return correctType;
            return null;
        }

        public static List<T> FindChildrenOfType<T>(Node node) where T : Node
        {
            return FindChildrenOfType<T>(node, 0);
        }

        public static List<T> FindChildrenOfType<T>(Node node, int depth) where T : Node
        {
            List<T> results = new List<T>();
            foreach (Node child in node.GetChildren())
            {
                if (child is T correctType)
                    results.Add(correctType);
                if (depth > 0 || depth == -1)
                {
                    foreach (T grandChild in FindChildrenOfType<T>(child, depth == -1 ? -1 : depth - 1))
                    {
                        results.Add(grandChild);
                    }
                }
            }

            return results;
        }

        public static T FindParentOfType<T>(Node node) where T : Node
        {
            while (node != null)
            {
                if (node is T correctType)
                {
                    return correctType;
                }

                node = node.GetParent();
            }

            return null;
        }
    }
}