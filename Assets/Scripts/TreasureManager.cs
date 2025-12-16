using UnityEngine;

public class TreasureManager : MonoBehaviour
{
    public static TreasureManager instance;
    public int totalTreasure;
    private int collectedTreasures;
    private void Awake()
    {
        instance = this;
    }

    public void CollectTreasure()
    {
        collectedTreasures++;

        Debug.Log("Treasure collected :" + collectedTreasures + "/" + totalTreasure);
    }
}
