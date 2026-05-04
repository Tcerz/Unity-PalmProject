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
    public float normalDistance = 1000f;
    public float sprintDistance = 1500f;
    public float distanceSpeed = 5f;

    [Header("Directional Culling Settings")]
    public Transform terrain; // drag terrain ke sini
    public float maxViewAngle = 100f; // sudut pandang depan

    float xRotation = 0f;
    Camera cam;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = GetComponent<Camera>();

        cam.fieldOfView = baseFOV;
        cam.farClipPlane = normalDistance;
    }

    void Update()
    {
        // Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        // FOV
        float targetFOV = Input.GetKey(KeyCode.LeftShift) ? sprintFOV : baseFOV;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, fovSpeed * Time.deltaTime);

        // Render Distance
        float targetDistance = Input.GetKey(KeyCode.LeftShift) ? sprintDistance : normalDistance;
        cam.farClipPlane = Mathf.Lerp(cam.farClipPlane, targetDistance, distanceSpeed * Time.deltaTime);

        // 🔥 Direction-based terrain rendering
        if (terrain != null)
        {
            Vector3 dirToTerrain = (terrain.position - cam.transform.position).normalized;
            float angle = Vector3.Angle(cam.transform.forward, dirToTerrain);

            // aktif hanya kalau di depan kamera
            terrain.gameObject.SetActive(angle < maxViewAngle);
        }
    }
}