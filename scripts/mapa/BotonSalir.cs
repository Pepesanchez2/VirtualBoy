using Godot;
using System;

namespace Mapa
{
    public partial class BotonSalir : Area2D
    {
    private bool playerInside = false;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    private void OnBodyEntered(Node body)
    {
        if (body.IsInGroup("Personajegroup"))
        {
            playerInside = true;
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body.IsInGroup("Personajegroup"))
        {
            playerInside = false;
        }
    }

    public override void _Process(double delta)
    {
        if (playerInside && Input.IsActionJustPressed("enter"))
        {
            DoButtonAction();
        }
    }

    private void DoButtonAction()
    {
        GD.Print("Botón presionado!");
		GetTree().Quit();
    }
    }
}
