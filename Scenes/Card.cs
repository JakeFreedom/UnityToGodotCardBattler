using Godot;


/**''
 Ideas:
	Card Zones
		A card should be aware of what zone it's in.. IE: Deck, Hand, PlayArea, Discard, Exiled.. eTC. <-- I think this would be good information to know.
 
 */

//Making a comment to see if we are on the correct repo.
public partial class Card : Node2D
{
	[Export] CardData cardData;
	[Export] Label CardName;
	[Export] Label CardDescription;
	[Export] Sprite2D AbilityImage;
	// Called when the node enters the scene tree for the first time.

	[Signal]
	public delegate void CardWasPlayedEventHandler(Card card);

	private Vector2 originalScale;
	private Vector2 originalPosition;
	private bool canMouseDrag = false;
	private GameManager gameManager;
	private int cardID;
	public override void _Ready()
	{

		GetNode<Area2D>("MouseHoverArea").MouseEntered += Card_MouseEntered;
		GetNode<Area2D>("MouseHoverArea").MouseExited += Card_MouseExited;

		originalScale = GlobalScale;
		originalPosition = GlobalPosition;
	}

	private void Card_MouseExited()
	{
		if (GameManager.Instance.CardBeingDraggedByID == this.cardID)
		{
			canMouseDrag = false;
			GameManager.Instance.CardBeingDraggedByID = -1;
			ZIndex = -1;
			GlobalScale = originalScale;
			GlobalPosition = originalPosition;
		}
	}

	private void Card_MouseEntered()
	{
		if (!GameManager.Instance.IsPlayerDragginCard && GameManager.Instance.CardBeingDraggedByID == -1)
		{
			canMouseDrag = true;
			GameManager.Instance.CardBeingDraggedByID = this.cardID;
			ZIndex = 1;
			GlobalScale = new Vector2(1.0f, 1.0f);
			GlobalPosition = new Vector2(GlobalPosition.X, GlobalPosition.Y + -40);
		}
	}

	private Vector2 GetMousePosition()
	{
		return GetViewport().GetMousePosition();
	}
	public void SetupCard(CardData cardData, int cardID)
	{
		this.cardID = cardID;
		this.cardData = cardData;
		CardName.Text = cardData.CardName;
		CardDescription.Text = cardData.CardDescription;
		AbilityImage.Texture = cardData.AbilityImage;

		GetNode<Label>("CostBackGround/Cost").Text = cardData.ActivationCost.ToString();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsMouseButtonPressed(MouseButton.Left) && canMouseDrag && GameManager.Instance.CardBeingDraggedByID == this.cardID)
		{
			GlobalPosition = GetMousePosition();
			GameManager.Instance.IsPlayerDragginCard = true;
			GameManager.Instance.CardBeingDraggedByID = this.cardID;
		}

		if (Input.IsActionJustReleased("Left_Mouse_Button"))
		{
			//This is causing a little visual bug, where the card in the discard pile will not stack visually like we want. TODO: Deal with the stacking issue --- The card needs to know it's be discarded. HENCE GET THE EVENT BUS implemented.
			GlobalPosition = originalPosition;


			GlobalScale = originalScale;
			if (GameManager.Instance.IsPlayerDragginCard)
			{
				GameManager.Instance.IsPlayerDragginCard = false;
			}
		}
	}

	private bool canClick = true;
	public override void _Input(InputEvent inputEvent)
	{
		//All this is going to do is simulate discarding for now.
		if (inputEvent is InputEventMouseButton mouseEvent)
		{
			//GD.Print(mouseEvent.ButtonIndex == MouseButton.Right);
			if (mouseEvent.ButtonIndex == MouseButton.Right && this.canMouseDrag && canClick)
			{
				//GD.Print(CardName.Text);
				canClick = false;
				//MoveToDiscardZone();
			}
			else if (mouseEvent.IsActionReleased("Right_Mouse_Button"))
			{
				canClick = true;
			}
		}
	}

	public string GetCardName() { return this.CardName.Text; }

	public void PlayCard()
	{
		//GD.Print($"Card that was played{CardName.Text}");
		//Signal to playerHand that this card was played
		canClick = false;
        canMouseDrag = false;
        GameManager.Instance.CardBeingDraggedByID = -1;
        ZIndex = -1;
        GlobalScale = originalScale;
        GlobalPosition = originalPosition;
        EmitSignal("CardWasPlayed", this);//<--How do we know who is listening... This make the code Domain and debugging difficult.
	}

	public CardData GetCardData() => this.cardData;
	//private void MoveToDiscardZone()
	//{
	//	GD.Print("Move to Discard");
	//}
}
