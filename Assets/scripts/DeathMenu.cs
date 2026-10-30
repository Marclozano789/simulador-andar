using UnityEngine;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{

    public static DeathMenu Instance { get; private set; }

    [Header("Referencias")]
    public GameObject deathPanel;   // Panel con la imagen/texto de muerte
    public Button retryButton;
    public Button quitButton;

    void Awake() => Instance = this;

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Start()
    {
        deathPanel.SetActive(false);
        retryButton.onClick.AddListener(Gamemanager.RestartLevel);
        quitButton.onClick.AddListener(Gamemanager.QuitGame);
    }

    public void Show()
    {
        deathPanel.SetActive(true);

        // Congela el juego y libera el ratón para poder pulsar botones
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
