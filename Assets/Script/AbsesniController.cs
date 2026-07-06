using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Wajib untuk pindah scene
using TMPro;

public class AbsensiController : MonoBehaviour
{
    [Header("UI")]
    public GameObject panelLaporan;
    public TMP_Text teksDetailLaporan;
    public Button btnEnd; // Pastikan slot ini diisi di Inspector

    [Header("Player Ref")]
    public PlayerMovement playerMovement; // Tarik skrip PlayerMovement Anda ke sini

    private bool playerBisaInteraksi = false;

    void Start()
    {
        if (btnEnd != null) btnEnd.onClick.AddListener(CheckWinLose);
    }

    void Update()
    {
        if (playerBisaInteraksi && Input.GetKeyDown(KeyCode.E))
        {
            TampilkanLaporan();
        }
    }

    public void TampilkanLaporan()
    {
        if (panelLaporan == null) return;

        panelLaporan.SetActive(true);

        // 1. Kunci Pergerakan dengan menonaktifkan skrip PlayerMovement
        if (playerMovement != null) playerMovement.enabled = false;

        // 2. Unlock Cursor agar pemain bisa klik tombol
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Tampilkan teks laporan
        string laporan = "LAPORAN KERJA:\n\n";
        foreach (string entry in GameManager.Instance.riwayatPoin)
        {
            laporan += entry + "\n";
        }
        laporan += "\n--------------------\n";
        laporan += "TOTAL POIN = " + GameManager.Instance.totalPoin;

        teksDetailLaporan.text = laporan;
    }

    public void CheckWinLose()
    {
        int total = GameManager.Instance.totalPoin;

        // Logika Win/Lose
        if (total >= 2000)
            SceneManager.LoadScene("WinScene"); // Pastikan nama scene sesuai
        else if (total < 1500)
            SceneManager.LoadScene("LoseScene"); // Pastikan nama scene sesuai
        else
        {
            Debug.Log("Poin belum memenuhi syarat untuk berakhir: " + total);
            // Opsional: Tutup panel jika poin belum memenuhi syarat
            panelLaporan.SetActive(false);
            if (playerMovement != null) playerMovement.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) playerBisaInteraksi = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) playerBisaInteraksi = false;
    }
}