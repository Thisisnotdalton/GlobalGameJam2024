using Godot;
using System;

public class Zombie : KinematicBody
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private const float MOVE_SPEED = 3f;

	private AnimationPlayer animPlayer;
	private RayCast raycast;

	Boolean dead = false;
	KinematicBody player = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		raycast = GetNode<RayCast>("RayCast");

		animPlayer.Play("walk");
		AddToGroup("zombies");
	}

	public override void _PhysicsProcess(float delta)
	{
		if (dead) return;
		if (player == null) return;

		var vecToPlayer = player.Translation - Translation;
		vecToPlayer = vecToPlayer.Normalized();
		raycast.CastTo = vecToPlayer * 1.5f;

		MoveAndCollide(vecToPlayer * MOVE_SPEED * delta);

		if (raycast.IsColliding())
		{
			var coll = raycast.GetCollider();
			if (coll != null && coll is Player colliderNode)
			{
				if (colliderNode.Name == "Player")
				{
					colliderNode.Kill();
				}
			}
		}
	}

	public void Kill()
	{
		dead = true;
		var collisionShape = this.GetNode<CollisionShape>("CollisionShape");
		collisionShape.Disabled = true;
		animPlayer.Play("die");
	}

	public void SetPlayer(KinematicBody p)
	{
		player = p;
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
