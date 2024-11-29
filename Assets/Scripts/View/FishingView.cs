using System.Collections;
using UnityEngine;

public class FishingView : MonoBehaviour
{
    [SerializeField] private RodAnimation rod;

    [SerializeField] private HookAnimation hook;    

    //TODO Когдя тянем то смещаем влево или вправо. Частота смены стороны смещения. Поражение. Рестарт. Победа. Детекция земли. Разрешено ли сюда забросить.
    private void Start()
    {
        GlobalData.gameState.OnValueChanged += StopFishing;
    }

    public void Hook(Vector3 fishNewPosition, float speed, int rodDirection)
    {
        rod.HolderSpeed = speed;
       // Vector3 target = transform.position;
       // target.y = hook.transform.position.y;
       // hook.transform.position = Vector3.MoveTowards(hook.transform.position, target, fishSpeed * Time.deltaTime);
    }

    public void LetGo(Vector3 fishNewPosition, float speed)
    {
        rod.HolderSpeed = speed;
        //escapeVector.y = hook.transform.position.y;
        //hook.transform.position = Vector3.MoveTowards(hook.transform.position, escapeVector, fishSpeed * Time.deltaTime);
    }    

    public void ThrowHook(Vector3 hookPos, float waitTime)
    {
        GlobalData.FishingState.Value = GlobalData.FishingStates.WaitingFish;
        hook.SetPosition(hookPos);
        
        StartCoroutine(WaitingFish(waitTime));
    }

    public void StartFishing()
    {
        GlobalData.FishingState.Value = GlobalData.FishingStates.Hooking;
    }

    public void StopFishing()
    {
        hook.SetPosition(new Vector3(0, -100, 0));
        GlobalData.FishingState.Value = GlobalData.FishingStates.WaitThrowing;
    }

    private IEnumerator WaitingFish(float waitingTime)
    {
        yield return new WaitForSecondsRealtime(waitingTime);
        GlobalData.FishingState.Value = GlobalData.FishingStates.FishOnHook;
    }
}
