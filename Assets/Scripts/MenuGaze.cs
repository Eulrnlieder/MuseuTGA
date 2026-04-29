using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGaze : MonoBehaviour
{
    [Header("Configurações")]
    public string sceneName;
    public float gazeTime = 1f;

    [Header("Sons")]
    public AudioClip soundEnter;
    public AudioClip soundExit;
    public AudioClip soundLoad;

    private Coroutine gazeCoroutine;
    private Vector3 originalScale;
    private AudioSource audioSource;

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Waypoint");

        originalScale = transform.localScale;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnPointerEnter()
    {
        if (gazeCoroutine != null)
            StopCoroutine(gazeCoroutine);

        PlaySound(soundEnter);
        gazeCoroutine = StartCoroutine(GazeTimer());
    }

    void OnPointerExit()
    {
        PlaySound(soundExit);
        ResetButton();
    }

    private IEnumerator GazeTimer()
    {
        float timer = 0f;

        while (timer < gazeTime)
        {
            timer += Time.deltaTime;

            float progress = timer / gazeTime;
            float scale = 1f + (progress * 0.5f);

            transform.localScale = new Vector3(
                originalScale.x,
                originalScale.y * scale,
                originalScale.z
            );

            yield return null;
        }

        PlaySound(soundLoad);
        yield return new WaitForSeconds(soundLoad != null ? soundLoad.length : 0f);

        SceneManager.LoadScene(sceneName);
    }

    private void ResetButton()
    {
        transform.localScale = originalScale;

        if (gazeCoroutine != null)
        {
            StopCoroutine(gazeCoroutine);
            gazeCoroutine = null;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
            audioSource.PlayOneShot(clip);
    }
}