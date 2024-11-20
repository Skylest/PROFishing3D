using UnityEngine;
using UnityEngine.SceneManagement;

public class FishingModel
{
    //TODO а шо если слишком близко??? Имба получается. А вдали нереально
    private const float DistanceFromPlayer = 100f;
    private const float EscapeDistanceFromPlayer = 110f;
    private const float SectorAngle = 80f;

    private int totalProbability = 0;

    private Vector3 escapeVector;

    private FishScriptableObject[] allFishesPrefabs;
    private FishWrapper currentFish = new FishWrapper();

    public void PrepareLocationConfig(int locNum) 
    {
        string sceneName = SceneManager.GetActiveScene().name;
        allFishesPrefabs = Resources.LoadAll<FishScriptableObject>($"Maps/{sceneName}/Loc{locNum}");

        for (int i = 0; allFishesPrefabs.Length > i; i++)
            totalProbability += allFishesPrefabs[i].HookProbability;
    }
    public FishWrapper PrepareFish(out float time)
    {
        time = Random.Range(3f, 15f);

        FishScriptableObject fish = new FishScriptableObject();
        fish.RandomizeParameters();

        currentFish.CopyFrom(fish);
        return currentFish;
    }    

    public Vector3 GetEscapePoint(Transform player)
    {
        Vector3 forward = player.forward;

        float randomAngle = Random.Range(-SectorAngle / 2, SectorAngle / 2);

        Vector3 escapeDirection = Quaternion.Euler(0, randomAngle, 0) * forward;

        Vector3 escapePoint = player.position + escapeDirection.normalized * DistanceFromPlayer;

        return escapePoint;
    }

    public void ChangeEscapeVector()
    {
        
    }

    public FishScriptableObject PickRandomFish()
    {
        int randomValue = Random.Range(0, totalProbability);
        int currentWeightSum = 0;

        for (int i = 0; allFishesPrefabs.Length > i; i++)
        {
            currentWeightSum += allFishesPrefabs[i].HookProbability;
            if (randomValue < currentWeightSum)            
                return allFishesPrefabs[i];            
        }

        return null; // На случай, если что-то пойдет не так
    }

    private void GetFish()
    {
        //TODO Sadok add currentFish
        //И проверить как мне добавлять рыбу, чтоб я случайно не передавал ссылку в массив
    }
}