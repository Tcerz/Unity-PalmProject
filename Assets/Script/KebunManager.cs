using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KebunManager : MonoBehaviour
{
    public static KebunManager instance;

    [Header("Referensi Truk")]
    // Taruh truk yang ada di scene (bawah tanah) ke sini
    public GameObject prefabTruk;
    public Transform spawnPointTruk;

    [Header("Referensi Lain")]
    public KontrolGerbang referensiGerbangWorld;
    public List<Transform> listStopPoints = new List<Transform>();
    public int batasMaksimalBuah = 60;

    private List<SawitGrowth> daftarAntreanPohon = new List<SawitGrowth>();
    private List<TruckSawitManager> poolTruk = new List<TruckSawitManager>();
    private bool sedangProsesTruk = false;
    public TruckDatabase database;

    void Awake()
    {
        instance = this;
        // Sembunyikan truk awal di start
        if (prefabTruk != null) prefabTruk.SetActive(false);
    }

    public bool TambahLaporanBerbuah(SawitGrowth pohon)
    {
        if (daftarAntreanPohon.Count >= batasMaksimalBuah) return false;
        if (!daftarAntreanPohon.Contains(pohon))
        {
            daftarAntreanPohon.Add(pohon);
            if (daftarAntreanPohon.Count >= 3 && !sedangProsesTruk)
                StartCoroutine(SiklusSpawnTruk());
            return true;
        }
        return false;
    }

    IEnumerator SiklusSpawnTruk()
    {
        sedangProsesTruk = true;
        while (daftarAntreanPohon.Count >= 3)
        {
            if (TruckSawitManager.jumlahTrukAktif < 4)
            {
                SpawnTruk();
                for (int i = 0; i < 3; i++)
                {
                    if (daftarAntreanPohon.Count > 0)
                    {
                        daftarAntreanPohon[0].ResetPohon();
                        daftarAntreanPohon.RemoveAt(0);
                    }
                }
                yield return new WaitForSeconds(5f);
            }
            else yield return new WaitForSeconds(2f);
        }
        sedangProsesTruk = false;
    }

    void SpawnTruk()
    {
        TruckSawitManager trukTersedia = poolTruk.Find(t => !t.gameObject.activeInHierarchy);

        if (trukTersedia == null)
        {
            GameObject obj = Instantiate(prefabTruk, spawnPointTruk.position, spawnPointTruk.rotation);
            trukTersedia = obj.GetComponent<TruckSawitManager>();
            poolTruk.Add(trukTersedia);
        }
        else
        {
            trukTersedia.transform.position = spawnPointTruk.position;
            trukTersedia.transform.rotation = spawnPointTruk.rotation;
        }

        TruckData dataBaru = database.GetRandomTruck();
        trukTersedia.InitializeTruk(dataBaru);

        trukTersedia.gameObject.SetActive(true);

        trukTersedia.gameObject.SetActive(true);
        trukTersedia.ResetTrukUntukMisiBaru();
    }
}