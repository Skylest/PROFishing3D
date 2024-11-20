using DG.Tweening;
using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    private readonly float sensitivity = 10f; // Чувствительность камеры
    private Vector2 lastTouchPosition; // Последняя позиция касания или мыши
    private bool isExistInitialPosition = false;

    public void SetStartPosition(Vector2 touchPosition)
    {
        lastTouchPosition = touchPosition;
        isExistInitialPosition = true;
    }

    public void Move(Vector2 currentTouchPosition)
    {
        if (!isExistInitialPosition)
            return;
#if UNITY_EDITOR || UNITY_EDITOR
        DesktopControl(currentTouchPosition);
#elif UNITY_ANDROID || UNITY_IOS
            MobileControl(currentTouchPosition);
#endif
    }

    public void LookAtPoint(Vector3 point)
    {
        transform.DOLookAt(point, GlobalData.animationBaseTime / 2).SetEase(Ease.InOutSine);
    }

    private void DesktopControl(Vector2 currentTouchPosition)
    {        
        Vector2 delta = currentTouchPosition - lastTouchPosition;
        lastTouchPosition = currentTouchPosition;

        float rotationX = -delta.y * sensitivity * Time.deltaTime;
        float rotationY = delta.x * sensitivity * Time.deltaTime;

        transform.Rotate(rotationX, rotationY, 0, Space.Self);

        // Ограничение вращения камеры по оси X
        Vector3 euler = transform.eulerAngles;
        euler.z = 0;
        transform.eulerAngles = euler;
    }

    private void MobileControl(Vector2 currentTouchPosition)
    {
        Vector2 delta = currentTouchPosition - lastTouchPosition;
        lastTouchPosition = currentTouchPosition;

        float rotationX = -delta.y * sensitivity * Time.deltaTime;
        float rotationY = delta.x * sensitivity * Time.deltaTime;

        transform.Rotate(rotationX, rotationY, 0, Space.Self);

        // Ограничение вращения камеры по оси X
        Vector3 euler = transform.eulerAngles;
        euler.z = 0;
        transform.eulerAngles = euler;
    }
}