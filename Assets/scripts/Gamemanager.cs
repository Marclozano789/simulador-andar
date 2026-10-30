using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public static void PlayerDied()
    {

        if (DeathMenu.Instance != null) DeathMenu.Instance.Show();
        // Sustituye por tu pantalla de Game Over / jumpscare
        RestartLevel();
    }

    public static void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public static void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
