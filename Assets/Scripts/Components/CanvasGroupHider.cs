using UnityEngine;

public class CanvasGroupHider : MonoBehaviour
{
    private void Awake()
    {
        if (TryGetComponent(out CanvasGroup canvasGroup))
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
