using System;
using UnityEngine;


// Pulsa la tecla indicada repetidamente antes de que se acabe el tiempo.
public class Quicktimeevent : MonoBehaviour
{
    public KeyCode key = KeyCode.E;
    public int pressesNeeded = 12;
    public float timeLimit = 4f;
    public float decayPerSecond = 1.5f;   // el progreso baja si dejas de pulsar

    bool active;
    float progress, timer;
    Action<bool> callback;

    public void Begin(Action<bool> onFinished)
    {
        callback = onFinished;
        progress = 0f;
        timer = timeLimit;
        active = true;
        // TODO: muestra aquí tu UI ("¡Pulsa E!") y desactiva el movimiento del jugador
    }

    void Update()
    {
        if (!active) return;

        timer -= Time.deltaTime;
        progress = Mathf.Max(0f, progress - decayPerSecond * Time.deltaTime);

        if (Input.GetKeyDown(key)) progress += 1f;

        // TODO: actualiza tu barra de progreso con progress / pressesNeeded

        if (progress >= pressesNeeded) Finish(true);
        else if (timer <= 0f) Finish(false);
    }

    void Finish(bool success)
    {
        active = false;
        // TODO: oculta la UI y devuelve el control al jugador
        callback?.Invoke(success);
    }
}
