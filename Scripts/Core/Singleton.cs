using System;
using Godot;

namespace GlobalGameJam2024.Scripts.Core
{
    public class Singleton<T> : Node where T : Node, new()
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var instance = new T();
                    instance.Name = typeof(T).Name;
                    instance.Call("RegisterSingleton");
                    _instance = instance;
                }

                return _instance;
            }
        }

        private void RegisterSingleton()
        {
            if (SingletonManager.Instance is SingletonManager manager)
                manager.Register(this);
            else
                throw new Exception($"Failed to access SingletonManager when registering {Name}!");
        }
    }
}