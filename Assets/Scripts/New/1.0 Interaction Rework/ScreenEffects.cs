using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class ScreenEffects : MonoBehaviour
{
    public static ScreenEffects Instance;

    [Header("Images")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private Image deathFadeImage;

    [Header("Danger")]
    [SerializeField] private RectTransform dangerTransform;
    [SerializeField] private float maxDangerScale = 1.4f;
    [SerializeField] private float minDangerScale = 1.0f;

    [Header("Danger Shake")]
    [SerializeField] private float shakeAmount = 8f;
    [SerializeField] private float shakeSpeed = 2f;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string volumeParameter = "MasterVolume";

    [SerializeField]
    [Range(-80f, 0f)]
    private float deathVolume = -20f;

    private Coroutine fadeCoroutine;

    private Vector2 dangerOriginalPosition;
    private float currentProximity;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        dangerOriginalPosition = dangerTransform.anchoredPosition;

        // Ensure the death fade starts transparent and disabled.
        Color deathColor = deathFadeImage.color;
        deathColor.a = 0f;
        deathFadeImage.color = deathColor;
        deathFadeImage.enabled = false;

        // Ensure audio starts at full volume.
        audioMixer.SetFloat(volumeParameter, 0f);

        FadeIn(1f, 3f);
    }

    private void Update()
    {
        UpdateDangerShake();
    }

    #region Fade

    public void FadeIn(float duration)
    {
        FadeIn(0f, duration);
    }

    public void FadeIn(float delay, float duration)
    {
        fadeImage.enabled = true;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeRoutine(1f, 0f, delay, duration));
    }

    public void FadeOut(float duration)
    {
        fadeImage.enabled = true;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeRoutine(0f, 1f, 0f, duration));
    }

    private IEnumerator FadeRoutine(float startAlpha, float endAlpha, float delay, float duration)
    {
        Color color = fadeImage.color;
        color.a = startAlpha;
        fadeImage.color = color;

        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            color.a = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            fadeImage.color = color;

            yield return null;
        }

        color.a = endAlpha;
        fadeImage.color = color;

        if (endAlpha == 0f)
            fadeImage.enabled = false;

        fadeCoroutine = null;
    }

    #endregion

    #region Danger

    /// <summary>
    /// Moves the vignette onto the screen based on proximity.
    /// 0 = far away
    /// 1 = very close
    /// </summary>
    public void SetDangerProximity(float proximity)
    {
        currentProximity = Mathf.Clamp01(proximity);

        float scale = Mathf.Lerp(maxDangerScale, minDangerScale, currentProximity);
        dangerTransform.localScale = Vector3.one * scale;
    }

    public void SetDangerProximityByDistance(float distance, float maxDistance)
    {
        float proximity = 1f - Mathf.Clamp01(distance / maxDistance);
        SetDangerProximity(proximity);
    }

    private void UpdateDangerShake()
    {
        if (currentProximity <= 0f)
        {
            dangerTransform.anchoredPosition = dangerOriginalPosition;
            return;
        }

        float intensity = currentProximity * currentProximity * shakeAmount;

        float x = (Mathf.PerlinNoise(Time.time * shakeSpeed, 0f) - 0.5f) * 2f * intensity;
        float y = (Mathf.PerlinNoise(0f, Time.time * shakeSpeed) - 0.5f) * 2f * intensity;

        dangerTransform.anchoredPosition = dangerOriginalPosition + new Vector2(x, y);
    }

    /// <summary>
    /// Controls the enemy death fade and audio.
    /// </summary>
    public void SetDeathFade(float progress)
    {
        progress = Mathf.Clamp01(progress);

        if (progress > 0f)
            deathFadeImage.enabled = true;

        Color color = deathFadeImage.color;
        color.a = progress;
        deathFadeImage.color = color;

        if (progress <= 0f)
            deathFadeImage.enabled = false;

        // Fade the game's audio.
        float volume = Mathf.Lerp(0f, deathVolume, progress);
        audioMixer.SetFloat(volumeParameter, volume);
    }

    #endregion
}