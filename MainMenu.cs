using Godot;
using System;

public partial class MainMenu : Control
{
	public override void _Ready()
	{
		// load info from txt
	}

	// public override void _Process(double delta)
	// {
	// }

	public void playLevel(){
		GetTree().ChangeSceneToFile("res://Levels/Level1.tscn");
	}
}
