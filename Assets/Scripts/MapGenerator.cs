using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    CellularAutomata m_CA;
    RegionIdentifier m_RI;
    CorridorGenerator m_CG;
    MarchingSquares m_MS;
    GameManager m_GM;

    [SerializeField] CameraController[] m_cameras;
    [SerializeField] Camera m_mapCamera;

    private void Awake()
    {
        m_CA = GetComponent<CellularAutomata>();
        m_RI = GetComponent<RegionIdentifier>();
        m_CG = GetComponent<CorridorGenerator>();
        m_MS = GetComponent<MarchingSquares>();
        m_GM = GetComponent<GameManager>();
    }

    private void Start()
    {
        m_CA.StartCA();

        List<List<Vector3Int>> regions = m_RI.GetRegions(m_CA.m_width, m_CA.m_height, m_CA.m_grid);
        List<List<Vector3Int>> edges = m_CG.GetAllRegionEdges(regions, m_CA.m_width, m_CA.m_height, m_CA.m_grid);

        if (regions.Count > 1)
        {
            m_CG.FindConnectingRooms(edges, m_CA.m_grid);
            m_CA.FinaliseGrid();
        }

        m_MS.MarchSquares(m_CA.m_width, m_CA.m_height, m_CA.m_grid);

        if (m_MS.m_combineMeshes)
        {
            m_MS.CombinePBMeshes();
        }

        m_mapCamera.transform.position = new Vector3((m_CA.m_width / 2) - 0.5f, (m_CA.m_width + m_CA.m_height) / 2, (m_CA.m_height / 2) - 0.5f);
    }

    void OtherStuff()
    {
        //GameManager gM = GetComponent<GameManager>();
        //gM.SpawnPlayer();
        //gM.SpawnPickups();

        foreach (CameraController cam in m_cameras)
        {
            cam.FindPlayer();
        }

        
    }
}
