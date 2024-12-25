using DG.Tweening;
using System.Collections;
using UnityEngine;
using static GlobalEnums;

public class HookAnimation : MonoBehaviour
{
    private Sequence sequenceMove, sequenceRotate;
    private Tween cameraTween;
    private const float DefoltYPos = 19.07f, DefoltRotationBounds = 9f, AngleToCamera = -30f;

    private void Start()
    {
        GlobalData.FishingState.OnValueChanged += ChangeAnim;
        GlobalData.FishingState.OnValueChanged += () => { StopCoroutine(LookAtCamera()); };
    }

    public void SetPosition(Vector3 hookPos)
    {
        hookPos.y = DefoltYPos;
        transform.SetPositionAndRotation(hookPos, Quaternion.Euler(-DefoltRotationBounds, 0, 0));
    }

    private void ChangeAnim()
    {
        sequenceMove?.Kill();
        sequenceRotate?.Kill();

        sequenceMove = DOTween.Sequence();
        sequenceRotate = DOTween.Sequence();

        sequenceMove.SetEase(Ease.InOutSine);
        sequenceRotate.SetEase(Ease.InOutSine);

        switch (GlobalData.FishingState.Value)
        {
            case FishingStates.WaitingFish:

                sequenceRotate.Append(transform.DORotate(new Vector3(DefoltRotationBounds, 0, 0), GlobalData.animationFishOnHookTime * 5))
                              .SetLoops(-1, LoopType.Yoyo);

                sequenceMove.Append(transform.DOMoveY(DefoltYPos - 0.025f, GlobalData.animationFishOnHookTime))
                            .Append(transform.DOMoveY(DefoltYPos + 0.015f, GlobalData.animationFishOnHookTime))
                            .Append(transform.DOMoveY(DefoltYPos - 0.01f, GlobalData.animationFishOnHookTime * 2))
                            .Append(transform.DOMoveY(DefoltYPos, GlobalData.animationFishOnHookTime * 4))
                            .AppendInterval(0.5f)
                            .SetLoops(-1, LoopType.Restart);

                break;

            case FishingStates.FishOnHook:
                sequenceRotate.Append(transform.DORotate(Vector3.zero, GlobalData.animationFishOnHookTime))
                              .Join(transform.DOMoveY(DefoltYPos, GlobalData.animationFishOnHookTime));

                sequenceMove.Append(transform.DOMoveY(DefoltYPos - 0.14f, GlobalData.animationFishOnHookTime))
                        .Append(transform.DOMoveY(DefoltYPos + 0.06f, GlobalData.animationFishOnHookTime / 2))
                        .Append(transform.DOMoveY(DefoltYPos - 0.04f, GlobalData.animationFishOnHookTime))
                        .Append(transform.DOMoveY(DefoltYPos, GlobalData.animationFishOnHookTime * 2))
                        .AppendInterval(0.5f)
                        .SetLoops(-1, LoopType.Restart)
                        .SetDelay(GlobalData.animationFishOnHookTime);
                break;

            case FishingStates.Hooking:
                transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);

                sequenceRotate.Append(transform.DORotateQuaternion(GetRotationWithTilt(), GlobalData.animationFishOnHookTime / 2))
                              .Join(transform.DOMoveY(DefoltYPos, GlobalData.animationFishOnHookTime / 2))
                              .OnComplete(() =>
                              {
                                  StartCoroutine(LookAtCamera());
                              });
                break;
        }
    }

    private Quaternion GetRotationWithTilt()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        targetRotation *= Quaternion.Euler(AngleToCamera, 0f, 0f);
        return targetRotation;
    }

    private IEnumerator LookAtCamera()
    {
        while (GlobalData.FishingState.Value == FishingStates.Hooking)
        {
            cameraTween?.Kill();
            transform.rotation = GetRotationWithTilt();
            cameraTween = Camera.main.transform.DOLookAt(transform.position, 0.5f);
            yield return null;
        }
    }
}