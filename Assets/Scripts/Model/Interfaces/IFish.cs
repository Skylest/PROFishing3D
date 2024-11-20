using UnityEngine;

public interface IFish
{
    public enum FishRarity
    {
        Common,
        Uncommon,
        Rare,
        Mythical,
        Legendary
    }

    string Name { get; }
    string Description { get; }
    float Speed { get; }
    float Weight { get; }
    FishRarity Rarity { get; }
    GameObject FishModel { get; }
    bool WasHooked { get; set; }
    int HookProbability { get; }
}

