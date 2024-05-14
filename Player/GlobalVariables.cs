using Godot;
using System;
using System.Collections.Generic;

public partial class GlobalVariables : Node
{
	public List<PackedScene> inventory; 
	public int enemyCount;
	public int level = 1;
	
	public override void _Ready()
	{
		// get save file data
		inventory = new List<PackedScene>();
		PackedScene scene = (PackedScene) ResourceLoader.Load("res://Player/Spear.tscn");
		inventory.Add(scene);
		//inventory.Add(scene);
		PackedScene scene1 = (PackedScene) ResourceLoader.Load("res://Player/Sword.tscn");
		inventory.Add(scene1);
		
	}

	public void nextLvl(){
		level++;
		if(level>1){
			GD.Print("there is no more levels m8");
			CallDeferred("restart");
		}
	}

	public void restart(){
		GetTree().ChangeSceneToFile("res://MainMenu.tscn");
	}

	
	// public override void _Process(double delta)
	// {
	// }
}
