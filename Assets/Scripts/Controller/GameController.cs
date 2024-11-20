using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private CameraAnimation cameraAnimation;
    public void MoveToMap()
    {
        cameraAnimation.MoveToMap();
    }

    public void MoveToPlayer(int posNum)
    {
        cameraAnimation.MoveToPlayer(posNum);
    }

    public void MoveToMenu()
    {
        cameraAnimation.MoveToMenu();
    }
}