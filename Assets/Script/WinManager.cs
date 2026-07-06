using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public void MainLagi()
    {
        // Ganti ke index 2 agar langsung ke level "Proto Road"
        SceneManager.LoadScene(2);
    }

    public void KeMenuUtama()
    {
        // Ganti ke index 1 sesuai posisi "MenuUtama" di Build Settings kamu
        SceneManager.LoadScene(1);
    }
}