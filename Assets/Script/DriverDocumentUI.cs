using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DriverDocumentUI : MonoBehaviour
{
    public TMP_Text noRegis;
    public TMP_Text nama;
    public TMP_Text jenis;
    public TMP_Text perusahaan;
    public TMP_Text berat;
    public Toggle dokumenLolos;
    public Toggle dokumenTidakLolos;

    // Hapus OnEnable dan GenerateNewTruck agar tidak auto-generate saat dibuka

    // Di dalam DriverDocumentUI.cs
    public void UpdateDokumen(TruckData data)
    {
        if (data == null) return;

        noRegis.text = data.noRegis;
        nama.text = data.namaPengemudi;
        jenis.text = data.jenisTruk;
        perusahaan.text = data.perusahaan;

        // Pastikan ini menggunakan 'data' yang dikirim dari UIController (yaitu dataDokumen)
        berat.text = data.beratTrukAsli.ToString() + " kg";
    }
}