using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DriverDocumentUI : MonoBehaviour
{
    public TruckDatabase database;

    public TMP_Text noRegis;
    public TMP_Text nama;
    public TMP_Text jenis;
    public TMP_Text perusahaan;
    public TMP_Text berat;

    public Toggle dokumenLolos;
    public Toggle dokumenTidakLolos;

    private TruckData currentTruck;

    void OnEnable()
    {
        GenerateNewTruck();
    }

    void GenerateNewTruck()
    {
        currentTruck = database.GetRandomTruck();
        SetUI(currentTruck);
    }

    void SetUI(TruckData data)
    {
        noRegis.text = data.noRegis;
        nama.text = data.namaPengemudi;
        jenis.text = data.jenisTruk;
        perusahaan.text = data.perusahaan;
        berat.text = data.beratTrukAsli.ToString();

        dokumenLolos.isOn = false;
        dokumenTidakLolos.isOn = false;

        if (data.kelayakan.ToLower() == "lolos")
        {
            dokumenLolos.isOn = true;
            dokumenTidakLolos.isOn = false;
        }
        else
        {
            dokumenLolos.isOn = false;
            dokumenTidakLolos.isOn = true;
        }
    }
}