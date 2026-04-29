using System.Collections;
using UnityEngine;

public class InteractionGaze : MonoBehaviour
{
    [Header("Referências")]
    public GameObject infoPanel;

    [Header("Configurações")]
    public float gazeTime = 1f;
    public float fadeSpeed = 1f;

    [Header("Som")]
    public AudioClip music;

    private Coroutine gazeCoroutine;
    private CanvasGroup canvasGroup;
    private AudioSource audioSource;

    void Start()
    {
        gameObject.layer = LayerMask.NameToLayer("Waypoint");

        canvasGroup = infoPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = infoPanel.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        infoPanel.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = music;
        audioSource.loop = true;
    }

    void OnPointerEnter()
    {
        if (gazeCoroutine != null)
            StopCoroutine(gazeCoroutine);

        gazeCoroutine = StartCoroutine(ShowPanel());
    }

    void OnPointerExit()
    {
        if (gazeCoroutine != null)
        {
            StopCoroutine(gazeCoroutine);
            gazeCoroutine = null;
        }

        audioSource.Stop();
        gazeCoroutine = StartCoroutine(HidePanel());
    }

    private IEnumerator ShowPanel()
    {
        yield return new WaitForSeconds(gazeTime);

        audioSource.Play();
        infoPanel.SetActive(true);

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private IEnumerator HidePanel()
    {
        while (canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        infoPanel.SetActive(false);
    }
}