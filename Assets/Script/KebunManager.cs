using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class KebunManager : MonoBehaviour
{
    public static KebunManager instance;
    public GameObject prefabTruk;
    public Transform spawnPointTruk;

    // BARU: Tambahkan list untuk menampung titik-titik pemberhentian dari Hierarchy
    public List<Transform> listStopPoints = new List<Transform>();

    private List<SawitGrowth> daftarAntreanPohon = new List<SawitGrowth>();
    private bool sedangProsesTruk = false;

    void Awake() { instance = this; }

    public void TambahLaporanBerbuah(SawitGrowth pohon)
    {
        if (!daftarAntreanPohon.Contains(pohon))
        {
            daftarAntreanPohon.Add(pohon);
            Debug.Log("<color=cyan>Pohon Berbuah = " + daftarAntreanPohon.Count + "</color>");
        }

        if (daftarAntreanPohon.Count >= 3 && !sedangProsesTruk)
        {
            StartCoroutine(SiklusAntreanTruk());
        }
    }

    IEnumerator SiklusAntreanTruk()
    {
        sedangProsesTruk = true;

        while (daftarAntreanPohon.Count >= 3)
        {
            // --- LOGIKA PEMBATASAN BARU ---
            // Hitung berapa truk yang sedang aktif di world
            int jumlahTrukSekarang = FindObjectsOfType<TruckSawitManager>().Length;

            if (jumlahTrukSekarang >= 6)
            {
                // Jika sudah 6, tunggu 5 detik lalu cek lagi (jangan spawn dulu)
                Debug.Log("<color=red>World Penuh! Menunda spawn truk...</color>");
                yield return new WaitForSeconds(5f);
                continue; // Ulangi loop dari atas untuk cek jumlah lagi
            }
            // ------------------------------

            // 1. Munculkan Truk
            GameObject trukBaru = Instantiate(prefabTruk, spawnPointTruk.position, spawnPointTruk.rotation);

            TruckSawitManager scriptTruk = trukBaru.GetComponent<TruckSawitManager>();
            if (scriptTruk != null)
            {
                scriptTruk.IsiMuatanFull();
                scriptTruk.MulaiMisi(listStopPoints);
            }

            // 2. Reset 3 pohon terdepan
            for (int i = 0; i < 3; i++)
            {
                if (daftarAntreanPohon.Count > 0)
                {
                    daftarAntreanPohon[0].ResetPohon();
                    daftarAntreanPohon.RemoveAt(0);
                }
            }

            Debug.Log("<color=yellow>Truk Keluar! Sisa Antrean: " + daftarAntreanPohon.Count + "</color>");

            // 3. JEDA 20 DETIK sebelum truk berikutnya boleh muncul
            yield return new WaitForSeconds(20f);
        }

        sedangProsesTruk = false;
    }
}