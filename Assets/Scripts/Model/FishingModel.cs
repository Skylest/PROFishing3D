using UnityEngine;
using UnityEngine.SceneManagement;

public class FishingModel
{
    private int totalProbability = 0;

    private Fish[] allFishesPrefabs;
    private FishWrapper currentFish = new FishWrapper();

    public void PrepareLocationConfig(int locNum) 
    {
        string sceneName = SceneManager.GetActiveScene().name;
        allFishesPrefabs = Resources.LoadAll<Fish>($"Maps/{sceneName}/Loc{locNum}");

        for (int i = 0; allFishesPrefabs.Length > i; i++)
            totalProbability += allFishesPrefabs[i].HookProbability;
    }

    public FishWrapper PrepareFish(out float time)
    {
        time = Random.Range(3f, 15f);

        Fish fish = PickRandomFish();
        fish.RandomizeParameters();

        currentFish.CopyFrom(fish);
        return currentFish;
    }    

    public Fish PickRandomFish()
    {
        int randomValue = Random.Range(0, totalProbability);
        int currentWeightSum = 0;

        for (int i = 0; allFishesPrefabs.Length > i; i++)
        {
            currentWeightSum += allFishesPrefabs[i].HookProbability;
            if (randomValue < currentWeightSum)            
                return allFishesPrefabs[i];            
        }

        return null;
    }

    private void GetFish()
    {
        //TODO Sadok add currentFish
        //И проверить как мне добавлять рыбу, чтоб я случайно не передавал ссылку в массив
    }
}