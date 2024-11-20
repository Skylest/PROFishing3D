using System.Collections;
using UnityEngine;

public class FishingView : MonoBehaviour
{
    [SerializeField] RodAnimation rod;

    [SerializeField] HookAnimation hook;    

    //TODO ����� ����� �� ������� ����� ��� ������. ������� ����� ������� ��������. ���������. �������. ������. �������� �����. ��������� �� ���� ���������.
    private void Start()
    {
        GlobalData.gameState.OnValueChanged += StopFishing;
    }

    public void Hook(Vector3 escapeVector, float fishSpeed)
    {
        Vector3 target = transform.position;
        target.y = hook.transform.position.y;
        hook.transform.position = Vector3.MoveTowards(hook.transform.position, target, fishSpeed * Time.deltaTime);
    }

    public void Hold(Vector3 escapeVector, float fishSpeed)
    {
        escapeVector.y = hook.transform.position.y;
        hook.transform.position = Vector3.MoveTowards(hook.transform.position, escapeVector, fishSpeed * Time.deltaTime);
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
