using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    [Header("FOV Settings")]
    public float baseFOV = 60f;
    public float sprintFOV = 80f;
    public float fovSpeed = 5f;

    [Header("Render Distance Settings")]
    public float normalDistance = 200f; // Jarak pandang normal (pendekkan ke 200 agar terasa bedanya)
    public float sprintDistance = 400f;
    public float distanceSpeed = 5f;

    [Header("Layer-Based Culling")]
    public float jarakBuahSawit = 30f;   // Hanya muncul jika sangat dekat
    public float jarakPohonDetail = 100f;

    [Header("Fog Settings (Smooth Rendering)")]
    public bool useFog = true;
    public Color fogColor = new Color(0.5f, 0.5f, 0.5f);

    float xRotation = 0f;
    Camera cam;
    float[] distances = new float[32];

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = GetComponent<Camera>();

        // Inisialisasi awal
        UpdateCullDistances();

        // Pengaturan Kabut agar transisi objek hilang lebih halus
        if (useFog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogMode = FogMode.Linear;
        }
    }

    void Update()
    {
        // --- MOUSE LOOK ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        // --- DYNAMIC FOV & DISTANCE ---
        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        float targetFOV = isSprinting ? sprintFOV : baseFOV;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, fovSpeed * Time.deltaTime);

        float targetDistance = isSprinting ? sprintDistance : normalDistance;
        cam.farClipPlane = Mathf.Lerp(cam.farClipPlane, targetDistance, distanceSpeed * Time.deltaTime);

        // Update Fog agar sinkron dengan Far Clip Plane
        if (useFog)
        {
            RenderSettings.fogEndDistance = cam.farClipPlane;
            RenderSettings.fogStartDistance = cam.farClipPlane * 0.5f;
        }

        // Opsional: Panggil ini jika kamu sering mengubah jarak di Inspector saat Play Mode
        UpdateCullDistances();
    }

    void UpdateCullDistances()
    {
        distances[10] = jarakBuahSawit;
        distances[11] = jarakPohonDetail;
        cam.layerCullDistances = distances;
    }
}