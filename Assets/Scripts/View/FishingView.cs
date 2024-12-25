using System.Collections;
using UnityEngine;
using static GlobalEnums;

public class FishingView : MonoBehaviour
{
    [SerializeField] private RodAnimation rod;

    private FishWrapper currentFish;

    //TODO а шо если слишком близко??? Имба получается. А вдали нереально
    private const float DistanceFromPlayer = 100f;
    private const float EscapeDistanceFromPlayer = 110f;
    private const float SectorAngle = 80f;
    private Vector3 escapeVector;

    public Vector3 GetEscapePoint(Transform player)
    {
        Vector3 forward = player.forward;

        float randomAngle = Random.Range(-SectorAngle / 2, SectorAngle / 2);

        Vector3 escapeDirection = Quaternion.Euler(0, randomAngle, 0) * forward;

        Vector3 escapePoint = player.position + escapeDirection.normalized * DistanceFromPlayer;

        return escapePoint;
    }

    //TODO Частота смены стороны смещения. Поражение. Рестарт. Победа. Детекция земли. Разрешено ли сюда забросить. Разрешено ли туда плыть (я думаю проверять попадает ли луч
    // от игрока до поплавка или стыкается еще с чем-то)
    private void Start()
    {
        GlobalData.gameState.OnValueChanged += StopFishing;
    }

    public void Hook(int rodDirection)
    {
        float speed = 10f;
        rod.SetHookingRotation(speed, rodDirection);
       // Vector3 target = transform.position;
       // target.y = hook.transform.position.y;
       // hook.transform.position = Vector3.MoveTowards(hook.transform.position, target, fishSpeed * Time.deltaTime);
    }

    public void LetGo()
    {
        float speed = -15f;
        rod.SetDefoltRotation(speed);
        //escapeVector.y = hook.transform.position.y;
        //hook.transform.position = Vector3.MoveTowards(hook.transform.position, escapeVector, fishSpeed * Time.deltaTime);
    }    

    public void ThrowHook(Vector3 hookPos, FishWrapper fish, float waitTime)
    {
        GlobalData.FishingState.Value = FishingStates.WaitingFish;
        currentFish = fish;
        rod.ThrowAnimation(hookPos);
        StartCoroutine(WaitingFish(waitTime));
    }

    public void StartFishing()
    {
        GlobalData.FishingState.Value = FishingStates.Hooking;
    }

    public void StopFishing()
    {
        rod.SetDefoltRotation();
        GlobalData.FishingState.Value = FishingStates.WaitThrowing;
    }

    private IEnumerator WaitingFish(float waitingTime)
    {
        yield return new WaitForSecondsRealtime(waitingTime);
        GlobalData.FishingState.Value = FishingStates.FishOnHook;
    }

    private IEnumerator ChangeEscapeVector()
    {
       // fishingModel.ChangeEscapeVector();
        float waitTime = Random.Range(1f, 10f);
        yield return new WaitForSecondsRealtime(waitTime);
    }

    private void CheckCoroutine()
    {
        if (GlobalData.FishingState.Value == FishingStates.Hooking)
            StartCoroutine(ChangeEscapeVector());
        else
            StopCoroutine(ChangeEscapeVector());
    }
}
