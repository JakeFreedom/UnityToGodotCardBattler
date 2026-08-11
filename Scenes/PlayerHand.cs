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
	private List<Card> playerHand = new List<Card>();
	private List<Node2D> cardSlots = new List<Node2D>();

	// private List<Card> playersCards = new List<Card>();



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


		dp = GetParent().GetNode<DiscardPile>("DiscardPile") as DiscardPile;


		GameManager.Instance.GetBus().Subscribe<CardPlayedEvent>(OnCardPlayedEventHandler);
		GameManager.Instance.GetBus().Subscribe<DrawCardEvent>(OnCardDrawnEventHandler);
		GameManager.Instance.GetBus().Subscribe<PlayerDeathEvent>(OnPlayerDeathEventHandler);
		
		LoadStartingHand();
	}

	//Get Called from deck
	private void AddDrawnCardToHand(Card card)
	{
		//GD.Print($"Add {card.GetCardData().CardName} to hand.");
		//GD.Print("Adding card to hand");
		playerHand.Add(card);
		CallDeferred("DrawToScreen");
		//DrawToScreen();
	}

	//This I don't think needs any checks. Only thing right now I can think of is if we introduce the mulligan or something along those lines
	private void LoadStartingHand()
	{
		for(int x = 0; x < 5; x++)
			deck.Draw();

		GameManager.Instance.IsPlayerHandFull = IsHandFull;
		// GD.Print($"Player Hand Size {playerHand.Count}");
	}

	private void DrawToScreen()
	{
		// GD.Print("Draw Player Hand to screen");
		ClearSlots();

		// GD.Print(playerHand.Count);
		foreach (Card drawnCard in playerHand)
		{
			Card card = baseCard.Instantiate<Card>();
			card.SetupCard(drawnCard.GetCardData(), drawnCard.GetCardID);
			
			int emptyIndex = GetNextCardSlot();//cardSlots.FindIndex(item => item.GetChildCount() == 0); //Get the first slot that doesn't have a child
			if (emptyIndex == -1)
				return;


			cardSlots[emptyIndex].AddChild(card);

		}
	}

    private void OnCardPlayedEventHandler(CardPlayedEvent e)
    {
		//Here we will need to make sure that card being played is the card we react to, or else all the card in the hand with that same name
		//will be discarded....
		GD.Print($"Card ID {GameManager.Instance.CardBeingDraggedByID} Incoming Event Card ID {e.card.GetCardID}");
	
		if(GameManager.Instance.CardBeingDraggedByID == e.card.GetCardID)
		{
			// GD.Print("The card being dragged is the correct card.");

			//GD.Print($"Player Hand Card Played Event Handler {GameManager.Instance.CardBeingDraggedByID} -- {e.card.GetCardID}");
			Card card = e.card;
			Card findCard = playerHand.Find(t => t.GetCardID==card.GetCardID);
			GD.Print($"Card ID {card.GetCardID}--Find Card ID{findCard.GetCardID}");
			playerHand.Remove(findCard);
			findCard.CallDeferred("queue_free");

	
			//Move to discard pile -- Need ref to discard pile -- We need the event bus right now.
			dp.Discard(card.GetCardData(), card);
			GameManager.Instance.CardBeingDraggedByID = -1;
			GameManager.Instance.IsPlayerDragginCard = false;

			//This will keep all the card to the left side of that player hand, not allowing for empty slots.
			//The newly drawn card will always be to the far right. If there are empty slots.
			//ClearSlots();
			CallDeferred("DrawToScreen");
			//GameManager.CardPlayed(card.GetCardData());

			GameManager.Instance.IsPlayerHandFull = IsHandFull;
		}
    }

	private void OnCardDrawnEventHandler(DrawCardEvent e)
	{
		
		AddDrawnCardToHand(e.EventCard);
		GameManager.Instance.IsPlayerHandFull = IsHandFull;

	}

	private void OnPlayerDeathEventHandler(PlayerDeathEvent e)
	{
		GD.Print("On Player Death Event Handler");
		//Lock Hand
		DisableHand();
	}

	private void DisableHand()
	{
		foreach(Card card in playerHand)
		{
			card.IsInteractable = false;
		}
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
				// slot.GetChild(0).CallDeferred("queue_free");
			}
		}
	}

	public bool IsHandFull
	{
		get {  return playerHand.Count >= 5; }
	}
	//Need a way to expose slots in use vs. max hand size.
}
