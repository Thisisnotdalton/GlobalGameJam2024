using System;
using GlobalGameJam2024.Scripts.Character;
using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.Level
{
    public class CheckPoint : Spatial, ISpawnPoint
    {
        public Vector3 GetPosition()
        {
            return GlobalTranslation;
        }

        public override void _Ready()
        {
            base._Ready();
            var area = SearchNodeType.FindChildOfType<Area>(this);
            if (area == null) throw new Exception($"Could not find an area node in CheckPoint children of {Name}!");
            if (area.Connect("body_entered", this, "UpdatePlayerCheckPoint") != Error.Ok)
                throw new Exception("Failed to connect body_entered signal to UpdatePlayerCheckPoint!");
        }

        private void UpdatePlayerCheckPoint(Node body)
        {
            if (SearchNodeType.FindChildOfType<Respawnable>(body) is Respawnable respawnable)
                respawnable.SpawnPoint = this;
        }
    }
}