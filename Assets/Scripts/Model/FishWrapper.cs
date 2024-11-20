using UnityEngine;
using static IFish;

public class FishWrapper : IFish
{
    private string fishName;
    private string fishDescription;
    private float speed;
    private float weight;
    private FishRarity rarity;
    private GameObject fishModel;
    private bool wasHooked;
    private int hookProbability;

    public string Name => fishName;
    public string Description => fishDescription;
    public float Speed => speed;
    public float Weight => weight;
    public FishRarity Rarity => rarity;
    public GameObject FishModel => fishModel;
    public bool WasHooked { get => wasHooked; set => wasHooked = value; }
    public int HookProbability => hookProbability;

    public void CopyFrom(FishScriptableObject fishScriptableObject)
    {
        fishName = fishScriptableObject.Name;
        fishDescription = fishScriptableObject.Description;
        speed = fishScriptableObject.Speed;
        weight = fishScriptableObject.Weight;
        rarity = fishScriptableObject.Rarity;
        fishModel = fishScriptableObject.FishModel;
        wasHooked = fishScriptableObject.WasHooked;
        hookProbability = fishScriptableObject.HookProbability;
    }
}