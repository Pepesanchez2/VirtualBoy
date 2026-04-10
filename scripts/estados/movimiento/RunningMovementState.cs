using Godot;

public partial class RunningMovementState : State
{
    private Personaje _player;

    private AudioStreamPlayer2D walkSound;
    private CharacterBody2D player;

    private AnimatedSprite2D sprite;


    public override async void Ready()
    {
        _player = (Personaje)GetTree().GetFirstNodeInGroup("Personajegroup");
        player = GetOwner<CharacterBody2D>();
        walkSound = player.GetNode<AudioStreamPlayer2D>("Sounds/WalkSound");
        sprite = _player.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        if (!_player.IsNodeReady())
            await ToSignal(_player, "ready");
    }
    public override void Enter()
    {
        walkSound.Play();
        _player.SetAnimation("running");
    }

    public override void UpdatePhysics(double delta)
    {
    Vector2 velocity = _player.Velocity;

    if (Input.IsActionJustPressed("jump") && _player.IsOnFloor())
    {
        velocity.Y = _player.JumpVelocity;
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

    if (_player.IsOnFloor())
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