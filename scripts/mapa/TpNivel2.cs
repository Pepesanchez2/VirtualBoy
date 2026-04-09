using Godot;
using System;

public partial class TpNivel2 : Area2D
{
    [Export] public string NewScenePath = "res://scenes/mapa/parcur2.tscn";

    private bool playerInside = false;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        if (body.IsInGroup("Personajegroup"))
        {
            playerInside = true;
            CallDeferred(nameof(ChangeScene));
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body.IsInGroup("Personajegroup"))
        {
            playerInside = false;
        }
    }

    private void ChangeScene()
    {
        GetTree().ChangeSceneToFile(NewScenePath);
    }
}
