using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseMenu : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject pausePanel;
    public Button resumeButton;
    public Button mainMenuButton;
    public Button quitButton;

    [Header("Ajustes")]
    public KeyCode pauseKey = KeyCode.Escape;
    public string mainMenuSceneName = "MenuPrincipal";   // Nombre EXACTO de tu escena de menú

    bool isPaused;

    void Start()
    {
        pausePanel.SetActive(false);
        resumeButton.onClick.AddListener(Resume);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        quitButton.onClick.AddListener(Gamemanager.QuitGame);
    }

    void Update()
    {
        if (!Input.GetKeyDown(pauseKey)) return;

        if (isPaused) Resume();
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
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
