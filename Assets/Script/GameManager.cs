using UnityEngine;
using TMPro;
using System.Collections.Generic; // Wajib untuk List

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Sistem Skor")]
    public int totalPoin = 0; // Diubah jadi 0 agar perhitungan dimulai dari nol
    public TMP_Text teksSkorUI;

    // List untuk menyimpan riwayat poin
    public List<string> riwayatPoin = new List<string>();

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateUITeks();
    }

    // Fungsi TambahPoin sekarang memerlukan 'alasan' untuk log
    public void TambahPoin(int poinTambahan, string alasan)
    {
        totalPoin += poinTambahan;

        // Simpan log ke dalam list
        string tanda = poinTambahan >= 0 ? "+" : "";
        riwayatPoin.Add(alasan + " (" + tanda + poinTambahan + ")");

        UpdateUITeks();
        Debug.Log("Poin berubah! Total sekarang: " + totalPoin);
    }

    void UpdateUITeks()
    {
        if (teksSkorUI != null)
        {
            teksSkorUI.text = "Poin: " + totalPoin;
        }
    }
}