using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class ItemManager : MonoBehaviour
{
    /// <summary>
    /// Экземпляр ItemManager
    /// </summary>
    private static ItemManager instance;

    /// <summary>
    /// Получение единственного экземпляра ItemManager
    /// </summary>
    public static ItemManager Instance { get => instance; }

    public GameItem[] allGameItems; // Все доступные ScriptableObject

    private List<GameItem> inventoryItems;
    private List<FishWrapper> fishCage;
    
    private float coins = 0f, diamonds = 0f;

    [SerializeField] private Rod defoltRod;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // Загрузка данных игрока
        //PlayerData data = SaveManager.LoadPlayerData();
        string data = null;
        if (data != null)
        {

            // Восстановление инвентаря
           // inventoryItems = allGameItems.FindAll(item => data.inventoryObjectIds.Contains(item.Id));
        }
        else
        {
            allGameItems = Resources.LoadAll<GameItem>("Items");
            print(allGameItems);
            inventoryItems = new List<GameItem>() { defoltRod };
            defoltRod.IsGetted = defoltRod.IsSelected = true;
        }
    }

    public void SaveGame()
    {
        // Создаём PlayerData
        /* PlayerData data = new PlayerData
         {
             selectedObjectId = selectedItem != null ? selectedItem.Id : null,
             inventoryObjectIds = inventoryItems.ConvertAll(item => item.Id)
         };

         // Сохраняем данные
         SaveManager.SavePlayerData(data);*/
    }

    public Rod GetSelectedRod()
    {
        //return inventoryItems.Find(rod => rod.IsSelected);
        return defoltRod;
    }
}