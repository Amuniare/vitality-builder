// Create Services/CombatService.cs
public class CombatService
{
    public (bool success, int effect) ResolveAttack(
        int attackerTier,
        int attackerFocus,
        int attackerPower,
        int targetMobility,
        int targetEndurance)
    {
        // Accuracy Check
        var accuracyRoll = new Random().Next(1, 21) + attackerTier + attackerFocus;
        var avoidance = 10 + attackerTier + targetMobility;
        
        if(accuracyRoll < avoidance) 
            return (false, 0);

        // Damage Calculation
        var damageRoll = Roll3d6() + attackerTier + (int)Math.Ceiling(attackerPower * 1.5);
        var durability = (int)Math.Ceiling(attackerTier + (targetEndurance * 1.5));
        
        return (true, Math.Max(0, damageRoll - durability));
    }

    private int Roll3d6()
    {
        var roll = new Random();
        int total = 0;
        int diceToRoll = 3;
        
        while(diceToRoll-- > 0)
        {
            int result = roll.Next(1, 7);
            total += result;
            if(result == 6) diceToRoll++;
        }
        
        return total;
    }
}