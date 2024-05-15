using Godot;
using System;

public partial class Sword : Weapon
{
	
	public const int speed = 350;
	public Vector2 target;
	public bool right = false;
	public bool inUse;
	public Player player;
	public float offset = 0;

    public override void _Ready()
    {
        player = GetParent<Player>();
    }

    public override void _PhysicsProcess(double delta)
	{
		if(!inUse){
			return;
		}

		if(!right){
			Rotation -= (float) (Math.PI*2/3 * delta*4);
		} else{
			Rotation += (float) (Math.PI*2/3 * delta*4);
		}
		Vector2 pos = new Vector2(0,-30+offset).Rotated(Rotation);
		if(right){
			pos.X -= 12;
		} else{
			pos.X += 12;
		}
		Position = pos;
		MoveAndSlide();
		if(player.weaponTimer.TimeLeft==0){
			QueueFree();
		}
		offset += .2f;
	}

	public override void start(Vector2 target){
		inUse = true;
		this.target = target;
		Rotation = 0;
		float angle = GetAngleTo(target);
		if(angle<0){
			angle = (float)Math.PI*2+angle;
		}
		if(angle<=Math.PI/2 || angle>Math.PI*1.5){
			right = true;
		}
		//SetCollisionMaskValue(1, true);
		SetCollisionLayerValue(4, true);
		GD.Print("Starting attack with sword");
		player.weaponTimer.Start(.25);
	}
}
