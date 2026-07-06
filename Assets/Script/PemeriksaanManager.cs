using UnityEngine;

public class PemeriksaanManager : MonoBehaviour
{
    public int totalPoin = 1000;

    public void HitungSkor(string jenisBuah, float beratAsli, float beratDokumen, bool isLolos)
    {
        if (!isLolos) return; // Jika tidak lolos, mungkin ada sistem poin berbeda

        // Logika Penilaian
        int poin = 0;

        // Cek kondisi berat
        string kondisiBerat = "akurat";
        if (beratDokumen < beratAsli) kondisiBerat = "ringan";
        else if (beratDokumen > beratAsli) kondisiBerat = "besar";

        // Tabel Penilaian
        if (jenisBuah == "bagus")
        {
            if (kondisiBerat == "akurat") poin = 1000;
            else if (kondisiBerat == "ringan") poin = 1500;
            else poin = 500;
        }
        else if (jenisBuah == "kurang")
        {
            if (kondisiBerat == "akurat") poin = 200;
            else if (kondisiBerat == "ringan") poin = 500;
            else poin = -500;
        }
        else if (jenisBuah == "tidak bagus")
        {
            if (kondisiBerat == "akurat") poin = -1000;
            else if (kondisiBerat == "ringan") poin = -500;
            else poin = -2000;
        }

        totalPoin += poin;
        Debug.Log("Pemeriksaan selesai. Skor akhir: " + totalPoin);
    }
}