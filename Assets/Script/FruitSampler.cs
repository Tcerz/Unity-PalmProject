using UnityEngine;
using UnityEngine.UI; // Wajib ditambahkan untuk mengakses UI
using System.Collections;

public class FruitSampler : MonoBehaviour
{
    [Header("Pengaturan UI")]
    [SerializeField] private Image uiFruitDisplay; // Drag objek UI Image Anda ke sini
    [SerializeField] private Sprite[] fruitSprites; // Drag 6 sprite Anda ke sini

    private bool isPlayerInRange = false;
    private bool isDisplaying = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            uiFruitDisplay.gameObject.SetActive(false); // Sembunyikan saat menjauh
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ShowRandomFruit();
        }
    }

    private void ShowRandomFruit()
    {
        if (fruitSprites.Length > 0)
        {
            int randomIndex = Random.Range(0, fruitSprites.Length);
            uiFruitDisplay.sprite = fruitSprites[randomIndex];
            uiFruitDisplay.gameObject.SetActive(true);

            // Memulai hitungan mundur untuk menutup gambar
            StartCoroutine(HideFruitAfterDelay(2f));
        }
    }

    // Fungsi tambahan untuk hitung mundur
    private IEnumerator HideFruitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        uiFruitDisplay.gameObject.SetActive(false);
        Debug.Log("Tombol E ditekan di dekat truk!");
    }
}