using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class TruckSawitManager : MonoBehaviour
{
    [Header("Settings")]
    public GameObject tumpukanSawitVisual;

    private NavMeshAgent agent;
    private NavMeshObstacle obstacle; // Tambahkan ini agar tidak error

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>(); // Ambil komponen obstacle

        if (agent != null) agent.enabled = false;

        // Setup awal Obstacle: Matikan dulu, aktifkan Carving
        if (obstacle != null)
        {
            obstacle.enabled = false;
            obstacle.carving = true;
        }
    }

    public void IsiMuatanFull()
    {
        if (tumpukanSawitVisual != null)
        {
            tumpukanSawitVisual.SetActive(true);
            tumpukanSawitVisual.transform.SetParent(this.transform);
            tumpukanSawitVisual.transform.localPosition = new Vector3(0, 1.3f, -0.5f);
            tumpukanSawitVisual.transform.localRotation = Quaternion.identity;
        }
    }

    public void MulaiMisi(List<Transform> points)
    {
        if (agent != null) agent.enabled = true;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 5.0f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
        }

        StartCoroutine(LogikaPerjalanan(points));
    }

    IEnumerator LogikaPerjalanan(List<Transform> p)
    {
        yield return new WaitForEndOfFrame();

        // 1. KE POS MASUK
        yield return StartCoroutine(JalanKeTitik(p[0].position));
        yield return new WaitForSeconds(2f);

        // 2. KE TIMBANGAN (Acak 1 atau 2)
        int pilihTimbangan = Random.Range(1, 3);

        // Saat menimbang, kita gunakan logika antrean
        yield return StartCoroutine(JalanKeTitik(p[pilihTimbangan].position));

        // --- LOGIKA ANTREAN DI TIMBANGAN ---
        AktifkanModeAntre(true);
        yield return new WaitForSeconds(5f); // Waktu nimbang
        AktifkanModeAntre(false);
        // -----------------------------------

        // 3. KE DROP SAWIT
        yield return StartCoroutine(JalanKeTitik(p[3].position));

        // Proses Bongkar
        AktifkanModeAntre(true);
        float waktuBongkar = Random.Range(10f, 20f);
        yield return new WaitForSeconds(waktuBongkar);
        if (tumpukanSawitVisual != null) tumpukanSawitVisual.SetActive(false);
        AktifkanModeAntre(false);

        // 4. KE POS KELUAR
        yield return StartCoroutine(JalanKeTitik(p[4].position));

        // 5. KE DESTROY POINT
        yield return StartCoroutine(JalanKeTitik(p[5].position));

        Destroy(gameObject);
    }

    // Fungsi pembantu agar kode tidak panjang berulang-ulang
    IEnumerator JalanKeTitik(Vector3 tujuan)
    {
        agent.SetDestination(tujuan);
        yield return new WaitUntil(() => SampaiTujuan());
    }

    void AktifkanModeAntre(bool aktif)
    {
        if (aktif)
        {
            agent.enabled = false;
            if (obstacle != null) obstacle.enabled = true;
        }
        else
        {
            if (obstacle != null) obstacle.enabled = false;
            agent.enabled = true;
        }
    }

    bool SampaiTujuan()
    {
        if (agent == null || !agent.enabled || !agent.isOnNavMesh) return false;

        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude <= 0.1f)
                    return true;
            }
        }
        return false;
    }
}