using System.Collections;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    float currentDuration = 0.0f;
    private Coroutine slowDownCoroutine;
    private float remainingSlowDownTime = 0f;
    private float pauseTimestamp = 0f;
    private bool isPaused = false;
    private float pausedScale = 1f;

    public void SlowDownTime(float scale = 0.01f, float duration = 0.75f)
    {
        if (duration >= currentDuration)
        {
            Debug.Log("Slow down time by " + scale + " duration " + duration);
            currentDuration = duration;
            if (slowDownCoroutine != null)
            {
                Debug.Log("Stopping previous coroutine");
                StopCoroutine(slowDownCoroutine);
            }
            remainingSlowDownTime = duration;
            slowDownCoroutine = StartCoroutine(SlowDown(scale, duration));
        }
    }

    private IEnumerator SlowDown(float scale, float duration)
    {
        Debug.Log("Coroutine started");
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        float startTime = Time.realtimeSinceStartup;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (isPaused)
            {
                pauseTimestamp = Time.realtimeSinceStartup;
                yield break; // Exit coroutine on pause
            }
            yield return null;
            elapsed = Time.realtimeSinceStartup - startTime;
            remainingSlowDownTime = duration - elapsed;
        }

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        currentDuration = 0.0f;
        slowDownCoroutine = null;
    }

    public void PauseSlowDown()
    {
        if (slowDownCoroutine != null && !isPaused)
        {
            isPaused = true;
            pausedScale = Time.timeScale;
            Time.timeScale = 0f;
            StopCoroutine(slowDownCoroutine);
            Debug.Log("Slowdown paused");
        }
    }

    public void ResumeSlowDown()
    {
        if (isPaused && remainingSlowDownTime > 0f)
        {
            isPaused = false;
            Time.timeScale = pausedScale;
            slowDownCoroutine = StartCoroutine(SlowDown(pausedScale, remainingSlowDownTime));
            Debug.Log("Slowdown resumed");
        }
    }
}