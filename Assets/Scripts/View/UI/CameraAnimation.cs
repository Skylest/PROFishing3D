using DG.Tweening;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    [Header("Canvases")]
    [SerializeField] private CanvasGroup menuCanvas;
    [SerializeField] private CanvasGroup mapCanvas;
    [SerializeField] private CanvasGroup gameplayCanvas;

    [Header("Cameras")]
    [SerializeField] private Camera gameplayCamera;
    [SerializeField] private Camera mapCamera;

    [Header("Position Points")]
    [SerializeField] private Transform[] locationPostiton;
    [SerializeField] private Transform orbitPostiton;
    [SerializeField] private Transform menuPostiton;

    //TODO доработать аниматор. Прозрачной панелькой блокировать клики. И чекнуть возможность запускать по очереди через Sequence.
    private void Awake()
    {
        gameplayCamera.enabled = false;
        mapCamera.enabled = true;
        mapCamera.transform.SetPositionAndRotation(menuPostiton.position, menuPostiton.rotation);
    }

    /// <summary>
    /// Moving main camera to orbit position and changing UI
    /// </summary>
    public void MoveToMap()
    {
        gameplayCamera.enabled = false;
        mapCamera.enabled = true;
        GlobalData.gameState.Value = GlobalData.GameStates.Map;

        CanvasAnimate(menuCanvas, false);
        CanvasAnimate(gameplayCanvas, false);
        CanvasAnimate(mapCanvas, true);

        mapCamera.transform
            .DORotate(orbitPostiton.rotation.eulerAngles, GlobalData.animationBaseTime)
            .SetEase(Ease.InOutSine);

        mapCamera.transform
            .DOMove(orbitPostiton.position, GlobalData.animationBaseTime)
            .SetEase(Ease.InOutSine);
    }

    /// <summary>
    /// Moving main camera to player position and changing UI
    /// </summary>
    /// <param name="posNum">Number of position</param>
    public void MoveToPlayer(int posNum)
    {
        gameplayCamera.transform.SetPositionAndRotation(locationPostiton[posNum].position, locationPostiton[posNum].rotation);

        CanvasAnimate(gameplayCanvas, true);
        CanvasAnimate(mapCanvas, false);
        
        Sequence sequence = DOTween.Sequence()
            .Append(mapCamera.transform.DORotate(locationPostiton[posNum].rotation.eulerAngles, GlobalData.animationBaseTime))
            .Join(mapCamera.transform.DOMove(locationPostiton[posNum].position, GlobalData.animationBaseTime))
            .SetEase(Ease.InOutSine).OnComplete(() =>
            {
                gameplayCamera.enabled = true;
                mapCamera.enabled = false;
                GlobalData.gameState.Value = GlobalData.GameStates.Gameplay;
            });
    }

    /// <summary>
    /// Moving main camera to menu position and changing UI
    /// </summary>
    public void MoveToMenu()
    {
        GlobalData.gameState.Value = GlobalData.GameStates.Menu;

        CanvasAnimate(menuCanvas, true);
        CanvasAnimate(mapCanvas, false);
        
        Sequence sequence = DOTween.Sequence()
            .Append(mapCamera.transform.DORotate(menuPostiton.rotation.eulerAngles, GlobalData.animationBaseTime))
            .Join(mapCamera.transform.DOMove(menuPostiton.position, GlobalData.animationBaseTime))
            .SetEase(Ease.InOutSine);
    }

    /// <summary>
    /// Animate canvas appear or disappear
    /// </summary>
    /// <param name="switchTo">Change canvas params to this variable</param>
    private void CanvasAnimate(CanvasGroup canvasGroup, bool switchTo)
    {
        float alpha = switchTo ? 1f : 0f;
        float duration = switchTo ? GlobalData.animationBaseTime : GlobalData.animationBaseTime / 2;
       
        canvasGroup.DOFade(alpha, duration)
            .SetEase(Ease.InOutSine)
            .OnStart(() =>
            {
                if (switchTo)
                {
                    canvasGroup.interactable = switchTo;
                    canvasGroup.blocksRaycasts = switchTo;
                }
            })
            .OnComplete(() =>
            {
                if (!switchTo)
                {
                    canvasGroup.interactable = switchTo;
                    canvasGroup.blocksRaycasts = switchTo;
                }
            });             
    }
}
