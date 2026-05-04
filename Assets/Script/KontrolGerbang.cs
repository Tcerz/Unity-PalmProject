using UnityEngine;

public class KontrolGerbang : MonoBehaviour
{
    public enum TipeGerbang { Masuk, Keluar }

    [Header("Identitas Gerbang")]
    public TipeGerbang tipeGerbang;

    [Header("Komponen")]
    public Animator animatorEngsel;
    public GameObject teksPetunjuk;

    [Header("Status")]
    public bool sedangTerbuka = false;
    private bool playerBisaTekan = false;

    void Update()
    {
        // Player tetap bisa tekan E manual di gerbang mana saja
        if (playerBisaTekan && Input.GetKeyDown(KeyCode.E))
        {
            ToggleGerbang();
        }
    }

    // Fungsi tunggal untuk buka/tutup agar kode rapi
    public void ToggleGerbang()
    {
        if (!sedangTerbuka) BukaPalang();
        else TutupPalang();
    }

    void BukaPalang()
    {
        animatorEngsel.SetTrigger("Pencet");
        sedangTerbuka = true;
        Debug.Log(gameObject.name + " Membuka...");
    }

    public void TutupPalang()
    {
        animatorEngsel.SetTrigger("Tutup");
        sedangTerbuka = false;
        Debug.Log(gameObject.name + " Menutup...");
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. Logika untuk Player
        if (other.CompareTag("Player"))
        {
            playerBisaTekan = true;
            if (teksPetunjuk != null) teksPetunjuk.SetActive(true);
        }

        // 2. Logika OTOMATIS (Simulasi tekan E) untuk Gerbang Keluar
        // Jika yang masuk adalah Truk DAN ini adalah Gerbang Keluar
        if (tipeGerbang == TipeGerbang.Keluar && other.GetComponent<TruckSawitManager>())
        {
            if (!sedangTerbuka)
            {
                Debug.Log("Truk terdeteksi di Gerbang Keluar, membuka otomatis...");
                BukaPalang();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Matikan akses Player
        if (other.CompareTag("Player"))
        {
            playerBisaTekan = false;
            if (teksPetunjuk != null) teksPetunjuk.SetActive(false);
        }

        // Tutup Otomatis setelah Truk lewat (Berlaku untuk Masuk & Keluar)
        if (other.GetComponent<TruckSawitManager>())
        {
            if (sedangTerbuka)
            {
                // Beri sedikit delay (opsional) agar truk tidak terjepit palang
                Invoke("TutupPalang", 2f);
            }
        }
    }
}