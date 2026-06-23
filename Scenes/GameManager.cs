using Godot;
using System;

public partial class GameManager : Node2D
{

    private bool IsCardBeingDragged = false;
    private string CardBeingDragged = string.Empty;
    private int CardIDBeingDragged = -1;
    private int CardIndex = 0;
    public static GameManager Instance { get; private set; }

    public override void _Notification(int what)
    {
        if(what == NotificationSceneInstantiated)
        {
            Instance = this;
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

    public int GetNextCardIndex { get { return CardIndex++; } }

    public int CardBeingDraggedByID { get {  return CardIDBeingDragged; }  set { CardIDBeingDragged = value; } }

}
