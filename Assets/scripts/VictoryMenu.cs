using UnityEngine;
using UnityEngine.UI;

public class VictoryMenu : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject victoryPanel;   // Panel con la imagen/texto de victoria
    public Button restartButton;
    public Button quitButton;

    void Start()
    {
        victoryPanel.SetActive(false);
        restartButton.onClick.AddListener(Gamemanager.RestartLevel);
        quitButton.onClick.AddListener(Gamemanager.QuitGame);
    }

    public void Show()
    {
        victoryPanel.SetActive(true);

        // Congela el juego (monstruos incluidos) y libera el ratón para poder pulsar botones
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
