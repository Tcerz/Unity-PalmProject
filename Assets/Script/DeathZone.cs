using UnityEngine;

public class DeathZone : MonoBehaviour
{
    // Titik awal atau posisi aman untuk respawn
    public Vector3 respawnPoint = new Vector3(0f, 1f, 0f);

    private void OnTriggerEnter(Collider other)
    {
        // Mengecek apakah yang masuk ke zona ini adalah Player
        if (other.CompareTag("Player"))
        {
            // Karena kita menggunakan Character Controller, kita perlu mematikannya sejenak
            // agar posisi player bisa dipindahkan secara instan tanpa konflik fisik.
            CharacterController cc = other.GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false;
                other.transform.position = respawnPoint;
                cc.enabled = true;
            }
        }
    }
}