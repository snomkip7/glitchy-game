using Godot;
using System;
using System.Collections.Generic;

public partial class GlobalVariables : Node
{
	public List<PackedScene> inventory; 
	
	public override void _Ready()
	{
		// get save file data
		inventory = new List<PackedScene>();
		PackedScene scene = (PackedScene) ResourceLoader.Load("res://Player/Spear.tscn");
		inventory.Add(scene);
		inventory.Add(scene);
	}

	
	// public override void _Process(double delta)
	// {
	// }
}
