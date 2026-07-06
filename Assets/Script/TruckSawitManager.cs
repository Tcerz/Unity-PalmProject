using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class TruckSawitManager : MonoBehaviour
{
    public GameObject tumpukanSawitVisual;
    private NavMeshAgent agent;
    private Transform targetQueue;
    private bool sedangAntre = false;
    private bool sedangMisi = false;

    public bool telahLolosPemeriksaan = false;
    public bool batalMisi = false;
    public static int jumlahTrukAktif = 0;
    public TruckData dataTruk;
    public TruckData myData; // Data Asli (untuk timbangan)
    public TruckData dataDokumen;

    void OnEnable() { jumlahTrukAktif++; }
    void OnDisable() { jumlahTrukAktif--; }

    void Awake() { agent = GetComponent<NavMeshAgent>(); }

    // Panggil ini dari KebunManager untuk "Membangunkan" truk
    public void ResetTrukUntukMisiBaru()
    {
        sedangMisi = false;
        telahLolosPemeriksaan = false;
        batalMisi = false;

        if (agent != null) agent.enabled = true;
        IsiMuatanFull();

        if (GerbangQueueManager.Instance != null)
            GerbangQueueManager.Instance.DaftarkanTruk(this);

        PengecekanMuatan cek = GetComponentInChildren<PengecekanMuatan>();
        if (cek != null) cek.SetRandomFruit();
    }

    public void IsiMuatanFull() { if (tumpukanSawitVisual) tumpukanSawitVisual.SetActive(true); }

    public void PergiKeQueue(Transform titikQueue)
    {
        if (sedangMisi) return;
        targetQueue = titikQueue;
        sedangAntre = true;
        agent.isStopped = false;
        agent.SetDestination(targetQueue.position);
    }

    void Update()
    {
        if (sedangAntre && targetQueue != null && agent.enabled)
        {
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                agent.isStopped = true;
        }
    }
    public void SetData(TruckData data)
    {
        myData = data;
    }

    public void InitializeTruk(TruckData data)
    {
        this.dataTruk = data;
        this.dataDokumen = new TruckData();

        // Salin data dasar
        this.dataDokumen.noRegis = data.noRegis;
        this.dataDokumen.namaPengemudi = data.namaPengemudi;
        this.dataDokumen.jenisTruk = data.jenisTruk;
        this.dataDokumen.perusahaan = data.perusahaan;
        this.dataDokumen.kelayakan = data.kelayakan;

        // Logika 25% kemungkinan (1 dari 4) berat dokumen dimanipulasi
        // Menggunakan Random.value (0.0 sampai 1.0)
        if (Random.value < 0.25f)
        {
            // Beri selisih acak antara 500 hingga 1000 kg agar terlihat jelas
            int selisih = Random.Range(500, 1000);
            this.dataDokumen.beratTrukAsli = data.beratTrukAsli + selisih;
        }
        else
        {
            this.dataDokumen.beratTrukAsli = data.beratTrukAsli;
        }

        Debug.Log("Truk: " + data.noRegis + " | Asli: " + dataTruk.beratTrukAsli + " | Dokumen: " + dataDokumen.beratTrukAsli);
    }

    public void MulaiMisi(List<Transform> waypoint)
    {
        if (sedangMisi) return;
        sedangMisi = true;
        sedangAntre = false;
        StartCoroutine(JalankanMisi(waypoint));
    }

    IEnumerator JalankanMisi(List<Transform> p)
    {
        yield return StartCoroutine(JalanKeTitik(p[0].position));
        GerbangQueueManager.Instance.SetTrukDiPos(this);

        while (!telahLolosPemeriksaan && !batalMisi) yield return null;

        if (telahLolosPemeriksaan)
        {
            int pilih = Random.Range(1, 3);
            yield return StartCoroutine(JalanKeTitik(p[pilih].position));
            yield return new WaitForSeconds(5);
            yield return StartCoroutine(JalanKeTitik(p[3].position));
            yield return new WaitForSeconds(10);
            if (tumpukanSawitVisual) tumpukanSawitVisual.SetActive(false);
        }

        yield return StartCoroutine(JalanKeTitik(p[4].position));
        yield return StartCoroutine(JalanKeTitik(p[5].position));

        // GANTI DESTROY DENGAN INI:
        gameObject.SetActive(false);
    }

    IEnumerator JalanKeTitik(Vector3 tujuan)
    {
        agent.isStopped = false;
        agent.SetDestination(tujuan);
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            yield return null;
    }
}