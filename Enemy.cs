using Godot;
using System;

public partial class Enemy : Node2D
{

	private int currentHealth;
	private AnimationPlayer animationPlayer;
	private Sprite2D idle;
	private Sprite2D takeHit;
	private Sprite2D death;
	private Sprite2D attack;

	private HealthBar healthBar;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		animationPlayer.Play("enemy_IDLE");
		idle = GetNode<Sprite2D>("Visuals/Idle");
		takeHit = GetNode<Sprite2D>("Visuals/TakeHit");
		death = GetNode<Sprite2D>("Visuals/Death");
		attack = GetNode<Sprite2D>("Visuals/Attack");
		animationPlayer.AnimationFinished += AnimationFinished;
		healthBar = GetNode<HealthBar>("Visuals/HealthBar");
		
		
		GameManager.Instance.GetBus().Subscribe<DealDamageEvent>(TakeDamage);
		GameManager.Instance.GetBus().Subscribe<BossTurnStartEvent>(OnBossTurnStartEventHandler);
		currentHealth = healthBar.Health;
		isDead = false;
		
	}


	private void OnBossTurnStartEventHandler(BossTurnStartEvent e)
	{
		//Move Boss forward, similar to the player
		
		//Check to see if the enemy can take it's turn
		
		if(!isDead)
		{
			GD.Print($"Player Turn {GameManager.Instance.IsPlayerTurn}");
			idle.Visible = false;
			attack.Visible = true;
			animationPlayer.Play("enemy_ATTACK");
			// GameManager.Instance.GetBus().Publish(new DealPlayerDamageEvent(2));
			//Deal Damage to the player
			//This is one place we can initiate the Deal Damage to Player.
			//Should this be on the bus?
			//If only 1 object needs to know, is it worth put on the bus???
			GameManager.Instance.GetBus().Publish(new EnemyTurnEndEvent());
		}
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
			isDead = true;
		
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
		switch(animation)
		{
			case "enemy_TAKEHIT":
				takeHit.Visible = false;
				idle.Visible = true;
				animationPlayer.Play("enemy_IDLE");
			break;

			case "enemy_ATTACK":
				// GameManager.Instance.GetBus().Publish(new DealPlayerDamageEvent(2));
				attack.Visible = false;
				idle.Visible = true;
				animationPlayer.Play("enemy_IDLE");

			break;

			default:
				GD.Print("Animation Finished default");
			break;
		}
	}

	private void SendDamageToPlayer()
	{
		GameManager.Instance.GetBus().Publish(new DealPlayerDamageEvent(2));
	}

	public bool isDead
	{
		get;
		protected set;
	}
}
