using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class TaskPanelController : MonoBehaviour
{
    [Header("UI Reference Buah")]
    public Toggle checkBagus;
    public Toggle checkKurang;
    public Toggle checkTidakBagus;

    [Header("UI Reference Keputusan")]
    public Toggle checkLolos;
    public Toggle checkTidakLolos;

    [Header("Lainnya")]
    public TMP_InputField inputBerat;
    public Button doneButton;
    public Animator gateAnimator;

    void Start()
    {
        doneButton.onClick.AddListener(OnDoneClicked);
    }

    void OnEnable()
    {
        var truk = GerbangQueueManager.Instance.trukDiPosPengecekan;
        if (truk != null && inputBerat != null)
        {
            inputBerat.text = truk.dataTruk.beratTrukAsli.ToString();
        }
    }

    void OnDoneClicked()
    {
        var truk = GerbangQueueManager.Instance.trukDiPosPengecekan;
        if (truk == null) return;

        PengecekanMuatan cek = truk.GetComponentInChildren<PengecekanMuatan>();
        string buahAsli = cek != null ? cek.statusBuahTerpilih : "bagus";
        float beratAsli = truk.dataTruk.beratTrukAsli;

        string jenisBuahDipilih = checkBagus.isOn ? "bagus" : (checkKurang.isOn ? "kurang" : "tidak bagus");
        float beratDokumen = float.TryParse(inputBerat.text, out float b) ? b : 0f;

        // Menentukan label status berat untuk laporan
        string statusBerat = (beratDokumen == beratAsli) ? "Berat Tepat" : (beratDokumen < beratAsli ? "Berat Kurang" : "Berat Lebih");

        int poin = 0;
        string deskripsiPoin = "";

        if (checkLolos.isOn)
        {
            if (jenisBuahDipilih != buahAsli)
            {
                poin = -500;
                deskripsiPoin = "Salah ID Buah (" + jenisBuahDipilih + "), Aslinya: " + buahAsli;
            }
            else
            {
                // Menghitung poin berdasarkan jenis dan logika berat
                if (jenisBuahDipilih == "bagus")
                {
                    poin = (beratDokumen == beratAsli) ? 1000 : (beratDokumen < beratAsli ? 1500 : 500);
                }
                else if (jenisBuahDipilih == "kurang")
                {
                    poin = (beratDokumen == beratAsli) ? 200 : (beratDokumen < beratAsli ? 500 : -500);
                }
                else // tidak bagus
                {
                    poin = (beratDokumen == beratAsli) ? -1000 : (beratDokumen < beratAsli ? -500 : -2000);
                }
                deskripsiPoin = "Lolos Buah " + jenisBuahDipilih + " (" + statusBerat + ")";
            }

            GameManager.Instance.TambahPoin(poin, deskripsiPoin);

            if (gateAnimator != null)
            {
                gateAnimator.SetTrigger("Pencet");
                StartCoroutine(TutupGerbangOtomatis());
            }

            GerbangQueueManager.Instance.LolosPemeriksaan();
        }
        else
        {
            // Logika Menolak
            if (buahAsli != "bagus" || beratDokumen != beratAsli)
            {
                poin = 100;
                deskripsiPoin = "Menolak Buah " + buahAsli + " (Tindakan Benar)";
            }
            else
            {
                poin = -200;
                deskripsiPoin = "Salah Tolak Buah Bagus";
            }

            GameManager.Instance.TambahPoin(poin, deskripsiPoin);
            GerbangQueueManager.Instance.TolakPemeriksaan();
        }

        gameObject.SetActive(false);
    }

    IEnumerator TutupGerbangOtomatis()
    {
        yield return new WaitForSeconds(3f);
        if (gateAnimator != null)
        {
            gateAnimator.SetTrigger("Tutup");
        }
    }
}