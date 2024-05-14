using Godot;
using System;

public partial class Spear : Weapon
{
	
	public const int speed = 350;
	public bool forward = false;
	public bool inUse = false;
	public Vector2 target;

	public override void _PhysicsProcess(double delta)
	{
		if(!inUse){
			return;
		}

		Vector2 velocity = Velocity;
		if(forward){
			velocity = velocity.Lerp(new Vector2(0, -speed).Rotated(Rotation), 0.8f);
			//velocity = new Vector2(0,-speed).Rotated(Rotation);
		} else{
			velocity = velocity.Lerp(new Vector2(0, speed).Rotated(Rotation), 0.8f);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public override void start(Vector2 target){
		inUse = true;
		forward = true;
		this.target = target;
		Rotation = GetAngleTo(target) + (float)Math.PI/2;
		Position = new Vector2(0,-40).Rotated(Rotation);
		//SetCollisionMaskValue(1, true);
		SetCollisionLayerValue(4, true);
		GD.Print("Starting attack with spear");
		GetNode<Timer>("UseTimer").Start(.15f);
		GetParent<Player>().weaponTimer.Start(.85);
	}

	public void changeDirection(){
		if(forward==false){
			QueueFree();
		}
		forward = false;
		GetNode<Timer>("UseTimer").Start(.15f);
	}
}
