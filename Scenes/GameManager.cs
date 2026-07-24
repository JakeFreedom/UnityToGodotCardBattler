using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class GameManager : Node2D
{

    private static IEventBus<GameEvent> bus;
    private bool IsCardBeingDragged = false;
    private string CardBeingDragged = string.Empty;
    private int CardIDBeingDragged = -1;
    private int CardIndex = 0;
    public static GameManager Instance { get; private set; }
    public static Action<CardData> OnCardPlayed;
    public static Action<int> OnDealDamage;

    public GameManager()
    {
        // if(Instance == null)
        // {
        //     Instance = this;
        //     bus = new EventBus<GameEvent>();
            

        // }

        // GD.Print("Game Manager Constructor called");
    }

    public override void _Notification(int what)
    {
        if(what == NotificationSceneInstantiated)
        {
            Instance = this;
            bus = new EventBus<GameEvent>();
            GD.Print("Notifiation was called");
        }
    }

    public bool IsPlayerDragginCard 
    { 
        get 
        { 
            //GD.Print("get is player draggin card");  
            return IsCardBeingDragged; 
        } 
        set 
        {
            //GD.Print($"Card is being Dragged:{value} Card Name: {CardNameBeingDragged}");
            IsCardBeingDragged = value; 
        } 
    }
    public string CardNameBeingDragged 
    { 
        get 
        { 
            return CardBeingDragged; 
        } 
        set 
        { 
            CardBeingDragged = value; 
        } 
    }

    //public static void CardPlayed(CardData cardData) => OnCardPlayed?.Invoke(cardData);
    //public static void DealDamage(int damageAmount) => OnDealDamage?.Invoke(damageAmount);
    public int GetNextCardIndex { get { return CardIndex++; } }
    public int CardBeingDraggedByID { get {  return CardIDBeingDragged; }  set { CardIDBeingDragged = value; } }

    public IEventBus<GameEvent> GetBus() => bus;


}
