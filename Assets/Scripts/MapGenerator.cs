using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    CellularAutomata m_CA;
    RegionIdentifier m_RI;
    CorridorGenerator m_CG;
    MarchingSquares m_MS;
    GameManager m_GM;

    [SerializeField] CameraController[] m_cameras;
    [SerializeField] Camera m_mapCamera;

    int corridorIteration = 0;

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
        
    }

    public void GenerateMap()
    {
        m_CA.StartCA();

        GetRegionsAndEdges();

        m_MS.MarchSquares(m_CA.m_width, m_CA.m_height, m_CA.m_grid);


        if (m_MS.m_combineMeshes)
        {
            m_MS.CombinePBMeshes();
        }
        m_CA.FindOpenSpaces();

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

    void GetRegionsAndEdges()
    {
        List<List<Vector3Int>> regions = m_RI.GetRegions(m_CA.m_width, m_CA.m_height, m_CA.m_grid);

        List<List<Vector3Int>> regionsToRemove = new List<List<Vector3Int>>();

        foreach (List<Vector3Int> region in regions)
        {
            if (region.Count < 35)
            {
                for (int i = 0; i < region.Count; i++)
                {
                    m_CA.m_grid[region[i].x, region[i].z] = 1;
                }

                region.Clear();
                regionsToRemove.Add(region);
            }
        }

        for (int i = 0; i < regionsToRemove.Count; i++)
        {
            regions.Remove(regionsToRemove[i]);
        }

        List<List<Vector3Int>> edges = m_CG.GetAllRegionEdges(regions, m_CA.m_width, m_CA.m_height, m_CA.m_grid);

        if (regions.Count > 1)
        {
            m_CG.FindConnectingRooms(edges, m_CA.m_grid);
            m_CA.FinaliseGrid();
        }

        corridorIteration++;
        Debug.Log(regions.Count);

        if (regions.Count > 1)
        {
            regions.Clear();
            edges.Clear();

            if (corridorIteration <= 3)
            {
                GetRegionsAndEdges();
            }
        }
    }   
}
