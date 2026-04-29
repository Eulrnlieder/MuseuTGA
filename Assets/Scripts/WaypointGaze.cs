using System.Collections;
using UnityEngine;

public class WaypointGaze : MonoBehaviour
{
    [Header("Referências")]
    public Transform player;

    [Header("Configurações de gaze")]
    public float gazeTime = 0.75f;

    [Header("Sons")]
    public AudioClip soundEnter;
    public AudioClip soundTeleport;

    [Header("Cores")]
    public Color normalColor = Color.gray;
    public Color gazingColor = Color.blue;

    private float gazeTimer = 0f;
    private Renderer rend;
    private Coroutine gazeCoroutine;
    private Vector3 originalScale;
    private AudioSource audioSource;


    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.color = normalColor;

        gameObject.layer = LayerMask.NameToLayer("Waypoint");
        originalScale = transform.localScale;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnPointerEnter()
    {
        rend.material.color = gazingColor;

        if (gazeCoroutine != null)
            StopCoroutine(gazeCoroutine);

        PlaySound(soundEnter);
        gazeCoroutine = StartCoroutine(GazeTimer());
    }

    void OnPointerExit()
    {
        ResetWaypoint();
    }

    private IEnumerator GazeTimer()
    {
        gazeTimer = 0f;

        while (gazeTimer < gazeTime)
        {
            gazeTimer += Time.deltaTime;

            float progress = gazeTimer / gazeTime;
            float scale = 1f + (progress * 0.5f);

            transform.localScale = new Vector3(
                originalScale.x * scale,
                originalScale.y,
                originalScale.z * scale
            );

            yield return null;
        }

        PlaySound(soundTeleport);
        Teleport();
    }

    private void Teleport()
    {
        if (player != null)
        {
            player.position = new Vector3(
                transform.position.x,
                player.position.y,
                transform.position.z
            );
        }

        ResetWaypoint();
    }

    private void ResetWaypoint()
    {
        rend.material.color = normalColor;
        transform.localScale = originalScale;
        gazeTimer = 0f;

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