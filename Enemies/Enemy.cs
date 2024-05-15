using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	public const int speed = 400;
	public const int gravity = 20;
	public const int jumpVelocity = -400;
	public Player player;
	public Timer selfDmgTimer;
	public int health = 3;
	public Color color;

    public override void _Ready()
    {
        player = GetParent().GetNode<Player>("Player");
		selfDmgTimer = GetNode<Timer>("SelfDmgTimer");
		color = Modulate;
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		if (!IsOnFloor()){
			velocity.Y += gravity;
		}
		else{
			velocity.Y = 0;
		}

		float distance = player.Position.X - Position.X;
		float above= Position.Y - player.Position.Y;

		if(Math.Abs(distance)<=1000 && above > 64 && above < 300){
			if(distance>100){
				velocity.X = speed;
			} else if(distance<-100){
				velocity.X = -speed;
			}
		}

		
		if(above > 64 && above < 300 && IsOnFloor()){
			velocity.Y = jumpVelocity;
		}

		for(int i=0;i<GetSlideCollisionCount();i++){
			var collision = GetSlideCollision(i);
			if(collision.GetCollider() == player && player.dmgTimer.TimeLeft == 0 && selfDmgTimer.TimeLeft==0 && player.weaponTimer.TimeLeft <.1){
				GD.Print("hit da player");
				player.takeDmg();
			}
		}

		Velocity = velocity;
		if(Position.Y>2500){
			health=0;
			fixColor();
		}
		MoveAndSlide();
	}

	public void weaponAttacked(Node2D body){
		if(selfDmgTimer.TimeLeft != 0){
			return;
		}
		Modulate = new Color(255, color.G, color.B);
		selfDmgTimer.Start();
		health--;
		if(player.Position.X-Position.X <0){
			Position = new Vector2(Position.X+80, Position.Y);
		} else{
			Position = new Vector2(Position.X-80, Position.Y);
		}
		if(Convert.ToString(body.Name) == "Sword"){
			health -=2;
			GD.Print("big outch");
			if(player.Position.X-Position.X <0){
				Position = new Vector2(Position.X+50, Position.Y);
			} else{
				Position = new Vector2(Position.X-50, Position.Y);
			}
		}
	}

	public void fixColor(){
		Modulate = color;
		if(health <= 0){
			player.count++;
			GetParent().GetNode<Label>("End/Remaining").Text = GetNode<GlobalVariables>("/root/GlobalVariables").enemyCount-player.count+" Enemies Remain";
			GD.Print("ded");
			QueueFree();
		}
	}
}
