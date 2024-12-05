using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    CellularAutomata m_CA;

    [SerializeField] GameObject m_player;
    [SerializeField] GameObject m_pickup;

    [Header("Player Stats")]

    [HideInInspector] public int m_pickupsToSpawn;
    [HideInInspector] public List<GameObject> m_pickupsInMap = new List<GameObject>();
    [HideInInspector] public int m_pickupsCollected;

    private void Awake()
    {
        m_CA = GetComponent<CellularAutomata>();        

        m_pickupsToSpawn = (m_CA.m_width + m_CA.m_height) / 20;
    }

    private void Update()
    {
        if (m_pickupsCollected == m_pickupsToSpawn && m_pickupsInMap.Count == 0)
        {
            Debug.Log("GameWin");
            Time.timeScale = 0;
        }
    }

    public void SpawnPickups()
    {
        for (int i = 0; i < m_pickupsToSpawn; i++)
        {
            int random = Random.Range(0, m_CA.m_openSpaces.Count - 1);

            m_pickupsInMap.Add(Instantiate(m_pickup, m_CA.m_openSpaces[random], Quaternion.identity));
            m_CA.m_openSpaces.RemoveAt(random);
        }
    }

    public void SpawnPlayer()
    {
        int random = Random.Range(0, m_CA.m_openSpaces.Count - 1);

        Instantiate(m_player, m_CA.m_openSpaces[random], Quaternion.identity);
        m_CA.m_openSpaces.RemoveAt(random);

        CameraController camera = GameObject.Find("MiniMap Camera").GetComponent<CameraController>();
        camera.FindPlayer();
    }
}
