using UnityEngine;
using static GlobalEnums;

public interface IFish
{
    string Name { get; }
    string Description { get; }
    float Speed { get; }
    float Weight { get; }
    ItemRarity Rarity { get; }
    GameObject Model { get; }
    bool WasHooked { get; set; }
    int HookProbability { get; }
}

