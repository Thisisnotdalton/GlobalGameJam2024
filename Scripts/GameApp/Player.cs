using Godot;
using System;
using System.Runtime.CompilerServices;

public class Player : KinematicBody
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	private const float MOVE_SPEED = 4f;
	private const float MOUSE_SENS = 0.5f;

	private AnimationPlayer animPlayer;
	private RayCast raycast;

	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		raycast = GetNode<RayCast>("RayCast");

		Input.MouseMode = Input.MouseModeEnum.Captured;

		// Equivalent to `yield(get_tree(), "idle_frame")`
		//GetTree().CreateTimer(0).Connect("timeout", this, nameof(OnIdleFrame));

		await ToSignal(GetTree(), "idle_frame");

		// Will be called after the idle frame
		GetTree().CallGroup("zombies", "SetPlayer", this);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseMotion eventMouseMotion)
		{
			RotationDegrees = new Vector3(
				RotationDegrees.x,
				RotationDegrees.y - MOUSE_SENS * eventMouseMotion.Relative.x,
				RotationDegrees.z
			);
		}
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (Input.IsActionPressed("exit"))
		{
			GetTree().Quit();
		}

		if (Input.IsActionPressed("restart"))
		{
			Kill(); // Assuming Kill is a method you've defined elsewhere
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		Vector3 moveVec = new Vector3();

		if (Input.IsActionPressed("move_forward"))
		{
			moveVec.z -= 1;
		}
		if (Input.IsActionPressed("move_back"))
		{
			moveVec.z += 1;
		}
		if (Input.IsActionPressed("move_left"))
		{
			moveVec.x -= 1;
		}
		if (Input.IsActionPressed("move_right"))
		{
			moveVec.x += 1;
		}

		moveVec = moveVec.Normalized();
		moveVec = moveVec.Rotated(new Vector3(0, 1, 0), Rotation.y);
		MoveAndCollide(moveVec * MOVE_SPEED * delta);

		if (Input.IsActionPressed("shoot") && !animPlayer.IsPlaying())
		{
			animPlayer.Play("shoot");
			if (raycast.IsColliding())
			{
				var coll = raycast.GetCollider();
				if (coll != null && coll is Zombie colliderNode)
				{
					colliderNode.Kill();
				}
			}
		}

		//base._PhysicsProcess(delta);
	}

	public void Kill()
	{
		GetTree().ReloadCurrentScene();
	}
}
