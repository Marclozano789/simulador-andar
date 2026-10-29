using UnityEngine;

// Pon este script en un objeto con Collider (Is Trigger activado) que cubra la zona iluminada.
// El jugador debe tener el Tag "Player".
[RequireComponent(typeof(Collider))]
public class Lightzone : MonoBehaviour
{

    public static bool PlayerIsSafe { get; private set; }
    static int zonesInside;

    void Reset() => GetComponent<Collider>().isTrigger = true;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        zonesInside++;
        PlayerIsSafe = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        zonesInside = Mathf.Max(0, zonesInside - 1);
        PlayerIsSafe = zonesInside > 0;
    }
}
