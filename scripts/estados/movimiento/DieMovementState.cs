using Godot;

    public partial class DieMovementState : State
    {
        private Personaje _player;
        private float die_direction;
        private Timer _death_timer;

        public override void Ready()
        {
            _player = (Personaje)GetTree().GetFirstNodeInGroup("Personajegroup");
            _death_timer = new Timer();
            _death_timer.WaitTime = 2f;
            _death_timer.OneShot = true;
            AddChild(_death_timer);
            _death_timer.Timeout += _on_death_timer;
        }
        public override void Enter()
        {
            _player.SetAnimation("morirse");
            _player.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Position += new Vector2(0, 10);
            _player.Velocity = -_player.Velocity;
            if (_player.Velocity.X < 0)
                die_direction = -1f;
            else
                die_direction = 1f;

             _death_timer.Start();
        }

        public override void Update(double delta)
        {
            AnimatedSprite2D sprite = _player.GetNode<AnimatedSprite2D>("AnimatedSprite2D");
            sprite.Rotation = Mathf.MoveToward(sprite.Rotation, Mathf.DegToRad(90 * die_direction), 5);
        }

        public override void UpdatePhysics(double delta)
        {
            Vector2 velocity = _player.Velocity;

            velocity += _player.GetGravity() * (float)delta;

            if (_player.IsOnFloor())
            {
                if (velocity.X != 0)
                    velocity = velocity.MoveToward(Vector2.Zero, 10);
            }

            _player.Velocity = velocity;
            _player.MoveAndSlide();
        }

        public void _on_death_timer()
        {
            GetTree().ReloadCurrentScene();
        }
}