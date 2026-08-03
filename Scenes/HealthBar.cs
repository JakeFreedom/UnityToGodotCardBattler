using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class HealthBar : Control
{
	// private Control healthIcon;
	[Export]
	private int CurrentHealth;
	[Export]
	private int MaxHealth;

	[Export]
	private PackedScene heartIcon;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// GameManager.OnCardPlayed += CardPlayed;
		ReDrawHealthHearts();
	}


	private void HealthChanged(int healthAmount)
	{
	
		if(healthAmount + CurrentHealth <= 0)
		{

			CurrentHealth = 0;
			// ReDrawHealthHearts();
			ClearHearts();
			//Send out event on the bus, game over.
			return;
		}

		if(healthAmount + CurrentHealth > MaxHealth)
			CurrentHealth = MaxHealth;
		else
			CurrentHealth += healthAmount;
		
		ReDrawHealthHearts();
	}

	private void ReDrawHealthHearts()
	{
		ClearHearts();
		//Add a child for each health point
		for(int x = CurrentHealth;  x>0; x--)
		{
			var healthIcon = heartIcon.Instantiate();
			GetNode<HBoxContainer>("MarginContainer/HBoxContainer").AddChild(healthIcon);
		}
	}
	
	//This is called from the gameManager script. 
	//This doesn't seem correct at all.
	// private void CardPlayed(CardData cd)
	// {
	// 	//Here we would need to check to see if the card type was a of heal card. Not just
	// 	//the fact that it's health is greater than zero. 

	// 	//We need something in the carddata that we can concretely look at to see what type of card it is.
	// 	// GD.Print("There was a card played, listening in the health bar");

	// 	// if(cd.CardHealth>0)
	// 	// {
	// 	// 	HealthChanged(cd.CardHealth);

	// 	// }

	// 	// if(cd.CardDamage>0)
	// 	// {
	// 	// 	HealthChanged(-cd.CardDamage);
	// 	// }
	// 		// GD.Print("This was a heal card, simply because we looked at Card Health.");
	// 		//This is a horrible method for a card decision tree.
	// }

	private void ClearHearts()
	{
		foreach(Node n in GetNode<HBoxContainer>("MarginContainer/HBoxContainer").GetChildren())
			n.CallDeferred("queue_free");		
	}

	public int Health
	{
		set{HealthChanged(value);}
		get{return CurrentHealth;}
	}
}
