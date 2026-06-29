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
    public float normalDistance = 200f;
    public float sprintDistance = 400f;
    public float distanceSpeed = 5f;

    [Header("Layer-Based Culling")]
    public float jarakBuahSawit = 30f;
    public float jarakPohonDetail = 100f;

    [Header("Fog Settings")]
    public bool useFog = true;
    public Color fogColor = new Color(0.5f, 0.5f, 0.5f);

    float xRotation = 0f;
    Camera cam;
    float[] distances = new float[32];

    public bool canLook = true;

    void Start()
    {
        cam = GetComponent<Camera>();
        UpdateCullDistances();

        if (useFog)
        {
            RenderSettings.fog = true;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogMode = FogMode.Linear;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!canLook) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);

        float targetFOV = isSprinting ? sprintFOV : baseFOV;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFOV, fovSpeed * Time.deltaTime);

        float targetDistance = isSprinting ? sprintDistance : normalDistance;
        cam.farClipPlane = Mathf.Lerp(cam.farClipPlane, targetDistance, distanceSpeed * Time.deltaTime);

        if (useFog)
        {
            RenderSettings.fogEndDistance = cam.farClipPlane;
            RenderSettings.fogStartDistance = cam.farClipPlane * 0.5f;
        }

        UpdateCullDistances();
    }

    void UpdateCullDistances()
    {
        distances[10] = jarakBuahSawit;
        distances[11] = jarakPohonDetail;
        cam.layerCullDistances = distances;
    }

    public void SetPaused(bool paused)
    {
        canLook = !paused;

        if (paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}