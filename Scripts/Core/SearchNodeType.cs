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
            List<T> results = new List<T>();
            foreach (Node child in node.GetChildren())
                if (child is T correctType)
                    results.Add(correctType);
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