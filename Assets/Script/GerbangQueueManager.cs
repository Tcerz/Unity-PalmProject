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


            Debug.Log(
                truk.name +
                " masuk antrean"
            );

        }

    }








    void UpdateQueue()
    {


        for (int i = 0; i < antrean.Count; i++)
        {


            if (i >= queuePoints.Count)
                break;



            antrean[i]
                .PergiKeQueue(
                    queuePoints[i]
                );

        }


    }



    IEnumerator KeluarkanTruk()
    {


        sedangMemproses = true;



        if (antrean.Count > 0)
        {


            TruckSawitManager truk =
                antrean[0];



            antrean.RemoveAt(0);



            UpdateQueue();



            yield return new WaitForSeconds(1);



            if (truk != null)
            {


                truk.MulaiMisi(
                    waypointMisi
                );


            }


        }



        yield return new WaitForSeconds(3);



        sedangMemproses = false;


    }

    public void LolosPemeriksaan()
    {
        Debug.Log("TRUK DINYATAKAN LOLOS");
    }


    public void TolakPemeriksaan()
    {
        Debug.Log("TRUK DINYATAKAN TIDAK LOLOS");
    }


}