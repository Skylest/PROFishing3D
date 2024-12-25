using UnityEngine;
using static GlobalEnums;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private FishingView fishingView;    

    [SerializeField] private PlayerMover playerMover;

    private FishingModel fishingModel;

    private bool isDragging = false, isFishingUp = false;
    private Vector3 initialTouchPosition;

    /// <summary>
    /// Статус действия с объектом
    /// </summary>
    private enum ActionStatus
    {
        None,
        Down,
        Hold,
        Up
    }

    //TODO добавить таймер для срыва если не потянули когда клюнуло
    //TODO а что если спиздить механику с СиОфСифс и нужно вести влево вправо или по центру чтоб эффективнее тащить рыбу. И срыв делать если проебать этот момент и рыба съебется.
    //Более крупная рыба будет быстрее плыть и малечшая ошибка будет откидывать прогресс
    //Еще сделать хэппи аурс чтоб в начале только по центру было. Рандомное время
    private void Start()
    {
        fishingModel = new FishingModel();        
    }

    private void Update()
    {
        if (GlobalData.gameState.Value != GameStates.Gameplay)
            return;

        if (GetActionStatus(ActionStatus.Down))
            initialTouchPosition = GetActionPosition();

        switch (GlobalData.FishingState.Value)
        {
            case FishingStates.WaitThrowing:
                if (GetActionStatus(ActionStatus.Down))
                {
                    playerMover.SetStartPosition(initialTouchPosition);
                    isDragging = false;
                }

                if (GetActionStatus(ActionStatus.Hold))
                {
                    Vector3 currentPosition = GetActionPosition();

                    if (!isDragging && Vector3.Distance(initialTouchPosition, currentPosition) > 5f)
                        isDragging = true;

                    if (isDragging)
                        playerMover.Move((Vector2)currentPosition);
                }

                if (GetActionStatus(ActionStatus.Up))
                {
                    if (!isDragging)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(GetActionPosition());

                        if (Physics.Raycast(ray, out RaycastHit hit))
                            if (hit.collider.CompareTag("Water"))
                            {
                                FishWrapper fish = fishingModel.PrepareFish(out float time);
                                fishingView.ThrowHook(hit.point, fish, time);
                                playerMover.LookAtPoint(hit.point);
                            }
                    }
                    isDragging = false;
                }
                break;

            case FishingStates.FishOnHook:
                if (GetActionStatus(ActionStatus.Down))
                    fishingView.StartFishing();
                break;

            case FishingStates.Hooking:
                if (GetActionStatus(ActionStatus.Hold))
                {
                    Vector3 currentPosition = GetActionPosition();
                    float deltaX = currentPosition.x - initialTouchPosition.x;

                    int direction = 0;

                    if (Mathf.Abs(deltaX) >= 75f)                    
                        direction = (int)Mathf.Sign(deltaX);
                    
                    fishingView.Hook(direction);
                    isFishingUp = false;
                }

                if (isFishingUp)                
                    fishingView.LetGo();
                
                if (GetActionStatus(ActionStatus.Up))
                    isFishingUp = true;
                break;
        }
    }

    public void StopFishing()
    {
        fishingView.StopFishing();
    }

    public void PrepareLocationConfig(int locNum)
    {
        fishingModel.PrepareLocationConfig(locNum);
    }

    /// <summary>
    /// Проверка действия с экраном
    /// </summary>
    /// <param name="status">Статус действия с экраном</param>
    /// <returns>Произошло ли действие с экраном за статусом</returns>
    private bool GetActionStatus(ActionStatus status)
    {
#if UNITY_EDITOR || UNITY_EDITOR_WIN
        return status switch
        {
            ActionStatus.None => false,
            ActionStatus.Down => Input.GetKeyDown(KeyCode.Mouse0),
            ActionStatus.Hold => Input.GetKey(KeyCode.Mouse0),
            ActionStatus.Up => Input.GetKeyUp(KeyCode.Mouse0),
            _ => false
        };

#elif UNITY_ANDROID || UNITY_IOS
        return status switch
        {
            ActionStatus.None => false,
            ActionStatus.Down => Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began,
            ActionStatus.Hold => Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved,
            ActionStatus.Up => Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended,
            _ => false
        };
#endif

    }

    /// <summary>
    /// Получение координат тапа на экран
    /// </summary>
    /// <returns>Координаты тапа на экран</returns>
    private Vector3 GetActionPosition()
    {
#if UNITY_EDITOR || UNITY_EDITOR_WIN
        return Input.mousePosition;
#elif UNITY_ANDROID || UNITY_IOS
        return Input.GetTouch(0).position;
#endif
    }
}