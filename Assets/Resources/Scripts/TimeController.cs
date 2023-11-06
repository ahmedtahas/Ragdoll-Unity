using System.Collections;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public void SlowDownTime(float scale, float duration)
    {
        StartCoroutine(SlowDown(scale, duration));
    }

    private IEnumerator SlowDown(float scale, float duration)
    {
        Time.timeScale = scale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}