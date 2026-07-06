using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Taruh Empty Object untuk titik aman di sini")]
    public Transform respawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        // Deteksi apakah yang jatuh adalah Player
        if (other.CompareTag("Player"))
        {
            RespawnPlayer(other.gameObject);
        }
    }

    void RespawnPlayer(GameObject player)
    {
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null)
        {
            // Matikan CC agar posisi bisa dipindahkan instan
            cc.enabled = false;
            player.transform.position = respawnPoint.position;
            cc.enabled = true;
        }
        else
        {
            // Jika tidak pakai CC, langsung pindah posisi
            player.transform.position = respawnPoint.position;
        }

        //Debug.Log("Player respawned ke: " + respawnPoint.name);
    }
}