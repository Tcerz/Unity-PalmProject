using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Wajib untuk pindah scene

public class MenuUtama : MonoBehaviour
{
    // Fungsi untuk tombol MULAI (pindah ke arena pabrik)
    public void StartGim()
    {
        // Pastikan "SampleScene" sudah terdaftar di Build Settings
        SceneManager.LoadScene("SampleScene");
    }

    // Fungsi untuk tombol KELUAR
    public void KeluarGim()
    {
        Debug.Log("Gim Berhenti..."); // Cek di console
        Application.Quit();
    }
}