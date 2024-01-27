using System;
using System.Collections.Generic;
using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
    public class PauseUI : Node
    {
        [Export] private Resource _pauseMenuStateIdentifier = null;
        [Export] private Resource _closePauseMenuStateIdentifier = null;

        private ExclusiveStateNodeManager _menu;

        public override void _Ready()
        {
            base._Ready();
            if (PauseManager.Instance.Connect("GamePaused", this, "ToggleUI") != Error.Ok)
                throw new Exception("Failed to connect PauseUI to PauseManager signal!");

            if (_menu == null)
            {
                _menu = SearchNodeType.FindChildOfType<ExclusiveStateNodeManager>(this);
            }

            if (_menu.Connect("StateChanged", this, "OnMenuStateChanged") != Error.Ok)
            {
                throw new Exception("Failed to connect pause menu state transitions!");
            }
        }

        private void ToggleUI(bool paused)
        {
            if (paused)
            {
                _menu.ChangeState(_pauseMenuStateIdentifier);
            }
            else
            {
                _menu.ChangeState(_closePauseMenuStateIdentifier);
            }
        }

        private void OnMenuStateChanged(Resource previousState, Resource nextState)
        {
            if (previousState == null || nextState == null)
            {
                return;
            }

            string nextID = ExclusiveStateNode.GetStateIDForResource(nextState);
            string pauseState = ExclusiveStateNode.GetStateIDForResource(_pauseMenuStateIdentifier);
            string resumeState = ExclusiveStateNode.GetStateIDForResource(_closePauseMenuStateIdentifier);
            bool nextStatePauses = nextID == pauseState;
            bool nextStateResumes = nextID == resumeState;
            bool shouldPause = nextStatePauses && !nextStateResumes;
            if (shouldPause != PauseManager.Instance.Paused)
            {
                PauseManager.Instance.PauseGame(shouldPause);
            }
        }
    }
}