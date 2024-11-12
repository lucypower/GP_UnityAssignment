using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    CellularAutomata m_CA;

    [SerializeField] GameObject m_player;

    private void Awake()
    {
        m_CA = GetComponent<CellularAutomata>();
    }

    public void SpawnPlayer()
    {
        int random = Random.Range(0, m_CA.m_openSpaces.Count - 1);

        Instantiate(m_player, m_CA.m_openSpaces[random], Quaternion.identity);
    }
}
