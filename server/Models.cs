// Models/Character.cs
public class Character
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Tier { get; set; }
    public int MainPoolPoints { get; set; }
    public int CombatPoints { get; set; }
    public int UtilityPoints { get; set; }
    public int SpecialPoints { get; set; }

    // Combat Attributes
    public CombatAttributes CombatAttributes { get; set; } = new();

    // Utility Attributes
    public UtilityAttributes UtilityAttributes { get; set; } = new();

    // Collections
    public List<Expertise> Expertise { get; set; } = new();
    public List<SpecialAttack> SpecialAttacks { get; set; } = new();
    public List<UniquePower> UniquePowers { get; set; } = new();
}

// Models/CombatAttributes.cs
public class CombatAttributes
{
    public int Id { get; set; }
    public int Focus { get; set; }
    public int Power { get; set; }
    public int Mobility { get; set; }
    public int Endurance { get; set; }

    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
}

// Models/UtilityAttributes.cs
public class UtilityAttributes
{
    public int Id { get; set; }
    public int Awareness { get; set; }
    public int Communication { get; set; }
    public int Intelligence { get; set; }

    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
}

// Models/Expertise.cs
public class Expertise
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Cost { get; set; }
    public string Description { get; set; } = string.Empty;

    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
}

// Models/SpecialAttack.cs
public class SpecialAttack
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AttackType { get; set; } = string.Empty;
    public string EffectType { get; set; } = string.Empty;
    public List<string> Limits { get; set; } = new();
    public List<string> Upgrades { get; set; } = new();

    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
}

// Models/UniquePower.cs
public class UniquePower
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Cost { get; set; }
    public string Description { get; set; } = string.Empty;

    public int CharacterId { get; set; }
    public Character Character { get; set; } = null!;
}
