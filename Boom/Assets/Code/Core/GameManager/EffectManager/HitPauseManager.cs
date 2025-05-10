using System.Collections;
using UnityEngine;

public static class HitPauseManager
{
    public static Coroutine PauseRoutine;

    public static void DoHitPause(float duration = 0.05f, float timeScale = 0.05f)
    {
        if (PauseRoutine != null)
            BattleManager.Instance.StopCoroutine(PauseRoutine);
        PauseRoutine = BattleManager.Instance.StartCoroutine(DoPause(duration, timeScale));
    }

    static IEnumerator DoPause(float duration, float timeScale)
    {
        Time.timeScale = timeScale;
        yield return new WaitForSecondsRealtime(duration); // 实时等待
        Time.timeScale = 1f;
    }
}