using UnityEngine;
using TMPro;

public class TreasureUI : MonoBehaviour
{
    public TextMeshProUGUI treasureText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        treasureText.text = "Treasures: " + TreasureManager.instance.collectedTreasures + " / " + TreasureManager.instance.totalTreasure;
    }

    public void UpdateTreasureText(int collected, int total)
    {
        treasureText.text = "Treasures: " + collected + " / " + total;

        if( TreasureManager.instance.collectedTreasures == TreasureManager.instance.totalTreasure)
        {
            treasureText.color = Color.green;
        }
    }
}
