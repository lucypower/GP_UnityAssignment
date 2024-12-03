using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class RegionIdentifier : MonoBehaviour
{
    CellularAutomata m_CA;

    private void Awake()
    {
        m_CA = GetComponent<CellularAutomata>();
    }

    List<List<Vector2>> GetRegions()
    {
        List<List<Vector2>> regions = new List<List<Vector2>>();
        int[,] tilesChecked = new int[m_CA.m_width, m_CA.m_height];

        for (int i = 0; i < m_CA.m_width; i++)
        {
            for (int j = 0; j < m_CA.m_height; j++)
            {
               if (tilesChecked[i, j] == 0 && m_CA.m_grid[i, j] == 0)
                {
                    List<Vector2> region = GetRegionArea(i, j);
                    regions.Add(region);

                    foreach (Vector2 tile in region)
                    {
                        tilesChecked[(int)tile.x, (int)tile.y] = 1;
                    }
                }
            }
        }

        return regions;
    }

    List<Vector2> GetRegionArea(int x, int y)
    {
        List<Vector2> tiles = new List<Vector2>();
        int[,] tilesChecked = new int[m_CA.m_width, m_CA.m_height];

        Queue<Vector2> tilesToCheck = new Queue<Vector2>();
        tilesToCheck.Enqueue(new Vector2(x, y));
        tilesChecked[x, y] = 1;

        while (tilesToCheck.Count > 0)
        {
            Vector2 tile = tilesToCheck.Dequeue();
            tiles.Add(tile);

            for (int i = (int)tile.x - 1; i <= tile.x + 1; i++)
            {
                for (int j = (int)tile.y - 1; j <= tile.y + 1; j++)
                {
                    if (i >= 0 && i < m_CA.m_width && j >= 0 && j < m_CA.m_height && (i == tile.x || j == tile.y))
                    {
                        if (tilesChecked[x,y] == 0 && m_CA.m_grid[x, y] == 0)
                        {
                            tilesChecked[x, y] = 1;
                            tilesToCheck.Enqueue(new Vector2(x, y));
                        }
                    }
                }
            }
        }

        return tiles;
    }
}
