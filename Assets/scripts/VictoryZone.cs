using UnityEngine;

// Pon este script en un objeto con Collider (Is Trigger) delante de la puerta de tu casa.
[RequireComponent(typeof(Collider))]
public class VictoryZone : MonoBehaviour
{
    public VictoryMenu victoryMenu;
    bool triggered;

    void Reset() => GetComponent<Collider>().isTrigger = true;

    void OnTriggerEnter(Collider other)
    {
        if (triggered || !other.CompareTag("Player")) return;
        triggered = true;
        victoryMenu.Show();
    }
}
