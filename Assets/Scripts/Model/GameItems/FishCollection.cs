using UnityEngine;

[CreateAssetMenu(fileName = "FishCollection", menuName = "ScriptableObjects/Fishing/Fish")]
public class FishCollection : ScriptableObject
{
    public Fish[] fishes;
}
