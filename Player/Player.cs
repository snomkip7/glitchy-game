using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const int speed = 800;
	public const int jumpVelocity = -600;
	public const int gravity = 20;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		
		if (!IsOnFloor()){
			velocity.Y += gravity;
		}
		else{
			velocity.Y = 0;
		}
		
		if (Input.IsActionJustPressed("jump") && IsOnFloor()){
			velocity.Y = jumpVelocity;
		}

		if(Input.IsActionPressed("left") && Input.IsActionPressed("right")){
			velocity = velocity.Lerp(new Vector2(0, velocity.Y), 0.7f);
		}
		else if (Input.IsActionPressed("left"))
		{
			velocity = velocity.Lerp(new Vector2(-speed, velocity.Y), 0.7f);
		}
		else if (Input.IsActionPressed("right")){
			velocity = velocity.Lerp(new Vector2(speed, velocity.Y), 0.7f);
		}

		velocity = velocity.Lerp(new Vector2(0, velocity.Y), 0.1f);
		Velocity = velocity;
		MoveAndSlide();
	}

	public void fallOutOfMap(Node2D body){
		GD.Print("Skill issue");
		CallDeferred("respawn");
	}

	public void respawn(){
		GetTree().ReloadCurrentScene();
	}
}
