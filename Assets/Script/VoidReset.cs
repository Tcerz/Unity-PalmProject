using UnityEngine;

public class VoidReset : MonoBehaviour
{
    // Objek kosong yang kita buat sebagai titik aman di atas tanah
    public Transform spawnPoint;

    // Fungsi ini otomatis jalan saat ada benda masuk ke area "Trigger"
    private void OnTriggerEnter(Collider other)
    {
        // Mengecek apakah yang jatuh itu objek bertag "Player"
        if (other.CompareTag("Player"))
        {
            // Jika player pakai CharacterController (standar FPP), 
            // kita harus matikan dulu CC-nya sebentar agar posisinya bisa dipindah.
            CharacterController cc = other.GetComponent<CharacterController>();

            if (cc != null)
            {
                cc.enabled = false; // Matikan fisik
                other.transform.position = spawnPoint.position; // Pindahkan posisi
                cc.enabled = true; // Nyalakan fisik lagi
            }
            else
            {
                // Jika tidak pakai CharacterController, langsung pindah saja
                other.transform.position = spawnPoint.position;
            }

            //Debug.Log("Player jatuh ke void! Mengembalikan ke posisi aman.");
        }
    }
}