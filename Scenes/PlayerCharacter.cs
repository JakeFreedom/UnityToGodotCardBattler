using Godot;
using System;

public partial class PlayerCharacter : Node2D
{
	private Sprite2D IDLE;
	private Sprite2D ATTACK;
	private Sprite2D TAKEHIT;
	private Sprite2D DEATH;
	private AnimationPlayer thePlayer;
	private Card cardThatWasPlayed;
	private GpuParticles2D healEffect;

	private HealthBar2 hb;


	public override void _Ready()
	{
		//GameManager.OnCardPlayed += HandleCardPlayed;
		thePlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		thePlayer.Play("Player_IDLE");
		IDLE = GetNode<Sprite2D>("Idle");
		ATTACK = GetNode<Sprite2D>("Attack");
		TAKEHIT = GetNode<Sprite2D>("TakeHit");
		DEATH = GetNode<Sprite2D>("Death");
        thePlayer.AnimationFinished += ThePlayer_AnimationFinished;
		healEffect = GetNode<GpuParticles2D>("HealEffect");
		hb = GetNode<HealthBar2>("HealthBar-2");
		//TurnEvents.OnPlayerTurnEnd += HandleOnPlayerTurnEnd;
		GameManager.Instance.GetBus().Subscribe<CardPlayedEvent>(HandleCardPlayed);
		GameManager.Instance.GetBus().Subscribe<DealPlayerDamageEvent>(OnDealPlayerDamageEventHandler);
		GameManager.Instance.GetBus().Subscribe<EnemyTurnEndEvent>(OnEnemyTurnEndEventHandler);
		
	}
    public override void _ExitTree()
    {
        //GameManager.OnCardPlayed -= HandleCardPlayed;
		GameManager.Instance.GetBus().Unsubscribe<CardPlayedEvent>(HandleCardPlayed);
    }

	private void OnEnemyTurnEndEventHandler(EnemyTurnEndEvent e)
	{
		GameManager.Instance.IsPlayerTurn = true;
	}
	private void OnDealPlayerDamageEventHandler(DealPlayerDamageEvent e)
	{
		// GD.Print($"Damage to the Player {e.DamageAmount}");
		thePlayer.Play("player_TAKEHIT");
		//hb.Health = -e.DamageAmount;
		IDLE.Visible = false;
		TAKEHIT.Visible = true;
		GameManager.Instance.GetBus().Publish(new HealthChangeEvent(e.DamageAmount, false));


	}
    private void ThePlayer_AnimationFinished(StringName animName)
    {
		switch (animName)
		{
			case "Player_ATTACK":
				IDLE.Visible = true;
				ATTACK.Visible = false;
				thePlayer.Play("Player_IDLE");
				break;

			case "player_TAKEHIT":
				if(hb.GetCurrentHealth <= 0)
				{
					//Play death animation
					GD.Print("Death");
					TAKEHIT.Visible = false;
					DEATH.Visible = true;
					thePlayer.Play("Player_DEATH");
					GameManager.Instance.GetBus().Publish(new PlayerDeathEvent());
				}
				else
				{
					IDLE.Visible = true;
					TAKEHIT.Visible = false;
					thePlayer.Play("Player_IDLE");
				}
				break;
		}
    }

	// private void HandleOnPlayerTurnEnd()
	// {
	// 	GD.Print("Player Turn End!!!!");
	// 	GameManager.Instance.IsPlayerTurn = false;
	// }

	private void HandleCardPlayed(CardPlayedEvent EventData)
	{
		
		cardThatWasPlayed = EventData.card;
		//Need to make sure this was an attack card
		if(EventData.card.GetCardData().CardDamage > 0) //This is a horrible way to handle this
		{
			IDLE.Visible = false;
			ATTACK.Visible = true;
			thePlayer.Play("Player_ATTACK");
		}

		if(EventData.card.GetCardData().CardHealth > 0)
		{
			//GD.Print("Health Card Played");
			//From here we will need to access the player health system
			//and we will need to fire off a particle effect to show some affordance to the player that something happened.
			//In the tut video they are using a heart, which we will do the same.
			healEffect.OneShot = true;
			healEffect.Emitting = true;

			//Find our health bar and update it
			//GetNode<HealthBar2>("HealthBar-2").GetCurrentHealth = EventData.card.GetCardData().CardHealth;
			GameManager.Instance.GetBus().Publish(new HealthChangeEvent(EventData.card.GetCardData().CardHealth, true));
		}
		// TurnEvents.PlayerTurnEnd();
	}

	//This is called at the end of the attack animation
	private void DealDamage()
	{
		//GD.Print("Deal damage to the enemy");
		// GameManager.DealDamage(cardThatWasPlayed.GetCardData().CardDamage);
		GameManager.Instance.GetBus().Publish(new DealDamageEvent{DamageAmount = cardThatWasPlayed.GetCardData().CardDamage});
	}
}
