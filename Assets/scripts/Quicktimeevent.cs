using System;
using System.Text;
using UnityEngine;
using TMPro;

// Pulsa la tecla indicada repetidamente antes de que se acabe el tiempo.
public class Quicktimeevent : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerController playerController;
    public TMP_Text promptText;           // Texto en pantalla

    [Header("Ajustes")]
    public KeyCode key = KeyCode.E;
    public int pressesNeeded = 12;
    public float timeLimit = 4f;
    public float decayPerSecond = 1.5f;   // el progreso baja si dejas de pulsar

    bool active;
    float progress, timer;
    Action<bool> callback;

    void Start()
    {
        if (promptText != null) promptText.gameObject.SetActive(false);
    }

    public void Begin(Action<bool> onFinished)
    {
        callback = onFinished;
        progress = 0f;
        timer = timeLimit;
        active = true;

        playerController.canMove = false;

        if (promptText != null)
        {
            promptText.text = $"¡PULSA [{key}] REPETIDAMENTE!";
            promptText.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (!active) return;

        timer -= Time.deltaTime;
        progress = Mathf.Max(0f, progress - decayPerSecond * Time.deltaTime);

        if (Input.GetKeyDown(key)) progress += 1f;

        // Efecto simple: el texto "late" para dar sensación de urgencia
        if (promptText != null)
        {
            float scale = 1f + Mathf.Sin(Time.time * 20f) * 0.05f;
            promptText.transform.localScale = Vector3.one * scale;
        }

        if (progress >= pressesNeeded) Finish(true);
        else if (timer <= 0f) Finish(false);
    }

    void Finish(bool success)
    {
        active = false;

        if (promptText != null) promptText.gameObject.SetActive(false);
        playerController.canMove = true;

        callback?.Invoke(success);
    }
}
