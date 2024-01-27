using System;
using Godot;

namespace GlobalGameJam2024.Scripts.Level
{
    public class SpawnPoint : Spatial, ISpawnPoint
    {
        public Vector3 GetPosition()
        {
            return GlobalTranslation;
        }
    }
}