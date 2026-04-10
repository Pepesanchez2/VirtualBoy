using Godot;
using System;

public partial class Personaje : CharacterBody2D
{

	private AudioStreamPlayer2D DieSound;
	public float Speed = 150.0f;
	public  float JumpVelocity = -300.0f;

	public int Contador = 0;

	public AnimatedSprite2D sprite;
	public StateMachine stateMachine;

	public float InicioX = 102;
    public float InicioY = 548;


    public override void _Ready()
    {
		sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		stateMachine = GetNode<StateMachine>("MovementStateMachine");
		DieSound = GetNode<AudioStreamPlayer2D>("Sounds/DieSound");
    }

	 public void Morirse()
		{
        CollisionMask = 1;
		DieSound.Play();
        stateMachine.TransitionTo("DieMovementState");
		}

	public void SetAnimation(string animationName)
    {
		GD.Print($"Playing: {animationName}");
		if (sprite != null)
		{
			sprite.Play(animationName);
		}
    }
}