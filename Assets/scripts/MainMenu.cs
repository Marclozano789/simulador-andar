using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Escenas")]
    public string gameSceneName = "SampleScene";   // Nombre EXACTO de tu escena de juego

    [Header("Paneles (opcional)")]
    public GameObject mainPanel;
    public GameObject controlsPanel;

    void Start()
    {
        // Por si venimos de una partida congelada o con el cursor bloqueado
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (controlsPanel != null) controlsPanel.SetActive(false);
    }

    // Enlaza estos métodos en el OnClick() de cada botón

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void ShowControls()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (controlsPanel != null) controlsPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        if (controlsPanel != null) controlsPanel.SetActive(false);
        if (mainPanel != null) mainPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
