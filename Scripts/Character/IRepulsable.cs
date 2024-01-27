using Godot;

namespace GlobalGameJam2024.Scripts.Character
{
    public interface IRepulsable
    {
        Vector3 GetPosition();
        void Repulse(Vector3 impulse);
    }
}