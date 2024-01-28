using System;
using Godot;

namespace GlobalGameJam2024.Scripts.Core
{
    public abstract class ConnectSignal
    {
        
        public static void Check(Node emitter, string signalName, Node listener, string methodName)
        {
            if (!emitter.IsConnected(signalName, listener, methodName) &&
                emitter.Connect(signalName, listener, methodName) != Error.Ok)
            {
                throw new Exception(
                    $"Failed to bind {signalName} of {emitter.Name} to {listener} {listener.Name} method {methodName}!");
            }
        }
    }
}