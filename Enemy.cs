using Godot;
using System;

public partial class Enemy : Node2D
{
	[Export]
	private int StartingHealth;
	[Export]
	private int MaxHealth;

	private int currentHealth;
	private AnimationPlayer animationPlayer;
	private Sprite2D idle;
	private Sprite2D takeHit;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("enemy_IDLE");
		idle = GetNode<Sprite2D>("Visuals/Idle");
		takeHit = GetNode<Sprite2D>("Visuals/TakeHit");
		GameManager.OnDealDamage += TakeDamage;
		animationPlayer.AnimationFinished += AnimationFinished;

		GetNode<Label>("Visuals/Health").Text = StartingHealth.ToString();
		currentHealth = StartingHealth;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void TakeDamage(int damageAmount)
	{
		currentHealth-= damageAmount;
		if(currentHealth<=0)
		{
			GD.Print("Enemy is dead");
			GetNode<Label>("Visuals/Health").Text = "0";
			return;
		}

		GetNode<Label>("Visuals/Health").Text = currentHealth.ToString();
		GD.Print($"I have taken {damageAmount} amount of damage.");
		//Play take hit animation
		idle.Visible = false;
		takeHit.Visible = true;
		animationPlayer.Play("enemy_TAKEHIT");
		

	}

	private void AnimationFinished(StringName animation)
	{
		if(animation == "enemy_TAKEHIT")
		{
			takeHit.Visible = false;
			idle.Visible = true;
			animationPlayer.Play("enemy_IDLE");
		}
	}
}
