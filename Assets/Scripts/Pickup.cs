using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
            
            gm.m_pickupsInMap.Remove(gameObject);
            gm.m_pickupsCollected++;

            Destroy(gameObject);
        }

        if (other.CompareTag("Wall"))
        {
            GameManager gm = GameObject.Find("Manager").GetComponent<GameManager>();

            gm.RespawnHiddenPickups(gameObject);
        }
    }
}
