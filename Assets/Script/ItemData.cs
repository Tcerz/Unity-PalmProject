using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Farming/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public GameObject prefabToPlant; // Pohon yang akan muncul saat ditanam
    public GameObject visualInHand;  // Model yang muncul di tangan
}