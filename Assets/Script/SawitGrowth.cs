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
            // Minta izin ke Manager sebelum tumbuh
            bool berhasil = KebunManager.instance.TambahLaporanBerbuah(this);

            if (berhasil)
            {
                buahSawit.SetActive(true);
                sedangBerbuah = true;
            }
            else
            {
                // Jika tidak berhasil, tunggu sebentar lalu coba lagi
                yield return new WaitForSeconds(5f);
                StartCoroutine(ProsesTumbuh());
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
        float jedaIstirahat = Random.Range(5f, 20f);
        yield return new WaitForSeconds(jedaIstirahat);
        StartCoroutine(ProsesTumbuh());
    }
}