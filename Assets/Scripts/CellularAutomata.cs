using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomata : MonoBehaviour
{
    [HideInInspector] public int[,] m_grid, m_tempNewGrid;

    [HideInInspector] public List<Vector3> m_openSpaces = new List<Vector3>();

    [SerializeField] GameObject m_floor;

    public int m_height, m_width, m_iterations;
    public float m_density;

    public void StartCA()
    {
        GenerateGrid();

        for (int i = 0; i < m_iterations; i++)
        {
            IterateGrid();
        }

        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                if (i == 0 || j == 0 || i == m_width - 1 || j == m_height - 1)
                {
                    m_grid[i, j] = 1;
                }
            }
        }

        //FindOpenSpaces();
    }

    public void GenerateGrid()
    {
        m_grid = new int[m_width, m_height];
        m_tempNewGrid = new int[m_width, m_height];


        GameObject floor = Instantiate(m_floor, new Vector3((m_width / 2) - .5f, 0, (m_height / 2) - .5f), Quaternion.identity);
        floor.transform.localScale += new Vector3(m_width , 0, m_height );


        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                if (i == 0 || j == 0 || i == m_width - 1 || j == m_height - 1)
                {
                    m_grid[i, j] = 1;
                }
                else
                {
                    m_grid[i, j] = UnityEngine.Random.value > m_density ? 0 : 1;
                }

            }
        }
    }

    public void IterateGrid()
    {
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                int neighbouringWalls = GetNeighbouringWallCount(i, j);

                if (neighbouringWalls > 4)
                {
                    m_tempNewGrid[i, j] = 1;
                }
                else
                {
                    m_tempNewGrid[i, j] = 0;                    
                }
            }
        }

        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                m_grid[i, j] = m_tempNewGrid[i, j];
            }
        }

        m_tempNewGrid = new int[m_width, m_height];
    }

    public int GetNeighbouringWallCount(int x, int y)
    {
        int neighbouringWalls = 0;

        for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
        {
            for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
            {
                if (neighbourX >= 0 && neighbourX < m_width && neighbourY >= 0 && neighbourY < m_height)
                {
                    if (neighbourX != 0 || neighbourY != 0)
                    {
                        neighbouringWalls += m_grid[neighbourX, neighbourY];
                    }
                }                
            }
        }

        return neighbouringWalls;
    }

    public void FinaliseGrid()
    {
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                if (i == 0 || j == 0 || i == m_width - 1 || j == m_height - 1)
                {
                    m_grid[i, j] = 1;
                }

                if ((i == 0 && j == 0) || (i == 0 && j == m_width - 1))
                {
                    //m_grid[i, j] = 0;
                }
            }
        }
    }

    public void FindOpenSpaces()
    {
        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                int neighbours = GetNeighbouringWallCount(i, j);

                if (m_grid[i, j] == 0 && neighbours == 0)
                {
                    m_openSpaces.Add(new Vector3(i, .5f, j));
                }
            }
        }
    }
}
