using UnityEngine;
using UnityEngine.UI;

public class PengecekanMuatan : MonoBehaviour
{
    [Header("Referensi UI")]
    public GameObject uiPanelSampelBuah;
    public Image imageDisplay;

    [Header("Data Buah")]
    public Sprite[] daftarSpriteBuah;

    private bool isPlayerNear = false;
    public string statusBuahTerpilih;

    void Awake()
    {
        // PERUBAHAN: Gunakan GetComponentInChildren agar skrip bisa mencari ke dalam child/cucu objek
        if (uiPanelSampelBuah == null)
            uiPanelSampelBuah = transform.GetComponentInChildren<Transform>(true).Find("PanelSampelBuah")?.gameObject;

        // Alternatif paling aman jika struktur tetap:
        if (uiPanelSampelBuah == null)
            uiPanelSampelBuah = transform.Find("Canvas/PanelSampelBuah").gameObject;

        // Mencari komponen Image di objek PanelSampelBuah
        if (imageDisplay == null && uiPanelSampelBuah != null)
            imageDisplay = uiPanelSampelBuah.GetComponent<Image>();

        // Pastikan panel tersembunyi saat game dimulai
        if (uiPanelSampelBuah != null) uiPanelSampelBuah.SetActive(false);
    }

    private static System.Random rng = new System.Random();

    public void SetRandomFruit()
    {
        if (daftarSpriteBuah.Length > 0 && imageDisplay != null)
        {
            int randomIndex = rng.Next(0, daftarSpriteBuah.Length);
            imageDisplay.sprite = daftarSpriteBuah[randomIndex];

            // Logika pemetaan berdasarkan index sprite di Inspector
            if (randomIndex <= 1) // Element 0 & 1
                statusBuahTerpilih = "tidak bagus";
            else if (randomIndex <= 3) // Element 2 & 3
                statusBuahTerpilih = "kurang";
            else // Element 4 & 5
                statusBuahTerpilih = "bagus";
        }
    }

    // ... sisa kode Update dan OnTrigger tetap sama ...
    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            if (uiPanelSampelBuah != null) uiPanelSampelBuah.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other) { if (other.CompareTag("Player")) isPlayerNear = true; }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (uiPanelSampelBuah != null) uiPanelSampelBuah.SetActive(false);
        }
    }
}