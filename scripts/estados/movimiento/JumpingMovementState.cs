using Godot;

public partial class JumpingMovementState : State
{
    private Personaje _player;

    private AnimatedSprite2D sprite;


    public override async void Ready()
    {
        _player = (Personaje)GetTree().GetFirstNodeInGroup("Personajegroup");
        sprite = _player.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        if (!_player.IsNodeReady())
            await ToSignal(_player, "ready");
    }
    public override void Enter()
    {
        _player.SetAnimation("jumping");
    }

   public override void UpdatePhysics(double delta)
    {
        Vector2 velocity = _player.Velocity;

		if (!_player.IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			velocity.Y = _player.JumpVelocity;
		}
		if (!_player.IsOnFloor())
		{
			velocity += _player.GetGravity() * (float)delta;
		}
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * _player.Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(_player.Velocity.X, 0, 15);
		}

        if (direction.X > 0)
        sprite.FlipH = false;
        else if (direction.X < 0)
        sprite.FlipH = true;

		if (Input.IsActionJustPressed("jump") && _player.IsOnFloor())
		{
			velocity.Y = _player.JumpVelocity;
		}

        _player.Velocity = velocity;
		_player.MoveAndSlide();
    }

    public override void Update(double delta)
    {
        if (!_player.IsOnFloor())
        {
            if (_player.Velocity.Y < 0)
                stateMachine.TransitionTo("JumpingMovementState");
            else
                stateMachine.TransitionTo("FallingMovementState");
        }
       if (_player.IsOnFloor())
        {
            if (_player.Velocity.X == 0) 
                stateMachine.TransitionTo("IdleMovementState");
            if (_player.Velocity.X != 0) 
                stateMachine.TransitionTo("RunningMovementState");
        }
    }

    public override void HandleInput(InputEvent @event)
    {
        if (@event.IsActionPressed("move_left") && _player.IsOnFloor() || @event.IsActionPressed("move_right") && _player.IsOnFloor())
            stateMachine.TransitionTo("RunningMovementState");
        if (@event.IsActionPressed("jump"))
            stateMachine.TransitionTo("DoubleJumpMovementState");
    }
}