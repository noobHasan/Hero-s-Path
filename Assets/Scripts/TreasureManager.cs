using UnityEngine;
using System.Collections.Generic;

public class TreasureManager : MonoBehaviour
{
    public static TreasureManager instance;

    public GameObject treasurePrefab;
    public List<Transform> spawnPoints = new List<Transform>();
    public int totalTreasure;
    private int collectedTreasures;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SpawnTreasures();
    }

    public void CollectTreasure()
    {
        collectedTreasures++;

        Debug.Log("Treasure collected :" + collectedTreasures + "/" + totalTreasure);
    }

    void SpawnTreasures()
    {
        List<Transform> availablePoints = new List<Transform>(spawnPoints);

        for(int i = 0; i<totalTreasure; i++)
        {
            int randomIndex = Random.Range(0,availablePoints.Count);
            Transform spawnPoint = availablePoints[randomIndex];
            Instantiate(treasurePrefab,spawnPoint.position,Quaternion.identity);
            availablePoints.RemoveAt(randomIndex);
        }
    }
}
