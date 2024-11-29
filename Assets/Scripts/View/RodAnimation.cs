using DG.Tweening;
using UnityEngine;

public class RodAnimation : MonoBehaviour
{
    [SerializeField] private Transform holder;

    private Tween rotationTween;

    public float HolderSpeed { get; set; } = 0f;
    private bool isHooking = true;
    
    void Start()
    {

    }

    public void SetRodRotation(Vector3 rotation)
    {
        rotationTween?.Kill();
        float duration = Vector3.Distance(holder.rotation.eulerAngles, rotation);
        rotationTween = transform.DORotate(rotation, GlobalData.animationFishOnHookTime * duration);
    }

    private void Update()
    {
        if (GlobalData.FishingState.Value == GlobalData.FishingStates.Hooking)
        {
            // Вычисляем изменение угла в зависимости от переменной increaseAngle
            float angleChange = HolderSpeed * Time.deltaTime * (isHooking ? 1f : -1f);

            // Изменяем угол вращения объекта по оси X
            holder.transform.rotation = Quaternion.Euler(
                holder.transform.rotation.eulerAngles.x + angleChange,
                holder.transform.rotation.eulerAngles.y,
                holder.transform.rotation.eulerAngles.z
            );
        }
    }
}
