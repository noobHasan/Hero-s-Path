using UnityEngine;
using DG.Tweening;

public class Treasure : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Code to handle treasure collection
            Debug.Log("Treasure collected!");
            TreasureManager.instance.CollectTreasure();
            //Destroy(gameObject);

            transform.DOScale(Vector3.zero, 0.5f).OnComplete(() => 
            {
                Destroy(gameObject);
            });
        }
    }
}
