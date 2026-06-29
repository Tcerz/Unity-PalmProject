using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject taskPanel;
    public GameObject driverDocumentPanel;
    public MouseLook mouseLook;

    bool isOpen;

    void Start()
    {
        isOpen = false;

        taskPanel.SetActive(false);
        driverDocumentPanel.SetActive(false);

        mouseLook.SetPaused(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;

            taskPanel.SetActive(isOpen);

            driverDocumentPanel.SetActive(
                isOpen && GameState.hasDriverDocument
            );

            mouseLook.SetPaused(isOpen);
        }
    }
}