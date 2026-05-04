using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class TruckSawitManager : MonoBehaviour
{
    [Header("Visual Settings")]
    public GameObject tumpukanSawitVisual;

    [Header("Logic Settings")]
    public KontrolGerbang skripGerbang;
    public float jarakDeteksiDepan = 6.0f; // Sensor agar tidak menabrak pantat truk depan

    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        if (agent != null)
        {
            agent.enabled = false;
            agent.stoppingDistance = 2.5f; // Jarak berhenti agar tidak terlalu mepet
        }

        if (obstacle != null)
        {
            obstacle.enabled = false;
            obstacle.carving = true;
        }
    }

    // --- FUNGSI MUATAN (KITA KEMBALIKAN) ---
    public void IsiMuatanFull()
    {
        if (tumpukanSawitVisual != null)
        {
            tumpukanSawitVisual.SetActive(true);
            tumpukanSawitVisual.transform.SetParent(this.transform);

            // Atur ulang posisi agar presisi di bak (sesuaikan lagi angkanya jika perlu)
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

        // 1. KE POS MASUK (DEPAN GERBANG)
        yield return StartCoroutine(JalanKeTitikDenganSensor(p[0].position));

        // --- LOGIKA GERBANG & ANTI TEROBOS ---
        if (skripGerbang != null)
        {
            AktifkanModeAntre(true); // Pasang obstacle agar yg belakang stop

            // Syarat: Gerbang harus terbuka DAN truk depan sudah menjauh (kosong)
            yield return new WaitUntil(() => skripGerbang.sedangTerbuka == true && !AdaMobilMepetDiDepan());

            Debug.Log("Truk: Giliran saya masuk gerbang.");
            AktifkanModeAntre(false);

            // Jeda 2 detik agar truk belakang tidak langsung "nempel" ikut masuk
            yield return new WaitForSeconds(2.5f);
        }

        // 2. KE TIMBANGAN (Acak 1 atau 2)
        int pilihTimbangan = Random.Range(1, 3);
        yield return StartCoroutine(JalanKeTitikDenganSensor(p[pilihTimbangan].position));

        AktifkanModeAntre(true);
        yield return new WaitForSeconds(5f); // Proses timbang
        AktifkanModeAntre(false);

        // 3. KE DROP SAWIT
        yield return StartCoroutine(JalanKeTitikDenganSensor(p[3].position));

        AktifkanModeAntre(true);
        yield return new WaitForSeconds(Random.Range(10f, 15f)); // Proses bongkar

        if (tumpukanSawitVisual != null) tumpukanSawitVisual.SetActive(false); // Sawit hilang (kosong)

        AktifkanModeAntre(false);

        // 4. KE POS KELUAR & DESTROY
        yield return StartCoroutine(JalanKeTitikDenganSensor(p[4].position));
        yield return StartCoroutine(JalanKeTitikDenganSensor(p[5].position));

        Destroy(gameObject);
    }

    // --- LOGIKA SENSOR JARAK ---
    void LogikaSensorJarak()
    {
        RaycastHit hit;
        // Tembakkan sinar ke depan (sedikit agak tinggi agar tidak kena aspal)
        Vector3 origin = transform.position + Vector3.up * 1.0f;

        if (Physics.Raycast(origin, transform.forward, out hit, jarakDeteksiDepan))
        {
            // Jika ada objek lain (truk) di depan
            if (hit.collider.GetComponent<TruckSawitManager>() || hit.collider.CompareTag("Vehicle"))
            {
                agent.isStopped = true;
                return;
            }
        }
        agent.isStopped = false;
    }

    bool AdaMobilMepetDiDepan()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 1.0f;
        // Cek jarak pendek (4 meter) untuk antrean gerbang
        if (Physics.Raycast(origin, transform.forward, out hit, 4.0f))
        {
            if (hit.collider.GetComponent<TruckSawitManager>()) return true;
        }
        return false;
    }

    IEnumerator JalanKeTitikDenganSensor(Vector3 tujuan)
    {
        if (agent != null && agent.enabled)
        {
            agent.SetDestination(tujuan);
            while (!SampaiTujuan())
            {
                LogikaSensorJarak(); // Selama jalan, terus cek jarak
                yield return null;
            }
        }
    }

    void AktifkanModeAntre(bool aktif)
    {
        if (aktif)
        {
            if (agent != null) agent.enabled = false;
            if (obstacle != null) obstacle.enabled = true;
        }
        else
        {
            if (obstacle != null) obstacle.enabled = false;
            if (agent != null) agent.enabled = true;
        }
    }

    bool SampaiTujuan()
    {
        if (agent == null || !agent.enabled || !agent.isOnNavMesh) return false;
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }
}