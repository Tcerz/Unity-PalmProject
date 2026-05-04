using UnityEngine;
using UnityEngine.SceneManagement; // Library wajib untuk pindah scene

public class LoseManager : MonoBehaviour
{
    // Fungsi untuk tombol COBA LAGI (Restart)
    public void CobaLagi()
    {
        // Berdasarkan gambar Build Settings kamu, SampleScene ada di Index 1
        SceneManager.LoadScene(1);
    }

    // Fungsi untuk tombol KEMBALI KE MENU
    public void KeMenuUtama()
    {
        // Berdasarkan gambar Build Settings kamu, MenuUtama ada di Index 0
        SceneManager.LoadScene(0);
    }
}