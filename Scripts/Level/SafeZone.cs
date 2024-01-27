using System;
using GlobalGameJam2024.Scripts.Character;
using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.Level
{
    public class SafeZone : Area
    {
        public override void _Ready()
        {
            if (Connect("body_entered", this, "OnBodyEntered") != Error.Ok)
                throw new Exception("Failed to connect body_entered to OnBodyEntered!");
            if (Connect("body_exited", this, "OnBodyExited") != Error.Ok)
                throw new Exception("Failed to connect body_exited to OnBodyExited!");
        }

        private void OnBodyEntered(Node body)
        {
            if (SearchNodeType.FindChildOfType<Invincible>(body) is Invincible invincible)
                invincible.MakeInvincible(this);
        }

        private void OnBodyExited(Node body)
        {
            if (SearchNodeType.FindChildOfType<Invincible>(body) is Invincible invincible)
                invincible.StopMakingInvincible(this);
        }
    }
}