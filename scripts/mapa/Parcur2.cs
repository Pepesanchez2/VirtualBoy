using Godot;
using System;

public partial class Parcur2 : Node
{
    private AudioStreamPlayer2D nivel2;

    public override void _Ready()
    {
        nivel2 = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        nivel2.Play();
    }
}