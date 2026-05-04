using UnityEngine;
using System.Collections;

public class PlantGrowth : MonoBehaviour
{
    public GameObject adultPrefab; // Masukkan model Palm dewasa ke sini
    public float timeToGrow = 10f; // Waktu tumbuh dalam detik (misal 10 detik)
    public float adultScale = 0.1f; // Skala untuk pohon dewasa

    void Start()
    {
        // Mulai menghitung waktu pertumbuhan saat bibit muncul
        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        // Tunggu selama waktu yang ditentukan
        yield return new WaitForSeconds(timeToGrow);

        // Munculkan pohon dewasa di posisi yang sama dengan bibit
        GameObject adultPlant = Instantiate(adultPrefab, transform.position, transform.rotation);

        // Atur skala pohon dewasa agar tidak raksasa
        adultPlant.transform.localScale = new Vector3(adultScale, adultScale, adultScale);

        // Hapus model bibit (dirinya sendiri)
        Destroy(gameObject);

        Debug.Log("Tanaman sudah dewasa!");
    }
}