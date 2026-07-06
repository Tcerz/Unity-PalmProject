using UnityEngine;
using System.Collections;

public class KontrolGerbang : MonoBehaviour
{
    public enum TipeGerbang { Masuk, Keluar }

    [Header("Identitas Gerbang")]
    public TipeGerbang tipeGerbang;

    [Header("Komponen")]
    public Animator animatorEngsel;
    public GameObject teksPetunjuk;

    [Header("Pengaturan")]
    [Tooltip("Waktu tunggu sebelum gerbang menutup otomatis (detik)")]
    public float durasiBuka = 5f;

    [Header("Status")]
    public bool sedangTerbuka = false;
    private bool playerBisaTekan = false;
    private Coroutine coroutineTutupOtomatis;

    void Update()
    {
        if (playerBisaTekan && Input.GetKeyDown(KeyCode.E))
        {
            ToggleGerbang();
        }
    }

    public void ToggleGerbang()
    {
        if (!sedangTerbuka) BukaPalang();
        else TutupPalang();
    }

    public void BukaPalang()
    {
        if (sedangTerbuka) return;

        animatorEngsel.SetTrigger("Pencet");
        sedangTerbuka = true;

        // Reset dan mulai ulang timer tutup otomatis
        if (coroutineTutupOtomatis != null) StopCoroutine(coroutineTutupOtomatis);
        coroutineTutupOtomatis = StartCoroutine(TutupOtomatisRoutine());

        //Debug.Log(gameObject.name + " Membuka...");
    }

    public void TutupPalang()
    {
        if (!sedangTerbuka) return;

        animatorEngsel.SetTrigger("Tutup");
        sedangTerbuka = false;

        if (coroutineTutupOtomatis != null) StopCoroutine(coroutineTutupOtomatis);
        //Debug.Log(gameObject.name + " Menutup...");
    }

    private IEnumerator TutupOtomatisRoutine()
    {
        yield return new WaitForSeconds(durasiBuka);
        TutupPalang();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerBisaTekan = true;
            if (teksPetunjuk != null) teksPetunjuk.SetActive(true);
        }

        // Logika Otomatis Truk (Hanya Gerbang Keluar)
        if (tipeGerbang == TipeGerbang.Keluar && other.GetComponent<TruckSawitManager>())
        {
            BukaPalang();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerBisaTekan = false;
            if (teksPetunjuk != null) teksPetunjuk.SetActive(false);
        }
    }
}