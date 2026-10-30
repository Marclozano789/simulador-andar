using System;
using System.Text;
using UnityEngine;
using TMPro;

// Pulsa la tecla indicada repetidamente antes de que se acabe el tiempo.
public class Quicktimeevent : MonoBehaviour
{

    [Header("Referencias")]
    public PlayerController playerController;
    public TMP_Text promptText;

    [Header("Secuencia")]
    public int sequenceLength = 5;
    public float timeLimit = 6f;
    public float wrongKeyPenalty = 0.75f;   // segundos que pierdes al fallar una tecla
    public bool resetOnWrongKey = true;     // si falla, vuelve al principio del patrón
    public bool alsoAcceptTopRowDigits = true; // para teclados sin numpad (acepta 0-9 de la fila superior)

    [Header("Colores")]
    public Color doneColor = new Color(0.3f, 1f, 0.3f);
    public Color currentColor = Color.yellow;
    public Color pendingColor = new Color(0.6f, 0.6f, 0.6f);

    static readonly KeyCode[] numpadKeys =
    {
        KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4,
        KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9
    };

    static readonly KeyCode[] topRowKeys =
    {
        KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4,
        KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9
    };

    bool active;
    float timer;
    int index;
    int[] sequence;          // dígitos 0-9
    Action<bool> callback;

    void Start()
    {
        if (promptText != null) promptText.gameObject.SetActive(false);
    }

    public void Begin(Action<bool> onFinished)
    {
        
        callback = onFinished;
        GenerateSequence();
        index = 0;
        timer = timeLimit;
        active = true;

        playerController.canMove = false;

        if (promptText != null) promptText.gameObject.SetActive(true);
        RefreshText();
    }

    // Semi-aleatorio: nunca repite el mismo dígito dos veces seguidas
    void GenerateSequence()
    {
        sequence = new int[sequenceLength];
        int previous = -1;
        for (int i = 0; i < sequenceLength; i++)
        {
            int n;
            do { n = UnityEngine.Random.Range(0, 10); } while (n == previous);
            sequence[i] = n;
            previous = n;
        }
    }

    void Update()
    {
        if (!active) return;

        timer -= Time.deltaTime;

        int pressed = GetPressedDigit();
        if (pressed != -1)
        {
            if (pressed == sequence[index])
            {
                index++;
                if (index >= sequence.Length) { Finish(true); return; }
            }
            else
            {
                timer -= wrongKeyPenalty;
                if (resetOnWrongKey) index = 0;
            }
        }

        if (timer <= 0f) { Finish(false); return; }

        RefreshText();
    }

    // Devuelve el dígito pulsado este frame, o -1 si no se pulsó ninguno
    int GetPressedDigit()
    {
        for (int d = 0; d < 10; d++)
        {
            if (Input.GetKeyDown(numpadKeys[d])) return d;
            if (alsoAcceptTopRowDigits && Input.GetKeyDown(topRowKeys[d])) return d;
        }
        return -1;
    }

    void RefreshText()
    {
        if (promptText == null) return;

        var sb = new StringBuilder();
        sb.AppendLine("¡REPITE EL PATRÓN!");
        sb.AppendLine();

        for (int i = 0; i < sequence.Length; i++)
        {
            Color c = i < index ? doneColor : (i == index ? currentColor : pendingColor);
            sb.Append($"<color=#{ColorUtility.ToHtmlStringRGB(c)}>{sequence[i]}</color>  ");
        }

        sb.AppendLine();
        sb.AppendLine();
        sb.Append($"<size=60%>{Mathf.Max(0f, timer):0.0}s</size>");

        promptText.text = sb.ToString();
    }

    void Finish(bool success)
    {
       

        active = false;

        if (promptText != null) promptText.gameObject.SetActive(false);
        playerController.canMove = true;

        callback?.Invoke(success);
    }
}
