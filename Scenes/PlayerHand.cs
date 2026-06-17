using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class PlayerHand : Node2D
{

	//Right now we will limit the hand size to just 5 cards -- No matter what
	[Export] Deck deck;
	[Export] PackedScene baseCard;
	//Player hand needs to know about card slots
	//Deck to draw from
	//List of cards in our current hand
	private List<CardData> playerHand = new List<CardData>();
	private List<Node2D> cardSlots = new List<Node2D>();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Godot.Collections.Array<Node> slots = GetNode("CardSlots").GetChildren();
		foreach(Node node in slots)
		{
			//GD.Print(node.Name);
			cardSlots.Add((Node2D)node);
		}
		//GD.Print(cardSlots.Count);
		LoadStartingHand();
	}
    public override void _Process(double delta)
    {
	
    }

	//This I don't think needs any checks. Only thing right now I can think of is if we introduce the mulligan or something along those lines
	private void LoadStartingHand()
	{
		for(int x = 0; x < 5; x++)
		{
			//Draw Five cards
			CardData drawnCard = deck.Draw();//This gives us a card ref
			playerHand.Add(drawnCard);
		}

		//Draw cards to the screen as they are drawn from the deck
		DrawToScreen();
	}

	private void DrawToScreen()
	{
		GD.Print("Draw to Screen");
		foreach (CardData drawnCard in playerHand)
		{
			Card card = baseCard.Instantiate<Card>() as Card;
			int emptyIndex = cardSlots.FindIndex(item => item.GetChildCount() == 0); //Get the first slot that doesn't have a child
			cardSlots[emptyIndex].AddChild(card);
			card.SetupCard(drawnCard);
		}
	}

	private int GetNextCardSlot()
	{
        int emptyIndex = cardSlots.FindIndex(item => item.GetChildCount() == 0); //Get the first slot that doesn't have a child
		return emptyIndex;
    }

	//Need a way to expose slots in use vs. max hand size.
}
