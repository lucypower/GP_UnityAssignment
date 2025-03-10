using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CorridorGenerator : MonoBehaviour
{
    CellularAutomata m_CA;

    private void Awake()
    {
        m_CA = GetComponent<CellularAutomata>();    
    }

    public List<List<Vector3Int>> GetAllRegionEdges(List<List<Vector3Int>> regions, int width, int height, int[,] grid) // takes getregions
    {
        List<List<Vector3Int>> edgeRegions = new List<List<Vector3Int>>();

        foreach (List<Vector3Int> region in regions)
        {
            List<Vector3Int> edges = GetRegionEdge(region, grid);
            edgeRegions.Add(edges);
        }

        return edgeRegions;
    }

    List<Vector3Int> GetRegionEdge(List<Vector3Int> region, int[,] grid)
    {
        List<Vector3Int> roomTiles = region;
        List<Vector3Int> edgeTiles = new List<Vector3Int>();

        foreach (Vector3Int tile in roomTiles)
        {
            for (int i = tile.x - 1; i <= tile.x + 1; i++)
            {
                for (int j = tile.z - 1; j <= tile.z + 1; j++)
                {
                    if ((i == tile.x || j == tile.z) && grid[i, j] == 1)
                    {
                        edgeTiles.Add(tile);                        
                    }
                }                
            }                 
        }

        return edgeTiles;
    }

    public void FindConnectingRooms(List<List<Vector3Int>> edges, int[,] CAgrid)
    {
        bool connectionFound = false;
        int shortestDistance = 0;

        Vector3Int bestA = new Vector3Int();
        Vector3Int bestB = new Vector3Int();

        foreach (List<Vector3Int> regionA in edges)
        {
            connectionFound = false;

            foreach (List<Vector3Int> regionB in edges)
            {
                if (regionA == regionB)
                {
                    continue;
                }

                for (int i = 0; i < regionA.Count; i++)
                {
                    for (int j = 0; j < regionB.Count; j++)
                    {
                        Vector3Int a = regionA[i];
                        Vector3Int b = regionB[j];

                        if (!regionA.Contains(b) && !regionB.Contains(a))
                        {
                            int distanceBetweenPoints = (int)(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.z - b.z, 2));

                            if (distanceBetweenPoints < shortestDistance || !connectionFound)
                            {
                                connectionFound = true;
                                shortestDistance = distanceBetweenPoints;

                                bestA = a;
                                bestB = b;
                            }
                        }                        
                    }
                }
            }

            if (connectionFound)
            {
                Debug.DrawLine(bestA, bestB, Color.green, 1000);

                if (bestA.x < bestB.x)
                {
                    ConnectRooms(bestA, bestB);
                }
                else if (bestA.x > bestB.x)
                {
                    ConnectRooms(bestB, bestA);
                }
                else
                {
                    if (bestA.y <= bestB.y)
                    {
                        ConnectRooms(bestA, bestB);
                    }
                    else
                    {
                        ConnectRooms(bestB, bestA);
                    }
                }
            }
        }
    }

    public void ConnectRooms(Vector3Int startCoord, Vector3Int endCoord)
    {
        float dx = endCoord.x - startCoord.x;
        float dz = endCoord.z - startCoord.z;

        float gradientOfLine = dz / dx;
        float c = endCoord.z - (gradientOfLine * endCoord.x);


        if (startCoord.x == endCoord.x)
        {
            for (int i = startCoord.z; i <= endCoord.z; i++)
            {
                m_CA.m_grid[startCoord.x, i] = 0;
            }
        }
        else if (startCoord.z == endCoord.z)
        {
            for (int i = startCoord.x; i <= endCoord.x; i++)
            {
                m_CA.m_grid[i, startCoord.z] = 0;
            }
        }
        else
        {
            for (int i = startCoord.x; i <= endCoord.x; i++)
            {
                int x = i;
                float z = (gradientOfLine * x) + c;

                z = Mathf.Round(z);

                if (!float.IsNaN(z))
                {
                    m_CA.m_grid[x, (int)z] = 0;
                    m_CA.m_grid[x + 1, (int)z] = 0;
                    m_CA.m_grid[x - 1, (int)z] = 0;

                    if (gradientOfLine >= 1.01)
                    {
                        m_CA.m_grid[x, (int)z + 1] = 0;
                        m_CA.m_grid[x, (int)z - 1] = 0;
                        m_CA.m_grid[x - 1, (int)z - 1] = 0;
                        m_CA.m_grid[x - 1, (int)z + 1] = 0;
                        m_CA.m_grid[x + 1, (int)z - 1] = 0;
                        m_CA.m_grid[x + 1, (int)z + 1] = 0;
                    }
                }
            }
        }        
    }
}
