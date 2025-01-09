using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    CellularAutomata m_CA;
    MapGenerator m_MG;

    [SerializeField] GameObject m_player;
    [SerializeField] GameObject m_pickup;

    [Header("Player Stats")]

    [HideInInspector] public int m_pickupsToSpawn;
    [HideInInspector] public List<GameObject> m_pickupsInMap = new List<GameObject>();
    [HideInInspector] public int m_pickupsCollected;

    [SerializeField] GameObject m_gameOverUI;

    private void Awake()
    {
        m_CA = GetComponent<CellularAutomata>();   
        m_MG = GetComponent<MapGenerator>();

        m_pickupsToSpawn = 1;
    }

    public void GenerateGrid()
    {
        m_MG.GenerateMap();
    }

    private void Update()
    {
        if (m_pickupsCollected == m_pickupsToSpawn && m_pickupsInMap.Count == 0)
        {
            m_gameOverUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("GameWin");
            Time.timeScale = 0;
        }
    }

    public void SpawnPickups()
    {
        m_pickupsToSpawn = (m_CA.m_width + m_CA.m_height) / 20;

        for (int i = 0; i < m_pickupsToSpawn; i++)
        {
            int random = Random.Range(0, m_CA.m_openSpaces.Count - 1);

            m_pickupsInMap.Add(Instantiate(m_pickup, m_CA.m_openSpaces[random], Quaternion.identity));
            m_CA.m_openSpaces.RemoveAt(random);
        }
    }

    public void RespawnHiddenPickups(GameObject originalPickup)
    {
        m_pickupsInMap.Remove(originalPickup);

        int random = Random.Range(0, m_CA.m_openSpaces.Count - 1);

        m_pickupsInMap.Add(Instantiate(m_pickup, m_CA.m_openSpaces[random], Quaternion.identity));
        m_CA.m_openSpaces.RemoveAt(random);

        Destroy(originalPickup);
    }

    public void SpawnPlayer()
    {
        int random = Random.Range(0, m_CA.m_openSpaces.Count - 1);

        Instantiate(m_player, m_CA.m_openSpaces[random], Quaternion.identity);
        m_CA.m_openSpaces.RemoveAt(random);

        CameraController miniMapCamera = GameObject.Find("MiniMap Camera").GetComponent<CameraController>();
        miniMapCamera.FindPlayer();

        CameraController playerCamera = GameObject.Find("Main Camera").GetComponent<CameraController>();
        playerCamera.FindPlayer();
    }
}
