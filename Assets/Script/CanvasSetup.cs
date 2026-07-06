using UnityEngine;

public class CanvasSetup : MonoBehaviour
{
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        // Mencari kamera utama yang sedang aktif di Scene
        canvas.worldCamera = Camera.main;
    }
}