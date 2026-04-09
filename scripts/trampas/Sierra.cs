using Godot;
using System;

public partial class Sierra : Area2D
{
	private Personaje _player;
	
    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
{
    if (body is Personaje player)
    {
        player.Morirse();
    }
}
}