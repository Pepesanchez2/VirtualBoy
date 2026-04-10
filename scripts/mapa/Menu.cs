using Godot;
using System;

public partial class Menu : Node
{
    private AudioStreamPlayer2D menu;

    public override void _Ready()
    {
        menu = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        menu.Play();
    }
}