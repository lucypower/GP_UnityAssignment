using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomata : MonoBehaviour
{
    int[,] m_grid;
    int[,] m_tempNewGrid;

    [SerializeField] GameObject m_whiteCube, m_blackCube;

    [SerializeField] int m_height, m_width, m_iterations;
    [SerializeField] float m_density;

    private void Start()
    {
        GenerateGrid();


        //for (int i = 0; i < m_iterations; i++)
        //{
        //    IterateGrid();
        //}

        InstantiateGrid();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("P");

            IterateGrid();

            InstantiateGrid();
        }
    }

    public void GenerateGrid()
    {
        m_grid = new int[m_height, m_width];
        m_tempNewGrid = m_grid;

        for (int i = 0; i < m_height; i++)
        {
            for (int j = 0; j < m_width; j++)
            {
                if (i == 0 || j == 0 || i == m_height - 1 || j == m_width - 1)
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
        for (int i = 0; i < m_height; i++)
        {
            for (int j = 0; j < m_width; j++)
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

        for (int i = 0; i < m_height; i++)
        {
            for (int j = 0; j < m_width; j++)
            {
                m_grid[i, j] = m_tempNewGrid[i, j];
            }
        }

        m_tempNewGrid = new int[m_height, m_width];
    }

    public int GetNeighbouringWallCount(int x, int y)
    {
        int neighbouringWalls = 0;

        for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
        {
            for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
            {
                if (neighbourX != 0 || neighbourY != 0)
                {
                    neighbouringWalls += m_grid[x, y];
                }
            }
        }

        return neighbouringWalls;
    }

    public void InstantiateGrid()
    {
        for (int i = 0; i < m_height; i++)
        {
            for (int j = 0; j < m_width; j++)
            {
                if (m_grid[i, j] == 1)
                {
                    Instantiate(m_blackCube, new Vector3(i, 0, j), Quaternion.identity);
                }
                else
                {
                    Instantiate(m_whiteCube, new Vector3(i, 0, j), Quaternion.identity);
                }
            }
        }

        //if (m_grid[x, y] == 1)
        //{
        //    Instantiate(m_blackCube, new Vector3(x, 0, y), Quaternion.identity);
        //}
        //else
        //{
        //    Instantiate(m_whiteCube, new Vector3(x, 0, y), Quaternion.identity);
        //}
    }
}
