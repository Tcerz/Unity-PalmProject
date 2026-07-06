using UnityEngine;

using System.Collections;

using System.Collections.Generic;





public class GerbangQueueManager : MonoBehaviour

{



    public static GerbangQueueManager Instance;





    [Header("Gerbang")]

    public KontrolGerbang skripGerbang;







    [Header("Queue")]

    public List<Transform> queuePoints =

        new List<Transform>();







    [Header("Rute Setelah Masuk")]

    public List<Transform> waypointMisi =

        new List<Transform>();







    private List<TruckSawitManager> antrean =

        new List<TruckSawitManager>();







    private bool sedangMemproses = false;
    public TruckSawitManager trukDiPosPengecekan;
    

    public DisplayBeratTruk skripDisplayBox;







    void Awake()

    {

        Instance = this;

    }





    void Update()

    {



        if (skripGerbang == null)

            return;







        if (skripGerbang.sedangTerbuka)

        {



            if (!sedangMemproses)

            {



                StartCoroutine(

                    KeluarkanTruk()

                );



            }



        }



    }



    public void DaftarkanTruk(

        TruckSawitManager truk

    )

    {





        if (!antrean.Contains(truk))

        {



            antrean.Add(truk);





            UpdateQueue();





            //Debug.Log( truk.name + " masuk antrean");



        }



    }



    IEnumerator KeluarkanTruk()

    {

        sedangMemproses = true;



        if (antrean.Count > 0)

        {

            // 1. Ambil truk terdepan

            TruckSawitManager truk = antrean[0];

            antrean.RemoveAt(0);



            // 2. Truk q0 langsung menjalankan misi TANPA menunggu 3 detik

            truk.MulaiMisi(waypointMisi);



            // 3. Beri jeda 3 detik sebelum antrean di belakangnya bergerak

            yield return new WaitForSeconds(3f);



            // 4. Baru perintahkan sisanya untuk maju

            UpdateQueue();

        }



        yield return new WaitForSeconds(1f); // Durasi cooldown pemrosesan

        sedangMemproses = false;

    }



    void UpdateQueue()

    {

        // Cukup perintahkan setiap truk yang tersisa ke posisi antrean baru mereka

        for (int i = 0; i < antrean.Count; i++)

        {

            if (i >= queuePoints.Count) break;

            antrean[i].PergiKeQueue(queuePoints[i]);

        }

    }



    public void SetTrukDiPos(TruckSawitManager truk)

    {

        trukDiPosPengecekan = truk;



        if (skripDisplayBox != null)

        {

            skripDisplayBox.UpdateBerat(truk.dataTruk.beratTrukAsli);

        }

    }



    public void LolosPemeriksaan()

    {

        if (trukDiPosPengecekan != null)

        {

            trukDiPosPengecekan.telahLolosPemeriksaan = true;

            trukDiPosPengecekan = null;



            // Tambahkan ini agar kotak kembali ke 0 setelah truk lolos

            if (skripDisplayBox != null) skripDisplayBox.ResetTampilan();

        }

    }



    public void TolakPemeriksaan()

    {

        if (trukDiPosPengecekan != null)

        {

            trukDiPosPengecekan.batalMisi = true;

            trukDiPosPengecekan = null;



            // Tambahkan ini juga agar kotak kembali ke 0

            if (skripDisplayBox != null) skripDisplayBox.ResetTampilan();

        }

    }





}