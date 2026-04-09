using Godot;
using System;

public partial class Camera2d : Camera2D
{
    [Export] public NodePath playerPath;
    [Export] public Vector2 mapSize = new Vector2(1150, 655);

	[Export] public float lookAheadDistance = 150f;
    [Export] public float lookAheadSpeed = 1f;

    private Node2D player;

	private Vector2 targetOffset = Vector2.Zero;

    public override void _Ready()
    {
        MakeCurrent();

		player = GetNode<Node2D>(playerPath);

        LimitLeft = 0;
        LimitTop = 0;
        LimitRight = (int)mapSize.X;
        LimitBottom = (int)mapSize.Y;

        PositionSmoothingEnabled = true;
        PositionSmoothingSpeed = 2f;
    }

    public override void _Process(double delta)
    {
        if (player != null)
        {
            Vector2 playerVelocity = Vector2.Zero;
            if (player is Personaje p)
            {
                playerVelocity = p.Velocity;
                float targetX = Mathf.Lerp(targetOffset.X, playerVelocity.X * (lookAheadDistance / p.Speed), lookAheadSpeed * (float)delta);
                targetOffset = new Vector2(targetX, 0);
            }

			Offset = targetOffset;
        }
    }
}
