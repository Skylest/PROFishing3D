using UnityEngine;
using static IFish;

[CreateAssetMenu(fileName = "Fish", menuName = "ScriptableObjects/Fish", order = 1)]
public class FishScriptableObject : ScriptableObject, IFish
{
    [Header("Fish Info")]
    [SerializeField] private string fishName;
    [SerializeField] private string fishDescription;
    private float speed;
    private float weight;
    private FishRarity rarity;
    private GameObject fishModel;
    private bool wasHooked;
    [SerializeField] private int hookProbability;

    public string Name => fishName;
    public string Description => fishDescription;
    public float Speed => speed;
    public float Weight => weight;
    public FishRarity Rarity => rarity;
    public GameObject FishModel => fishModel;
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
    [SerializeField] private FishRarity minRarity;
    [SerializeField] private FishRarity maxRarity;

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
            {
                rarity += 1;
                Debug.Log(rarity.ToString());
            }
        }
    }

    private float GetRarityCoef(FishRarity fishRarity)
    {
        return fishRarity switch
        {
            FishRarity.Common => 1f,
            FishRarity.Uncommon => 1.25f,
            FishRarity.Rare => 1.5f,
            FishRarity.Mythical => 2f,
            FishRarity.Legendary => 3f,
            _ => 1f
        };
    }

    private float GetRarityGrowthProbabiluty()
    {
        return rarity switch
        {
            FishRarity.Common => 0.33f,
            FishRarity.Uncommon => 0.2f,
            FishRarity.Rare => 0.1f,
            FishRarity.Mythical => 0.05f,
            FishRarity.Legendary => 0f,
            _ => 0f
        };
    }
}