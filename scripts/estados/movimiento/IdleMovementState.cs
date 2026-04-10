using Godot;

public partial class IdleMovementState : State
{
    private Personaje _player;

    private AudioStreamPlayer2D walkSound;
    private CharacterBody2D player;

    public override async void Ready()
    {
        _player = (Personaje)GetTree().GetFirstNodeInGroup("Personajegroup");
        player = GetOwner<CharacterBody2D>();
        walkSound = player.GetNode<AudioStreamPlayer2D>("Sounds/WalkSound");
        if (!_player.IsNodeReady())
            await ToSignal(_player, "ready");
    }
    public override void Enter()
    {
        walkSound.Stop();
        _player.SetAnimation("default");
    }

    public override void UpdatePhysics(double delta)
    {
        Vector2 velocity = _player.Velocity;

		if (Input.IsActionJustPressed("jump") && _player.IsOnFloor())
		{
			velocity.Y = _player.JumpVelocity;
		}
		else
		{
			velocity.X = Mathf.MoveToward(_player.Velocity.X, 0, _player.Speed);
		}
        if (_player.IsOnFloor() )
		{
			_player.Contador = 1;
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
            stateMachine.TransitionTo("JumpingMovementState");
    }
}