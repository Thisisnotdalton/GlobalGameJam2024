using System;
using GlobalGameJam2024.Scripts.Character;
using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.Level
{
    public class DeathZone : Area
    {
        public override void _Ready()
        {
            if (Connect("body_entered", this, "OnBodyEntered") != Error.Ok)
                throw new Exception("Failed to connect body_entered to OnBodyEntered!");
        }

        private void OnBodyEntered(Node body)
        {
            if (SearchNodeType.FindChildOfType<Respawnable>(body) is Respawnable respawnable)
                respawnable.Kill();
        }
    }
}