using UnityEngine;

public class DriverInteract : MonoBehaviour
{
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.F;

    public Transform player;
    public GameObject documentObject;

    private bool canInteract = false;

    void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        canInteract = distance <= interactDistance;

        if (canInteract)
        {
            if (Input.GetKeyDown(interactKey))
            {
                TakeDocument();
            }
        }
    }

    void TakeDocument()
    {
        if (GameState.hasDriverDocument) return;

        GameState.hasDriverDocument = true;

        if (documentObject != null)
            documentObject.SetActive(false);

        Debug.Log("Dokumen diambil dari supir");
    }
}