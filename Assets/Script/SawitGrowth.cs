using System.Collections;
using UnityEngine;

public class SawitGrowth : MonoBehaviour
{
    public GameObject buahSawit;
    public bool sedangBerbuah = false;

    [Header("Pengaturan Waktu")]
    public float waktuMin = 10f;
    public float waktuMax = 90f;

    void Start()
    {
        StartCoroutine(ProsesTumbuh());
    }

    IEnumerator ProsesTumbuh()
    {
        // Menghasilkan angka acak yang unik untuk setiap pohon
        float durasiAcak = Random.Range(waktuMin, waktuMax);
        yield return new WaitForSeconds(durasiAcak);

        if (!sedangBerbuah)
        {
            buahSawit.SetActive(true);
            sedangBerbuah = true;

            if (KebunManager.instance != null)
            {
                KebunManager.instance.TambahLaporanBerbuah(this);
            }
        }
    }

    public void ResetPohon()
    {
        StopAllCoroutines();
        buahSawit.SetActive(false);
        sedangBerbuah = false;

        // Jangan langsung tumbuh! Kasih jeda istirahat 30-60 detik 
        // baru mulai proses tumbuh lagi.
        StartCoroutine(JedaSebelumTumbuhLagi());
    }

    IEnumerator JedaSebelumTumbuhLagi()
    {
        float jedaIstirahat = Random.Range(30f, 60f);
        yield return new WaitForSeconds(jedaIstirahat);
        StartCoroutine(ProsesTumbuh());
    }
}