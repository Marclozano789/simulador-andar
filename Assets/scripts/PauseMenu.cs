using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject pausePanel;
    public Button resumeButton;
    public Button settingsButton;
    public Button mainMenuButton;
    public SettingsMenu settingsMenu;

    [Header("Ajustes")]
    public KeyCode pauseKey = KeyCode.Escape;
    public string mainMenuSceneName = "MenuPrincipal";   // Nombre EXACTO de tu escena de menú

    bool isPaused;

    void Start()
    {
        pausePanel.SetActive(false);
        resumeButton.onClick.AddListener(Resume);
        settingsButton.onClick.AddListener(OpenSettings);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        settingsMenu.Closed += OnSettingsClosed;
    }

    void Update()
    {
        if (!Input.GetKeyDown(pauseKey)) return;

        if (isPaused)
        {
            // Si los ajustes están abiertos, Esc los cierra y vuelve a la pausa
            if (settingsMenu.IsOpen) settingsMenu.Close();
            else Resume();
        }
        else if (Time.timeScale > 0f) Pause();   // Si ya está congelado (muerte/victoria), no se puede pausar
    }

    void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu Inicio");
    }

    void OpenSettings()
    {
        pausePanel.SetActive(false);   // Oculta los botones de pausa
        settingsMenu.Open();
    }

    void OnSettingsClosed()
    {
        pausePanel.SetActive(true);    // Los vuelve a mostrar al volver
    }
}
