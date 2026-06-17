using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public partial class Deck : Node2D
{
	[Export] CardData[] cardData;
	[Export] PackedScene cardBack;
	// Called when the node enters the scene tree for the first time.

	private List<CardData> cardDataList = new List<CardData>();
	public override void _Ready()
	{
		cardDataList = cardData.ToList<CardData>();
		ShuffleDeck();
		DrawDeckToScreen();
	}


	public CardData Draw()
	{
		//We also need to know if we have card slots open: IE: is our hand full.
		//The deck doesn't need to know about the player hand, it should not care.
		//We will have to figure this out later, for now let's not worry to much about it.
		if(cardDataList.Count > 0)
		{
			//Get the top card
			int topCardIndex = cardDataList.Count- 1;
			CardData topCard = cardDataList[topCardIndex];
			cardDataList.RemoveAt(topCardIndex);
			DrawDeckToScreen();
			return topCard;
		}

		return null;
	}

	//Drawing the DRAW pile deck to the screen.
	private void DrawDeckToScreen()
	{
        //Loop the list and stack that many card backs, offset the next card to the bottom just a little to show it's a stack
        int loopIndex = 0;
        var cardSpawnLocation = GetNode<Node2D>("CardDeck");//.AddChild(card);
		ClearDeckSpawnLocation(cardSpawnLocation);
        foreach (CardData cardData in cardDataList)
        {
            CardBack card = cardBack.Instantiate<CardBack>();
            
            cardSpawnLocation.AddChild(card);
            card.GlobalPosition = new Vector2(cardSpawnLocation.GlobalPosition.X + (loopIndex * 1.5f), cardSpawnLocation.GlobalPosition.Y + (loopIndex * 1.5f));// Y = loopIndex * .5f;
			card.DrawButtonClick += () => { Draw(); };
            loopIndex++;
        }
    }
	private void ClearDeckSpawnLocation(Node2D spawnLocation)
	{
		foreach(Node2D visualCard in spawnLocation.GetChildren())
			visualCard.CallDeferred("queue_free");
	}

	private void ShuffleDeck()
	{
		List<CardData> newShuffledDeck = new List<CardData>();
		RandomNumberGenerator rng = new RandomNumberGenerator();
		//Move cards around in the list to a random index, but don't use the count of the list and a possible number.
		foreach(CardData card in cardDataList)
		{
			newShuffledDeck.Insert(rng.RandiRange(0, newShuffledDeck.Count), card);
		}

		//GD.Print("Before Shuffle");
		//PrintDeckLists();
		cardDataList = newShuffledDeck;
		//GD.Print("After Shuffle");
		//PrintDeckLists();
	}

	private void PrintDeckLists()
	{
		foreach(CardData card in cardDataList)
		{
			GD.Print(card.CardName);
		}
	}
}
