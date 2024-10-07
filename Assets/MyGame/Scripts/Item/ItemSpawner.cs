using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("ThinhLe/ItemSpawner")]

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coin, health;

    public void DropItem()
    {
        int randomNum = Random.Range(1, 10);

        if(randomNum == 1)
        {
            Instantiate(health, transform.position, Quaternion.identity);
        }
        else
        {
            int randomAmountOfCoin = Random.Range(1, 10);
            if(randomAmountOfCoin == 2 || randomAmountOfCoin == 3)
            {
                for (int i = 0; i < randomAmountOfCoin; i++)
                {
                    Instantiate(coin, transform.position, Quaternion.identity);
                }
            }
            else
            {
                Instantiate(coin, transform.position, Quaternion.identity);
            }

        }
    }
}
