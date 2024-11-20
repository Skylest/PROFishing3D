using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DOTweenQueue : MonoBehaviour
{
    private Queue<Action> animationQueue = new Queue<Action>();
    private bool isAnimating = false;

    public Action OnQueueEmpty;    // Действие при пустой очереди
    public Action OnAddToQueue;    // Действие при добавлении в очередь

    // Метод для добавления анимации в очередь
    public void EnqueueAnimation(Action animationAction)
    {
        animationQueue.Enqueue(animationAction);

        // Выполняем действие при добавлении в очередь
        OnAddToQueue?.Invoke();

        // Если ничего не проигрывается, запускаем очередь
        if (!isAnimating)
        {
            StartCoroutine(ProcessQueue());
        }
    }

    // Метод для обработки очереди
    private IEnumerator ProcessQueue()
    {
        isAnimating = true;

        while (animationQueue.Count > 0)
        {
            // Берем следующую анимацию и выполняем её
            Action animationAction = animationQueue.Dequeue();
            animationAction.Invoke();

            // Ждём, пока текущая анимация завершится
            yield return new WaitWhile(() => DOTween.TotalPlayingTweens() > 0);
        }

        isAnimating = false;

        // Действие, когда очередь пустая
        OnQueueEmpty?.Invoke();
    }

    // Пример метода для добавления анимации CanvasGroup в очередь
    public void AddCanvasAnimation(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        EnqueueAnimation(() =>
        {
            canvasGroup.DOFade(targetAlpha, duration)
                .SetEase(Ease.InOutSine);
        });
    }
}
