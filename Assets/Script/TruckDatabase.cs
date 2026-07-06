using System.Collections.Generic;
using UnityEngine;

public class TruckDatabase : MonoBehaviour
{
    public TextAsset csvFile;
    public List<TruckData> trucks = new List<TruckData>();

    void Start()
    {
        LoadCSV();
    }

    void LoadCSV()
    {
        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] data = lines[i].Trim().Split(',');

            if (data.Length < 6) continue;

            TruckData truck = new TruckData
            {
                noRegis = data[0].Trim(),
                namaPengemudi = data[1].Trim(),
                jenisTruk = data[2].Trim(),
                perusahaan = data[3].Trim(),
                beratTrukAsli = int.Parse(data[4].Trim()),
                kelayakan = data[5].Trim()
            };

            trucks.Add(truck);
        }

        Debug.Log("Truck loaded: " + trucks.Count);
    }

    public TruckData GetRandomTruck()
    {
        return trucks[Random.Range(0, trucks.Count)];
    }
}