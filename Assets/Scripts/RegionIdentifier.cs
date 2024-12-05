using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class RegionIdentifier : MonoBehaviour
{
    public List<List<Vector3Int>> GetRegions(int width, int height, int[,] grid)
    {
        List<List<Vector3Int>> regions = new List<List<Vector3Int>>();
        int[,] tilesChecked = new int[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
               if (tilesChecked[i, j] == 0 && grid[i, j] == 0)
               {
                    List<Vector3Int> region = GetRegionArea(i, j, width, height, grid);
                    regions.Add(region);

                    foreach (Vector3Int tile in region)
                    {
                        tilesChecked[tile.x, tile.z] = 1;
                    }
               }
               else
               {
                    tilesChecked[i, j] = 1;
               }
            }
        }

        foreach (List<Vector3Int> region in regions)
        {
            if (region.Count < 20)
            {

            }
        }

        return regions;
    }

    List<Vector3Int> GetRegionArea(int x, int z, int width, int height, int[,] grid)
    {
        List<Vector3Int> tiles = new List<Vector3Int>();
        int[,] tilesChecked = new int[width, height];

        Queue<Vector3Int> tilesToCheck = new Queue<Vector3Int>();
        tilesToCheck.Enqueue(new Vector3Int(x, 0, z));
        tilesChecked[x, z] = 1;

        while (tilesToCheck.Count > 0)
        {
            Vector3Int tile = tilesToCheck.Dequeue();
            tiles.Add(tile);

            for (int i = tile.x - 1; i <= tile.x + 1; i++)
            {
                for (int j = tile.z - 1; j <= tile.z + 1; j++)
                {
                    if (i >= 0 && i < width && j >= 0 && j < height && (i == tile.x || j == tile.z))
                    {
                        if (tilesChecked[i, j] == 0 && grid[i, j] == 0)
                        {
                            tilesChecked[i, j] = 1;
                            tilesToCheck.Enqueue(new Vector3Int(i, 0, j));
                        }
                        else
                        {
                            tilesChecked[i, j] = 1;
                        }
                    }
                }
            }
        }

        return tiles;
    }    
}
