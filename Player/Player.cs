using Godot;
using System;
public partial class Player : CharacterBody2D
{
	public const int speed = 800;
	public const int jumpVelocity = -600;
	public const int gravity = 20;
	public GlobalVariables globalVariables;
	public int selectedSlot = 0;
	public Timer scrollTimer;
	public bool weaponAvailable = true;
	public Timer weaponTimer;
	public Timer dmgTimer;
	public Color color;
	public int health = 3;
	public int count = 0;

    public override void _Ready()
    {
		color = Modulate;
        globalVariables = GetNode<GlobalVariables>("/root/GlobalVariables");
		scrollTimer = GetNode<Timer>("ScrollTimer");
		weaponTimer = GetNode<Timer>("WeaponTimer");
		dmgTimer = GetNode<Timer>("DmgTimer");
		
		for(int i=0;i<globalVariables.inventory.Count;i++){
			Sprite2D sprite = new Sprite2D();
			sprite.Scale = new Vector2(.5f, .5f);
			sprite.Name = "Inventory"+(i+1);
			sprite.Visible = true;
			sprite.Texture = (Texture2D) ResourceLoader.Load("icon.svg");
			GetNode<CanvasLayer>("UILayer").AddChild(sprite, true);
		}
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
		setInventoryPositions();

		if(Input.IsActionJustPressed("scrollup") && scrollTimer.TimeLeft == 0){
			selectedSlot--;
			if(selectedSlot<0){
				selectedSlot = 0;
			}
			scrollTimer.Start();
		}
		if(Input.IsActionJustPressed("scrolldown") && scrollTimer.TimeLeft == 0){
			selectedSlot++;
			if(selectedSlot>globalVariables.inventory.Count-1){
				selectedSlot = globalVariables.inventory.Count-1;
			}
			scrollTimer.Start();
		}
		if(Input.IsActionJustPressed("attack") && weaponAvailable){
			Weapon scene = globalVariables.inventory[selectedSlot].Instantiate<Weapon>();
			AddChild(scene);
			scene.start(GetGlobalMousePosition());
			weaponAvailable = false;
			
		}
		MoveAndSlide();
	}

	public void fallOutOfMap(Node2D body){
		GD.Print("Skill issue");
		health = 1;
		takeDmg();
	}

	public void respawn(){
		GetTree().ReloadCurrentScene();
	}

	public void setInventoryPositions(){
		Camera2D cam = GetNode<Camera2D>("PlayerCamera");
		Vector2 camera = cam.GetTargetPosition();
		for(int i=0;i<globalVariables.inventory.Count;i++){
			GetNode<Sprite2D>("UILayer/Inventory"+(i+1)).Position = new Vector2(camera.X-1150+i*82, camera.Y-590);
		}
		GetNode<Sprite2D>("UILayer/Selected").Position = new Vector2(camera.X-1150+selectedSlot*82, camera.Y-590);
		for(int i=0;i<health;i++){
			GetNode<Sprite2D>("UILayer/Health"+i).Position = new Vector2(camera.X+1150-i*80, camera.Y-590);
		}
	}

	public void weaponOver(){
		weaponAvailable = true;
	}

	public void takeDmg(){
		health--;
		GetNode<Sprite2D>("UILayer/Health"+health).QueueFree();
		Modulate = new Color(255, color.G, color.B);
		dmgTimer.Start();
	}

	public void dmgDone(){
		Modulate = color;
		if(health<=0){
			CallDeferred("respawn");
		}
	}

	public void endEntered(Node2D body){
		if(count==globalVariables.enemyCount){
			GD.Print(count);
			globalVariables.nextLvl();
		}
	}
}
