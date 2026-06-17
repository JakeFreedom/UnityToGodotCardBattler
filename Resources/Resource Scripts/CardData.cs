using Godot;
using System;

public partial class CardData : Resource
{
    [Export] public string CardName;
    [Export] public string CardDescription;
    [Export] public int CardDamage;
    [Export] public int CardHealth;
    [Export] public int ActivationCost;
    [Export] public CardType Type;
    [Export] public CardRarity Rarity;
    [Export] public Texture2D AbilityImage;







    public enum CardType
    {
        CREATURE,
        INSTANT,

    }

    public enum CardRarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        MYTHIC
    }
}
