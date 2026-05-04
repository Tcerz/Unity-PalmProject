using UnityEngine;
using UnityEngine.SceneManagement;

public class WinManager : MonoBehaviour
{
    public void MainLagi()
    {
        // Memuat ulang level pabrik (Index 1 di Build Settings)
        SceneManager.LoadScene(1);
    }

    public void KeMenuUtama()
    {
        // Kembali ke menu awal (Index 0 di Build Settings)
        SceneManager.LoadScene(0);
    }
}