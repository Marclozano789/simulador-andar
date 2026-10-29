using UnityEngine;
using UnityEngine.SceneManagement;

public class Gamemanager : MonoBehaviour
{
    public static void PlayerDied()
    {
        // Sustituye por tu pantalla de Game Over / jumpscare
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
