using UnityEngine;



public class UIController : MonoBehaviour
{
    public GameObject taskPanel;
    public GameObject driverDocumentPanel;
    public DriverDocumentUI driverDocUI; // Referensi skrip di atas
    public MouseLook mouseLook;

    bool isOpen;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;

            if (isOpen && GameState.hasDriverDocument)
            {
                var truk = GerbangQueueManager.Instance.trukDiPosPengecekan;

                // Pastikan truk TIDAK NULL dan dataTruk di dalam truk tersebut JUGA TIDAK NULL
                if (truk != null && truk.dataDokumen != null) // Pastikan cek dataDokumen
                {
                    driverDocUI.UpdateDokumen(truk.dataDokumen); // Kirim dataDokumen ke UI
                }
                else
                {
                    Debug.LogWarning("Truk atau Data Truk belum siap!");
                }
            }

            taskPanel.SetActive(isOpen);
            driverDocumentPanel.SetActive(isOpen && GameState.hasDriverDocument);
            mouseLook.SetPaused(isOpen);
        }
    }


}