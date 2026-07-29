using Godot;
using System;

public partial class Enemy : Node2D
{

	private int currentHealth;
	private AnimationPlayer animationPlayer;
	private Sprite2D idle;
	private Sprite2D takeHit;
	private Sprite2D death;
	private HealthBar healthBar;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("enemy_IDLE");
		idle = GetNode<Sprite2D>("Visuals/Idle");
		takeHit = GetNode<Sprite2D>("Visuals/TakeHit");
		death = GetNode<Sprite2D>("Visuals/Death");
		animationPlayer.AnimationFinished += AnimationFinished;
		healthBar = GetNode<HealthBar>("Visuals/HealthBar");
		
		
		GameManager.Instance.GetBus().Subscribe<DealDamageEvent>(TakeDamage);
		currentHealth = healthBar.Health;
		
	}


	private void TakeDamage(DealDamageEvent e)
	{
		int damageAmount = e.DamageAmount;
		currentHealth-= damageAmount;
		if(currentHealth<=0)
		{
			// GD.Print("Enemy is dead");
			// GetNode<Label>("Visuals/Health").Text = "0";
			healthBar.Health = -damageAmount;
			idle.Visible = false;
			takeHit.Visible = false;
			death.Visible = true;
			animationPlayer.Play("enemy_DEATH");
		
		}
		else
		{
			// GetNode<Label>("Visuals/Health").Text = currentHealth.ToString();
			// GD.Print($"I have taken {damageAmount} amount of damage.");
			healthBar.Health = -damageAmount;
			//Play take hit animation
			idle.Visible = false;
			takeHit.Visible = true;
			animationPlayer.Play("enemy_TAKEHIT");
			
		}

		

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
