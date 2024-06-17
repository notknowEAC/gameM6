using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goinCoinPrefab;

    public void DropItem()
    {
        Instantiate(goinCoinPrefab, transform.position, Quaternion.identity);
    }
}
