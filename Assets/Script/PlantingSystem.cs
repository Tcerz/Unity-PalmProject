using UnityEngine;

public class PlantingSystem : MonoBehaviour
{
    public float reachDistance = 5f; // Jarak maksimal tangan bisa menjangkau lahan
    public LayerMask landLayer;      // Opsi tambahan: pilih layer lahan agar lebih akurat

    [Header("Inventory Reference")]
    public PlayerInventory inventory; // Tarik script PlayerInventory kamu ke sini

    void Update()
    {
        // Mengecek input Klik Kanan (Mouse 1)
        if (Input.GetMouseButtonDown(1))
        {
            TryPlanting();
        }
    }

    void TryPlanting()
    {
        // Membuat Ray tepat dari tengah layar kamera
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, reachDistance))
        {
            // Cek apakah objek yang terkena memiliki tag "Lahan"
            if (hit.collider.CompareTag("Lahan"))
            {
                // Cek apakah kita punya item untuk ditanam di tangan
                if (inventory != null && inventory.currentItem != null)
                {
                    Plant(hit.collider.gameObject, hit.point);
                }
                else
                {
                    Debug.Log("Tangan kosong! Tidak ada yang bisa ditanam.");
                }
            }
        }
    }

    void Plant(GameObject land, Vector3 spawnPos)
    {
        // Munculkan bibit (Palm Kecil)
        GameObject bibitBaru = Instantiate(inventory.currentItem.prefabToPlant, land.transform.position, Quaternion.identity);

        bibitBaru.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);

        // Matikan collider lahan
        land.GetComponent<Collider>().enabled = false;
    }
}