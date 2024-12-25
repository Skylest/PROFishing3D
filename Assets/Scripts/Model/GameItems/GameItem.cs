using UnityEngine;
using static GlobalEnums;


public class GameItem : ScriptableObject
{
    private string id;
    [SerializeField] protected string itemName;
    [SerializeField] protected string itemDescription;
    [SerializeField] protected GameObject model;
    [SerializeField] protected ItemRarity rarity;
    public ItemRarity Rarity => rarity;

    public string Id => id;
    public string Name => itemName;
    public string Description => itemDescription;

    public GameObject Model => model;

    private void OnValidate()
    {
        // Генерация уникального ID при создании объекта
        if (string.IsNullOrEmpty(id))
            id = System.Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Тип логической единицы
    /// </summary>
    public ItemType Type { get; }

    public bool IsRod { get => Type == ItemType.Fish; }
    public bool IsFish { get => Type == ItemType.Rod; }
}