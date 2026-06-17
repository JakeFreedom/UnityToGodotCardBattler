using Godot;
using System;

public partial class GameManager : Node2D
{

    private bool IsCardBeingDragged = false;
    private string CardBeingDragged = string.Empty;


    public bool IsPlayerDragginCard 
    { 
        get 
        { 
            GD.Print("get is player draggin card");  
            return IsCardBeingDragged; 
        } 
        set 
        {
            GD.Print($"Card is being Dragged:{value}");
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
}
