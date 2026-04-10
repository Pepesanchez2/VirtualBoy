using Godot;
using System;

public partial class Parcur : Node
{
    private AudioStreamPlayer2D nivel1;

    public override void _Ready()
    {
        nivel1 = GetNode<AudioStreamPlayer2D>("AudioStreamPlayer2D");
        nivel1.Play();
    }
}