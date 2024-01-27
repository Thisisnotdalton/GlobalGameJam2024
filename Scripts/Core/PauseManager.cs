using Godot;

namespace GlobalGameJam2024.Scripts.Core
{
    public class PauseManager : Singleton<PauseManager>
    {
        private bool _paused;

        public bool Paused
        {
            private set
            {
                _paused = value;
                GetTree().Paused = _paused;
                EmitSignal("GamePaused", _paused);
            }
            get => _paused;
        }


        public void PauseGame(bool paused)
        {
            Paused = paused;
        }

        public void TogglePaused()
        {
            Paused = !Paused;
        }

        [Signal]
        private delegate void GamePaused(bool paused);
    }
}