using System;
using System.Collections.Generic;
using Godot;

namespace GlobalGameJam2024.Scripts.Core
{
    public class SingletonManager : Node
    {
        private readonly Dictionary<Type, Node> singletons = new Dictionary<Type, Node>();

        public static SingletonManager Instance { get; private set; }

        public void Register(Node node)
        {
            var t = node.GetType();
            if (singletons.ContainsKey(t))
                throw new Exception($"Attempted to instantiate multiple singletons of type \"{t.Name}\"!");
            singletons[t] = node;
            AddChild(node);
        }

        public T GetSingleton<T>() where T : Node
        {
            if (singletons.TryGetValue(typeof(T), out var node) && node is T singleton)
                return singleton;

            throw new Exception($"Failed to find a singleton of type: \"{typeof(T).Name}\"!");
        }

        public override void _Ready()
        {
            base._Ready();
            if (Instance == null)
                Instance = this;
            else
                QueueFree();
        }
    }
}