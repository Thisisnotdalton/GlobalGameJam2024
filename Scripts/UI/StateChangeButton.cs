using System;
using GlobalGameJam2024.Scripts.Core;
using Godot;

namespace GlobalGameJam2024.Scripts.UI
{
	public class StateChangeButton : Button
	{
		[Export] private readonly Resource _targetStateIdentifier = null;
		
		public override void _Pressed()
		{
			base._Pressed();
			ExclusiveStateNodeManager stateManager = SearchNodeType.FindParentOfType<ExclusiveStateNodeManager>(this);
			if (stateManager == null)
			{
				throw new Exception($"No {nameof(ExclusiveStateNodeManager)} could be found as parent of {Name}!");
			}
			stateManager.ChangeState(_targetStateIdentifier);
		}
	}
}
