using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


public class TruckSawitManager : MonoBehaviour
{


    [Header("Visual")]
    public GameObject tumpukanSawitVisual;



    private NavMeshAgent agent;



    private Transform targetQueue;



    private bool sedangAntre = false;



    private bool sedangMisi = false;





    void Awake()
    {

        agent =
            GetComponent<NavMeshAgent>();

    }






    void Start()
    {


        if (GerbangQueueManager.Instance != null)
        {

            GerbangQueueManager.Instance
                .DaftarkanTruk(this);

        }


    }








    public void IsiMuatanFull()
    {

        if (tumpukanSawitVisual != null)
        {

            tumpukanSawitVisual.SetActive(true);

        }


    }









    // =====================================
    // SISTEM QUEUE
    // =====================================


    public void PergiKeQueue(
        Transform titikQueue
    )
    {


        if (sedangMisi)
            return;



        targetQueue = titikQueue;


        sedangAntre = true;



        agent.isStopped = false;


        agent.SetDestination(
            targetQueue.position
        );



        Debug.Log(
            name +
            " menuju "
            + targetQueue.name
        );


    }

    void Update()
    {


        if (sedangAntre &&
           targetQueue != null)
        {


            if (!agent.pathPending &&
               agent.remainingDistance <=
               agent.stoppingDistance)
            {


                agent.isStopped = true;


                Debug.Log(
                    name +
                    " berhenti di "
                    + targetQueue.name
                );


            }

        }



    }


    // =====================================
    // MULAI MISI SETELAH Q0 KELUAR
    // =====================================


    public void MulaiMisi(
        List<Transform> waypoint
    )
    {


        if (sedangMisi)
            return;



        sedangMisi = true;

        sedangAntre = false;



        StartCoroutine(
            JalankanMisi(
                waypoint
            )
        );


    }

    IEnumerator JalankanMisi(
        List<Transform> p
    )
    {

        // Timbang 1 / Timbang 2

        int pilih =
            Random.Range(0, 2);



        yield return StartCoroutine(
            JalanKeTitik(
                p[pilih].position
            )
        );



        yield return new WaitForSeconds(5);

        // Drop sawit

        yield return StartCoroutine(
            JalanKeTitik(
                p[2].position
            )
        );

        yield return new WaitForSeconds(10);



        if (tumpukanSawitVisual != null)
        {

            tumpukanSawitVisual.SetActive(false);

        }


        // Gerbang keluar

        yield return StartCoroutine(
            JalanKeTitik(
                p[3].position
            )
        );





        // Destroy

        yield return StartCoroutine(
            JalanKeTitik(
                p[4].position
            )
        );



        Destroy(gameObject);



    }









    IEnumerator JalanKeTitik(
        Vector3 tujuan
    )
    {



        agent.isStopped = false;



        agent.SetDestination(
            tujuan
        );




        while (true)
        {


            if (!agent.pathPending &&
               agent.remainingDistance <=
               agent.stoppingDistance)
            {

                break;

            }


            yield return null;


        }



    }





}