using UnityEngine;
using System.Collections;

public class KontrolPemeriksaan : MonoBehaviour
{

    public enum StatusPemeriksaan
    {
        Lolos,
        TidakLolos
    }



    [Header("Identitas Tombol")]
    public StatusPemeriksaan status;



    [Header("Komponen Gerbang (khusus Lolos)")]
    public Animator animatorGerbang;



    [Header("Teks Petunjuk")]
    public GameObject teksPetunjuk;



    [Header("Status")]
    public bool sedangAktif = false;



    private bool playerBisaTekan = false;



    void Update()
    {

        if (playerBisaTekan &&
           Input.GetKeyDown(KeyCode.E))
        {

            TekanTombol();

        }

    }





    void TekanTombol()
    {

        if (sedangAktif)
            return;



        sedangAktif = true;



        if (status == StatusPemeriksaan.Lolos)
        {

            Debug.Log("Pemeriksaan Lolos");


            BukaGerbang();



            GerbangQueueManager.Instance
                .LolosPemeriksaan();


        }


        else if (status == StatusPemeriksaan.TidakLolos)
        {

            Debug.Log("Pemeriksaan Tidak Lolos");


            GerbangQueueManager.Instance
                .TolakPemeriksaan();

        }



    }





    void BukaGerbang()
    {

        if (animatorGerbang != null)
        {

            animatorGerbang
                .SetTrigger("Pencet");


            StartCoroutine(
                TutupOtomatis()
            );

        }

    }





    IEnumerator TutupOtomatis()
    {

        yield return new WaitForSeconds(5f);



        if (animatorGerbang != null)
        {

            animatorGerbang
                .SetTrigger("Tutup");

        }



        sedangAktif = false;

    }





    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            playerBisaTekan = true;


            if (teksPetunjuk != null)
                teksPetunjuk.SetActive(true);

        }

    }





    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {

            playerBisaTekan = false;


            if (teksPetunjuk != null)
                teksPetunjuk.SetActive(false);

        }

    }

}