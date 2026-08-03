public class DealPlayerDamageEvent : GameEvent
{
    public DealPlayerDamageEvent(int damageAmount)//<--Again this should/will be card data.
    {
        DamageAmount = damageAmount;
    }

    public int DamageAmount {get; protected set;}
}