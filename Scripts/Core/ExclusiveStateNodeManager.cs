using System;
using System.Collections.Generic;
using Godot;

namespace GlobalGameJam2024.Scripts.Core
{
    public class ExclusiveStateNodeManager : Node
    {
        private ExclusiveStateNode _previousState = null;
        private ExclusiveStateNode _currentState = null;
        [Export] private Resource _defaultState = null;
        [Export] private Resource[] _externalStateIdentifiers = null;
        private readonly Dictionary<string, bool> _propagateStateChange = new Dictionary<string, bool>();

        private readonly Dictionary<string, ExclusiveStateNode> _states = new Dictionary<string, ExclusiveStateNode>();
        private bool _initialized = false;
        
        [Signal]
        public delegate void StateChanged(Resource previousState, Resource nextState);

        private void RegisterState(ExclusiveStateNode state)
        {
            RegisterState(state, state.GetStateID());
        }

        private void RegisterState(ExclusiveStateNode state, string stateID)
        {
            if (_initialized)
            {
                throw new Exception("Attempted to register new state after state manager was already initialized!");
            }
            string stateDisplayName = state == null ? stateID : state.GetStateDisplayName();
            GD.Print($"Attempting to register node {state?.Name} as state {stateDisplayName} in {Name}.");
            if (_states.TryGetValue(stateID, out ExclusiveStateNode existingState))
            {
                if (existingState != state)
                {
                    throw new Exception(
                        $"Multiple states attempting to register with unique identifier \"{stateID}\"!");
                }
            }
            else
            {
                _states[stateID] = state;
            }

            if (state != null)
            {
                EnableManagedNode(state, false);
            }
        }

        private void EnableManagedNode(ExclusiveStateNode node, bool enabled)
        {
            if (node == null)
            {
                throw new Exception("null state node provided!");
            }

            if (!_states.ContainsValue(node))
            {
                throw new Exception(
                    $"{GetType().Name} {Name} was not configured to manage {node.GetType().Name} {node.Name}!");
            }

            if (enabled)
            {
                if (node.GetParent() != this)
                {
                    AddChild(node);
                }
            }
            else if (node.GetParent() != null)
            {
                node.GetParent().RemoveChild(node);
            }
            node.TriggerStateChange(enabled);
        }

        private void ChangeStateNode(ExclusiveStateNode newState)
        {
            if (_currentState != null)
                EnableManagedNode(_currentState, false);
            _previousState = _currentState;
            if (newState != null)
                EnableManagedNode(newState, true);
            _currentState = newState;
        }

        public void ChangeState(Resource stateIdentifier)
        {
            string stateID = ExclusiveStateNode.GetStateIDForResource(stateIdentifier);
            GD.Print(
                $"Changing state of {Name} to {ExclusiveStateNode.GetStateDisplayNameForResource(stateIdentifier)}!");

            EmitSignal("StateChanged", _currentState?.StateIdentifier, stateIdentifier);
            if (_states.TryGetValue(stateID, out ExclusiveStateNode node))
            {
                if (node == null)
                {   
                    GD.Print($"{Name} substituting default {ExclusiveStateNode.GetStateDisplayNameForResource(_defaultState)} for {stateID}");
                    _states.TryGetValue(ExclusiveStateNode.GetStateIDForResource(_defaultState), out node);
                }

                if (node == null)
                {
                    throw new Exception(
                        $"No valid state could be found with identifier {ExclusiveStateNode.GetStateIDForResource(stateIdentifier)}");
                }
                GD.Print(
                    $"{Name} is changing to state {node.GetStateDisplayName()}");
                ChangeStateNode(node);
            }
            else
            {
                throw new Exception(
                    $"{GetType().Name} {Name} was not configured to manage nodes with state id: {stateID}!");
            }

            if (_propagateStateChange.TryGetValue(stateID, out bool propagate) && propagate)
            {
                ExclusiveStateNodeManager parentManager =
                    SearchNodeType.FindParentOfType<ExclusiveStateNodeManager>(GetParent());
                if (parentManager != null)
                {
                    GD.Print(
                        $"{Name} propagating state change to {ExclusiveStateNode.GetStateDisplayNameForResource(stateIdentifier)}");
                    parentManager.ChangeState(stateIdentifier);
                }
            }

        }

        public override void _Ready()
        {
            foreach (ExclusiveStateNode stateNode in SearchNodeType.FindChildrenOfType<ExclusiveStateNode>(this))
            {
                RegisterState(stateNode);
            }

            if (_externalStateIdentifiers != null)
            {
                foreach (Resource stateIdentifier in _externalStateIdentifiers)
                {
                    string stateID = ExclusiveStateNode.GetStateIDForResource(stateIdentifier);
                    if (!_states.ContainsKey(stateID))
                        RegisterState(null, stateID);
                    _propagateStateChange[stateID] = true;
                }
            }
            _initialized = true;
            ChangeState(_defaultState);
        }
    }
}