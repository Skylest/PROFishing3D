using UnityEngine;

[CreateAssetMenu(fileName = "Rod", menuName = "ScriptableObjects/Fishing/Rod", order = 2)]
public class Rod : GameItem
{
    [SerializeField] private float power;

    public bool IsSelected { get;  set; }

    public bool IsGetted { get;  set; }

    public float Power => power;
}