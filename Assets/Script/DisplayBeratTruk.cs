using UnityEngine;
using TMPro; // WAJIB: Ini untuk mengakses komponen TextMeshPro

public class DisplayBeratTruk : MonoBehaviour
{
    // Gunakan TextMeshProUGUI untuk komponen Text (TMP)
    public TextMeshProUGUI textTampilanBerat;

    public void UpdateBerat(float berat)
    {
        if (textTampilanBerat != null)
        {
            textTampilanBerat.text = "BERAT: " + berat.ToString("F0") + " kg";
        }
    }

    public void ResetTampilan()
    {
        if (textTampilanBerat != null)
            textTampilanBerat.text = "BERAT: 0 kg";
    }
}