using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUtama : MonoBehaviour
{
    // Fungsi untuk tombol MULAI
    public void StartGim()
    {
        // Ganti "SampleScene" menjadi "Proto Road" sesuai nama file di folder Scenes kamu
        // Pastikan ejaan dan spasi sama persis
        SceneManager.LoadScene("Proto Road");
    }

    // Fungsi untuk tombol KELUAR
    public void KeluarGim()
    {
        Debug.Log("Gim Berhenti...");

        // Application.Quit hanya bekerja setelah game di-build (.exe)
        // Baris di bawah ini membantu agar tombol keluar juga berfungsi saat di Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}