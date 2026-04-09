using Godot;
using System;

public partial class Murcielago : CharacterBody2D
{
    private Personaje _player;
    private Timer _death_timer;
    private bool isDying = false;

    private Personaje _playerInSight = null;
    private float playerLastY = 0;

	private float jumpStrength = -300;
    private float gravity = 900;

    public override void _Ready()
    {
        GetNode<Area2D>("AreaMatar").BodyEntered += AreaMatarEntered;
        GetNode<Area2D>("AreaMorirse").BodyEntered += AreaMorirseEntered;
        var areaVer = GetNode<Area2D>("AreaVer");
        areaVer.BodyEntered += AreaVerEntered;
        areaVer.BodyExited += AreaVerExited;

        _death_timer = new Timer();
        _death_timer.WaitTime = 0.5f;
        _death_timer.OneShot = true;
        AddChild(_death_timer);
        _death_timer.Timeout += _on_death_timer;
    }

    private void AreaMatarEntered(Node body)
    {
        if (body is Personaje player)
        {	
            player.Morirse();
        }
    }

    private void AreaVerEntered(Node body)
    {
        if (body is Personaje player)
        {
            GD.Print("Jugador detectado");
            _playerInSight = player;
            playerLastY = player.GlobalPosition.Y;
            
        if (IsOnFloor())
            {
                Velocity = new Vector2(Velocity.X, jumpStrength);
            }
        }
    }

    private void AreaVerExited(Node body)
    {
        if (body == _playerInSight)
        {
        GD.Print("Jugador salió del área de visión");
        _playerInSight = null;
        }
    }


    private void AreaMorirseEntered(Node body)
    {
		if (body is Personaje player)
		{
			if (isDying) return;
        	isDying = true;
            GetNode<Area2D>("AreaMatar").CallDeferred("set_monitoring", false);
            GetNode<Area2D>("AreaVer").CallDeferred("set_monitoring", false);
            GetNode<Area2D>("AreaMorirse").CallDeferred("set_monitoring", false);
			player.JumpVelocity = -300;
			player.Velocity = new Vector2(player.Velocity.X, player.JumpVelocity);
			GD.Print("Murcielago muere");

        	var sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        	sprite.Animation = "morirse";
        	sprite.Position += new Vector2(0, 10);

       		Velocity = Vector2.Zero;

        	_death_timer.Start();
		}        
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!isDying)
        {
            if (!IsOnFloor())
            {
                Velocity = new Vector2(Velocity.X, Velocity.Y + gravity * (float)delta);
            }

            if (_playerInSight != null && IsOnFloor())
        {
            float currentY = _playerInSight.GlobalPosition.Y;

            if (currentY < playerLastY)
            {
                GD.Print("Jugador saltó, murciélago salta!");
                Velocity = new Vector2(Velocity.X, jumpStrength);
            }

            playerLastY = currentY;
        }

            MoveAndSlide();
        }
    }

    private void _on_death_timer()
    {
        QueueFree();
    }
}