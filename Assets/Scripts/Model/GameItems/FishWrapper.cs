using UnityEngine;
using static GlobalEnums;

public class FishWrapper : IFish
{
    private string fishName;
    private string fishDescription;
    private float speed;
    private float weight;
    private ItemRarity rarity;
    private GameObject model;
    private bool wasHooked;
    private int hookProbability;

    public string Name => fishName;
    public string Description => fishDescription;
    public float Speed => speed;
    public float Weight => weight;
    public ItemRarity Rarity => rarity;
    public GameObject Model => model;
    public bool WasHooked { get => wasHooked; set => wasHooked = value; }
    public int HookProbability => hookProbability;

    public void CopyFrom(Fish fishScriptableObject)
    {
        fishName = fishScriptableObject.Name;
        fishDescription = fishScriptableObject.Description;
        speed = fishScriptableObject.Speed;
        weight = fishScriptableObject.Weight;
        rarity = fishScriptableObject.Rarity;
        model = fishScriptableObject.Model;
        wasHooked = fishScriptableObject.WasHooked;
        hookProbability = fishScriptableObject.HookProbability;
    }
}