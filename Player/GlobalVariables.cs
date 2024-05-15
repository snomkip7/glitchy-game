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
		// PackedScene scene = (PackedScene) ResourceLoader.Load("res://Player/Spear.tscn");
		// inventory.Add(scene);
		PackedScene scene1 = (PackedScene) ResourceLoader.Load("res://Player/Sword.tscn");
		inventory.Add(scene1);
		
	}

	public void nextLvl(){
		level++;
		if(level>2){
			GD.Print("there is no more levels m8");
			CallDeferred("win");
		}
		else{
			if(level==2){
				PackedScene scene = (PackedScene) ResourceLoader.Load("res://Player/Spear.tscn");
				inventory.Add(scene);
			}
			CallDeferred("goToNext");
		}
	}

	public void win(){
		GetTree().ChangeSceneToFile("res://Win.tscn");
	}

	public void goToNext(){
		GetTree().ChangeSceneToFile("res://Levels/Level"+level+".tscn");
	}

	
	// public override void _Process(double delta)
	// {
	// }
}
