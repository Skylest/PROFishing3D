using System.Collections;
using UnityEngine;

/// <summary>
/// Класс реализовывающий разблокировку максимального FPS
/// </summary>
public class FpsUnlocker : MonoBehaviour
{
    private float fps; // Переменная для хранения FPS

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(CalculateFps());
    }

    /// <summary>
    /// Рассчитывает FPS каждую секунду
    /// </summary>
    IEnumerator CalculateFps()
    {
        while (true)
        {
            fps = 1 / Time.deltaTime; // FPS рассчитывается как 1 / время кадра
            yield return new WaitForSeconds(1); // Обновляем каждую секунду
        }
    }

    /// <summary>
    /// Отображает FPS в левом верхнем углу экрана
    /// </summary>
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 150, 20), "FPS: " + Mathf.RoundToInt(fps)); // Отображаем FPS
    }
}
