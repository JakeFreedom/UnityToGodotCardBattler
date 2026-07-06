using Godot;
using System;

public partial class PlayerCharacter : Node2D
{
	private Sprite2D IDLE;
	private Sprite2D ATTACK;
	private AnimationPlayer thePlayer;

	private GpuParticles2D healEffect;
	public override void _Ready()
	{
		GameManager.OnCardPlayed += HandleCardPlayed;
		thePlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		thePlayer.Play("Player_IDLE");
		IDLE = GetNode<Sprite2D>("Idle");
		ATTACK = GetNode<Sprite2D>("Attack");
        thePlayer.AnimationFinished += ThePlayer_AnimationFinished;
		healEffect = GetNode<GpuParticles2D>("HealEffect");
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
		}
    }

    public override void _ExitTree()
    {
        GameManager.OnCardPlayed -= HandleCardPlayed;
    }
	public void HandleCardPlayed(CardData cardData)
	{
		//Need to make sure this was an attack card
		if(cardData.CardDamage > 0) //This is a horrible way to handle this
		{
			IDLE.Visible = false;
			ATTACK.Visible = true;
			thePlayer.Play("Player_ATTACK");
		}

		if(cardData.CardHealth > 0)
		{
			//GD.Print("Health Card Played");
			//From here we will need to access the player health system
			//and we will need to fire off a particle effect to show some affordance to the player that something happened.
			//In the tut video they are using a heart, which we will do the same.
			healEffect.OneShot = true;
			healEffect.Emitting = true;
		}
	}
}
