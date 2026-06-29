using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class KebunManager : MonoBehaviour
{

    public static KebunManager instance;

    [Header("Spawn Truk")]
    public GameObject prefabTruk;

    public Transform spawnPointTruk;

    [Header("Referensi")]
    public KontrolGerbang referensiGerbangWorld;

    [Header("Stop Point")]
    public List<Transform> listStopPoints =
        new List<Transform>();

    private List<SawitGrowth> daftarAntreanPohon =
        new List<SawitGrowth>();


    private bool sedangProsesTruk = false;

    void Awake()
    {
        instance = this;
    }

    public void TambahLaporanBerbuah(
        SawitGrowth pohon
    )
    {

        if (!daftarAntreanPohon.Contains(pohon))
        {

            daftarAntreanPohon.Add(pohon);


            Debug.Log(
                "<color=cyan>Pohon Berbuah = "
                + daftarAntreanPohon.Count
                + "</color>"
            );

        }

        if (daftarAntreanPohon.Count >= 3 &&
           !sedangProsesTruk)
        {

            StartCoroutine(
                SiklusSpawnTruk()
            );

        }

    }

    IEnumerator SiklusSpawnTruk()
    {

        sedangProsesTruk = true;

        while (daftarAntreanPohon.Count >= 3)
        {


            int jumlahTruk =
                FindObjectsOfType<TruckSawitManager>()
                .Length;



            if (jumlahTruk >= 4)
            {

                yield return new WaitForSeconds(5);

                continue;

            }

            SpawnTruk();

            for (int i = 0; i < 3; i++)
            {

                if (daftarAntreanPohon.Count > 0)
                {

                    daftarAntreanPohon[0]
                        .ResetPohon();


                    daftarAntreanPohon.RemoveAt(0);

                }

            }


            yield return new WaitForSeconds(20);

        }


        sedangProsesTruk = false;

    }


    void SpawnTruk()
    {


        GameObject trukBaru =
            Instantiate(
                prefabTruk,
                spawnPointTruk.position,
                spawnPointTruk.rotation
            );



        TruckSawitManager truck =
            trukBaru.GetComponent<TruckSawitManager>();

        if (truck != null)
        {

            truck.IsiMuatanFull();

            if (GerbangQueueManager.Instance != null)
            {

                GerbangQueueManager.Instance
                    .DaftarkanTruk(truck);

            }

            Debug.Log(
                "Truk baru masuk antrean gerbang"
            );

        }

        else
        {

            Debug.LogError(
                "Prefab Truk tidak memiliki TruckSawitManager!"
            );
        }
    }
}