using UnityEngine;
using static GlobalEnums;

[CreateAssetMenu(fileName = "Fish", menuName = "ScriptableObjects/Fishing/Fish", order = 1)]
public class Fish : GameItem, IFish
{
    private float speed;
    private float weight;
    private bool wasHooked;
    [SerializeField] private int hookProbability;

    public float Speed => speed;
    public float Weight => weight;
    public bool WasHooked { get => wasHooked; set => wasHooked = value; }
    public int HookProbability => hookProbability;

    [Header("Speed Range")]
    [Tooltip("Value contains coefficient of minimal rarity")]
    [SerializeField] private float minSpeed;
    [Tooltip("Value contains coefficient of minimal rarity")]
    [SerializeField] private float maxSpeed;

    [Header("Weight Range")]
    [Tooltip("Value contains coefficient of minimal rarity")]
    [SerializeField] private float minWeight;
    [Tooltip("Value contains coefficient of minimal rarity")]
    [SerializeField] private float maxWeight;

    [Header("Rarity Range")]
    [SerializeField] private ItemRarity minRarity;
    [SerializeField] private ItemRarity maxRarity;

    public void RandomizeParameters()
    {
        SetRandomRarity();
        
        speed = Random.Range(minSpeed, maxSpeed) / GetRarityCoef(minRarity) * GetRarityCoef(rarity);
        weight = Random.Range(minWeight, maxWeight) / GetRarityCoef(minRarity) * GetRarityCoef(rarity);
    }

    private void SetRandomRarity()
    {
        rarity = minRarity;
        bool canGrowth = false;
        
        while (rarity == maxRarity || !canGrowth)
        {            
            float growthProbabiluty = GetRarityGrowthProbabiluty();
            canGrowth = Random.value < growthProbabiluty;
            if (canGrowth)            
                rarity += 1;            
        }
    }

    private float GetRarityCoef(ItemRarity fishRarity)
    {
        return fishRarity switch
        {
            ItemRarity.Common => 1f,
            ItemRarity.Uncommon => 1.25f,
            ItemRarity.Rare => 1.5f,
            ItemRarity.Mythical => 2f,
            ItemRarity.Legendary => 3f,
            _ => 1f
        };
    }

    private float GetRarityGrowthProbabiluty()
    {
        return rarity switch
        {
            ItemRarity.Common => 0.33f,
            ItemRarity.Uncommon => 0.2f,
            ItemRarity.Rare => 0.1f,
            ItemRarity.Mythical => 0.05f,
            ItemRarity.Legendary => 0f,
            _ => 0f
        };
    }
}