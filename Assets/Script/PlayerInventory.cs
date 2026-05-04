using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public ItemData currentItem; // Drag & drop data bibit ke sini di Inspector
    public Transform handAnchor; // Drag & drop objek HandAnchor ke sini

    void Start()
    {
        UpdateHandVisual();
    }

    void UpdateHandVisual()
    {
        foreach (Transform child in handAnchor)
        {
            Destroy(child.gameObject);
        }

        if (currentItem != null && currentItem.visualInHand != null)
        {
            GameObject itemBaru = Instantiate(currentItem.visualInHand, handAnchor);

            // Karena kamu sudah mengecilkan HandAnchor di Inspector, 
            // biarkan scale item ini mengikuti parent-nya (1,1,1 relatif terhadap parent)
            itemBaru.transform.localScale = Vector3.one;

            // Reset posisi ke titik pusat HandAnchor
            itemBaru.transform.localPosition = Vector3.zero;
            itemBaru.transform.localRotation = Quaternion.identity;

            // Memastikan tidak ada tabrakan antara bibit di tangan dengan player
            if (itemBaru.GetComponent<Collider>())
            {
                itemBaru.GetComponent<Collider>().enabled = false;
            }
        }
    }
}