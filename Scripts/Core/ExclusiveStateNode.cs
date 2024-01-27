using Godot;

namespace GlobalGameJam2024.Scripts.Core
{
    public class ExclusiveStateNode : Node
    {
        [Export] private Resource _stateIdentifier = null;

        [Signal]
        private delegate void OnStateChanged(bool enabled);

        public void TriggerStateChange(bool enabled)
        {
            EmitSignal("OnStateChanged", enabled);
        }

        public Resource StateIdentifier
        {
            get { return _stateIdentifier; }
        }

        public static string GetStateIDForResource(Resource resource)
        {
            return resource.ResourcePath;
        }


        public static string GetStateDisplayNameForResource(Resource resource)
        {
            if (resource.Get("stateID") is string stateID && stateID.Length > 0)
            {
                return stateID;
            }

            return GetStateIDForResource(resource);
        }

        public string GetStateID()
        {
            return GetStateIDForResource(_stateIdentifier);
        }

        public string GetStateDisplayName()
        {
            return GetStateDisplayNameForResource(_stateIdentifier);
        }
    }
}