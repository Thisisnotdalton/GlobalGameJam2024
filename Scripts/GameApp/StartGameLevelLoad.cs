using GlobalGameJam2024.Scripts.Core;
using GlobalGameJam2024.Scripts.Level;
using Godot;

namespace GlobalGameJam2024.Scripts.GameApp
{
    public class StartGameLevelLoad : Node
    {
        [Export] private Resource _gameStateId = null;
        private LevelLoader _levelLoader;

        public override void _Ready()
        {
            base._Ready();
            ExclusiveStateNodeManager manager = SearchNodeType.FindParentOfType<ExclusiveStateNodeManager>(this);
            ConnectSignal.Check(manager, "StateChanged", this, "StateChangedResponse");
            _levelLoader = SearchNodeType.FindChildOfType<LevelLoader>(this);
        }

        private void StateChangedResponse(Resource previousState, Resource nextState)
        {
            if (ExclusiveStateNode.GetStateIDForResource(nextState) ==
                ExclusiveStateNode.GetStateIDForResource(_gameStateId) &&
                ExclusiveStateNode.GetStateIDForResource(previousState) !=
                ExclusiveStateNode.GetStateIDForResource(_gameStateId))
            {
                _levelLoader.LoadDefaultLevel();
            }
        }
    }
}