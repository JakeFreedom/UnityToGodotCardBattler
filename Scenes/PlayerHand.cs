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



	private DiscardPile dp;

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

		dp = GetParent().GetNode<DiscardPile>("DiscardPile") as DiscardPile;
		
	}

	public void AddDrawnCardToHand(CardData card)
	{
		//GD.Print($"Add {card.CardName} to hand.");
		//GD.Print("Adding card to hand");
		playerHand.Add(card);
		DrawToScreen();
	}

	//This I don't think needs any checks. Only thing right now I can think of is if we introduce the mulligan or something along those lines
	private void LoadStartingHand()
	{
		for(int x = 0; x < 5; x++)
		{
			//GD.Print($"Loading Starting Hand: Current Index {x}");
			//Draw Five cards
			CardData drawnCard = deck.Draw();//This gives us a card ref
		}

		//Draw cards to the screen as they are drawn from the deck
		///DrawToScreen();
	}

	private void DrawToScreen()
	{
		ClearSlots();
		//GD.Print("Draw to Screen");
		//GD.Print($"Player Hand Size{playerHand.Count}");
		foreach (CardData drawnCard in playerHand)
		{
			Card card = baseCard.Instantiate<Card>() as Card;
			int emptyIndex = cardSlots.FindIndex(item => item.GetChildCount() == 0); //Get the first slot that doesn't have a child
			//GD.Print($"Card {drawnCard.CardName} was put into slot {emptyIndex}. Player hand size is: {playerHand.Count}");
			if (emptyIndex == -1)
				return;

			cardSlots[emptyIndex].AddChild(card);
			//cardSlots[emptyIndex].CallDeferred("add_child", card);
			card.SetupCard(drawnCard, GameManager.Instance.GetNextCardIndex);
			card.CardWasPlayed -= Card_CardWasPlayed;
            card.CardWasPlayed += Card_CardWasPlayed;
		}
	}

    private void Card_CardWasPlayed(Card card)
    {
		playerHand.Remove(card.GetCardData());
		//GD.Print(playerHand.Count);
		card.CallDeferred("queue_free");
		//Move to discard pile -- Need ref to discard pile -- We need the event bus right now.
		dp.Discard(card.GetCardData(), card);

		//This will keep all the card to the left side of that player hand, not allowing for empty slots.
		//The newly drawn card will always be to the far right. If there are empty slots.
		//ClearSlots();
		DrawToScreen();
    }

    private int GetNextCardSlot()
	{
        int emptyIndex = cardSlots.FindIndex(item => item.GetChildCount() == 0); //Get the first slot that doesn't have a child
		return emptyIndex;
    }

	private void ClearSlots()
	{
		foreach(Node2D slot in cardSlots)
		{
			//GD.Print($"Clear Slot {slot.Name} Total Children: {slot.GetChildren()}");
			if(slot.GetChildCount() > 0)
			{
				slot.RemoveChild(slot.GetChild(0));
			}
		}
	}

	public bool IsHandFull
	{
		get {  return playerHand.Count < 5; }
	}
	//Need a way to expose slots in use vs. max hand size.
}
