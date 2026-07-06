using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class HealthBar : VBoxContainer
{
	// private Control healthIcon;
	[Export]
	private int CurrentHealth;
	[Export]
	private int MaxHealth;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// healthIcon = 
		// healthIcon = healthIcon.Duplicate() as Control;
		// GetNode<HBoxContainer>("HBoxContainer").AddChild(healthIcon);
		GameManager.OnCardPlayed += CardPlayed;
		// HealthChanged(30);
		ReDrawHealthHearts();
	}


	private void HealthChanged(int healthAmount)
	{
		if(healthAmount + CurrentHealth > MaxHealth)
		{
			CurrentHealth = MaxHealth;
			ReDrawHealthHearts();

		}
		else
		{

		CurrentHealth += healthAmount;
		ReDrawHealthHearts();
			

		}
	}

	private void ReDrawHealthHearts()
	{
		GD.Print($"Current Health{CurrentHealth}");
		//Get a reference to the heart icon
		Control heartIcon = GetNode<Control>("MarginContainer/HBoxContainer/HealthIcon");
		//Clear all children
	 	GetNode<HBoxContainer>("MarginContainer/HBoxContainer").GetChildren().Clear();
		GD.Print(GetNode<HBoxContainer>("MarginContainer/HBoxContainer").GetChildren().Count);
		//Add a child for each health point
		for(int x = CurrentHealth;  x>0; x--)
		{
			GD.Print($"X={x}");
			heartIcon = heartIcon.Duplicate() as Control;
			heartIcon.Visible = true;
			//GetNode<HBoxContainer>("MarginContainer/HBoxContainer").AddChild(heartIcon);
		}
	}
	
	//This is called from the gameManager script. 
	//This doesn't seem correct at all.
	private void CardPlayed(CardData cd)
	{
		//Here we would need to check to see if the card type was a of heal card. Not just
		//the fact that it's health is greater than zero. 

		//We need something in the carddata that we can concretely look at to see what type of card it is.
		GD.Print("There was a card played, listening in the health bar");

		if(cd.CardHealth>0)
		{
			HealthChanged(cd.CardHealth);

		}
			// GD.Print("This was a heal card, simply because we looked at Card Health.");
			//This is a horrible method for a card decision tree.
	}
}
