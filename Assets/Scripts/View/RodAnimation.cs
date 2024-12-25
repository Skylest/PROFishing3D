using DG.Tweening;
using UnityEngine;
using static GlobalEnums;

public class RodAnimation : MonoBehaviour
{
    [SerializeField] private Transform holder;
    [SerializeField] private HookAnimation hook;

    private Tween rotationTween;
    private Sequence throwTween;

    private readonly Vector3 defoltRotation = new(20f, 0f, 0f);
    private readonly Vector3 hookingRotation = new(7f, 15f, 25f);
    private float defoltDistance;
    private float holderSpeed = 0f;
    private int currentDirection = -2;

    void Start()
    {
        defoltDistance = Distance(new Vector3(hookingRotation.x, 0f, 0f), hookingRotation);
    }

    public void ThrowAnimation(Vector3 hookPos)
    {
        throwTween?.Kill();
        throwTween.SetEase(Ease.InOutSine);
        throwTween = DOTween.Sequence()
            .Append(transform.DOLocalRotate(new Vector3(0f, 0f, 0f), GlobalData.animationRodTime))
            .Append(transform.DOLocalRotate(new Vector3(50f, 0f, 0f), GlobalData.animationRodTime))
            .Append(transform.DOLocalRotate(defoltRotation, GlobalData.animationRodTime))
            .OnComplete(() => hook.SetPosition(hookPos));
    }

    public void SetDefoltRotation()
    {
        float distance = Distance(transform.localEulerAngles, defoltRotation) / defoltDistance;
        hook.SetPosition(new Vector3(0, -100, 0));

        rotationTween?.Kill();
        rotationTween = transform.DOLocalRotate(defoltRotation, GlobalData.animationRodTime * distance ).SetEase(Ease.InOutSine);
        currentDirection = -2;
    }

    public void SetDefoltRotation(float fishSpeed)
    {
        holderSpeed = fishSpeed;
        float distance = Distance(transform.localEulerAngles, defoltRotation) / defoltDistance;

        rotationTween?.Kill();
        rotationTween = transform.DOLocalRotate(defoltRotation, GlobalData.animationRodTime * distance /* / Mathf.Sqrt(Mathf.Abs(fishSpeed))*/).SetEase(Ease.InOutSine);
        currentDirection = -2;
    }

    public void SetHookingRotation(float fishSpeed, int direction)
    {
        holderSpeed = fishSpeed;

        if (currentDirection == direction)
            return;

        currentDirection = direction;

        Vector3 rotation = hookingRotation;
        rotation.y *= -direction;
        rotation.z *= -direction;

        float distance = Distance(transform.localEulerAngles, rotation) / defoltDistance;

        rotationTween?.Kill();
        rotationTween = transform.DOLocalRotate(rotation, GlobalData.animationRodTime * distance /* * Mathf.Sqrt(Mathf.Abs(fishSpeed))*/).SetEase(Ease.InOutSine);
    }

    private float Distance(Vector3 a, Vector3 b)
    {
        Vector3 normalizedCurrentRotation = NormalizeEuler(a);
        Vector3 normalizedTargetRotation = NormalizeEuler(b);
        return Vector3.Distance(normalizedCurrentRotation, normalizedTargetRotation);
    }

    private Vector3 NormalizeEuler(Vector3 eulerAngles)
    {
        return new Vector3(
            Mathf.DeltaAngle(0, eulerAngles.x),
            Mathf.DeltaAngle(0, eulerAngles.y),
            Mathf.DeltaAngle(0, eulerAngles.z)
        );
    }

    private void Update()
    {

        if (GlobalData.FishingState.Value == FishingStates.Hooking)
        {
            float angleChange = holderSpeed * Time.deltaTime;

            holder.transform.Rotate(new Vector3(-angleChange, 0, 0));
        }
    }
}
