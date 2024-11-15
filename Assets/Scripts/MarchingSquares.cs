using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingSquares : MonoBehaviour
{
    CellularAutomata m_CA;
    MeshFilter m_meshFilter;

    float m_threshold = 0.5f;

    List<Vector3> m_vertices = new List<Vector3>();
    List<int> m_triangles = new List<int>();

    private void Awake()
    {
        m_CA = GetComponent<CellularAutomata>();
        m_meshFilter = GetComponent<MeshFilter>();
    }

    public void CreateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = m_vertices.ToArray();
        mesh.triangles = m_triangles.ToArray();

        m_meshFilter.mesh = mesh;
    }

    public void MarchSquares()
    {
        for (int i = 0; i < m_CA.m_width - 1; i++)
        {
            for (int j = 0; j < m_CA.m_height - 1; j++)
            {
                float a = m_CA.m_grid[i, j];
                float b = m_CA.m_grid[i + 1, j];
                float c = m_CA.m_grid[i + 1, j + 1];
                float d = m_CA.m_grid[i, j + 1];

                CreateTriangles(GetHeight(a), GetHeight(b), GetHeight(c), GetHeight(d), i, j);
            }
        }
    }

    public int GetHeight(float value)
    {
        return value < m_threshold ? 0 : 1;
    }

    public void CreateTriangles(int a, int b, int c, int d, float offsetX, float offsetY)
    {
        int total = (a * 8) + (b * 4) + (c * 2) + (d * 1);

        Vector3[] localVertices = new Vector3[6];
        int[] localTriangles = new int[6];
        int vertexCount = m_vertices.Count;

        switch (total)
        {
            case 0:
                return;
            case 1:
                localVertices = new Vector3[]
                { new Vector3(0, 1f), new Vector3(0, 0.5f), new Vector3(0.5f, 1) };

                localTriangles = new int[]
                { 2, 1, 0};
                break;
            case 2:
                localVertices = new Vector3[]
                { new Vector3(1, 1), new Vector3(1, 0.5f), new Vector3(0.5f, 1) };

                localTriangles = new int[]
                { 0, 1, 2};
                break;
            case 3:
                localVertices = new Vector3[]
                { new Vector3(0, 0.5f), new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0.5f) };

                localTriangles = new int[]
                { 0, 1, 2, 0, 2, 3};
                break;
            case 4:
                localVertices = new Vector3[]
                { new Vector3(1, 0), new Vector3(0.5f, 0), new Vector3(1, 0.5f) };

                localTriangles = new int[]
                { 0, 1, 2};
                break;
            case 5:
                localVertices = new Vector3[]
                { new Vector3(0, 0.5f), new Vector3(0, 1), new Vector3(0.5f, 1), new Vector3(1, 0), new Vector3(0.5f, 0), new Vector3(1, 0.5f) };

                localTriangles = new int[]
                { 0, 1, 2, 3, 4, 5};
                break;
            case 6:
                localVertices = new Vector3[]
                { new Vector3(0.5f, 0), new Vector3(0.5f, 1), new Vector3(1, 1), new Vector3(1, 0) };

                localTriangles = new int[]
                { 0, 1, 2, 0, 2, 3};
                break;
            case 7:
                localVertices = new Vector3[]
                { new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0), new Vector3(0.5f, 0), new Vector3(0, 0.5f) };

                localTriangles = new int[]
                { 2, 3, 1, 3, 4, 1, 4, 0, 1};
                break;
            case 8:
                localVertices = new Vector3[]
                { new Vector3(0, 0.5f), new Vector3(0, 0), new Vector3(0.5f, 0) };

                localTriangles = new int[]
                { 2, 1, 0};
                break;
            case 9:
                localVertices = new Vector3[]
                { new Vector3(0, 0), new Vector3(0.5f, 0), new Vector3(0.5f, 1), new Vector3(0, 1) };

                localTriangles = new int[]
                { 1, 0, 2, 0, 3, 2};
                break;
            case 10:
                localVertices = new Vector3[]
                { new Vector3(0, 0), new Vector3(0, 0.5f), new Vector3(0.5f, 0), new Vector3(1, 1), new Vector3(0.5f, 1), new Vector3(1, 0.5f) };

                localTriangles = new int[]
                { 0, 1, 2, 5, 4, 3};
                break;
            case 11:
                localVertices = new Vector3[]
                { new Vector3(0, 0), new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0.5f), new Vector3(0.5f, 0) };

                localTriangles = new int[]
                { 0, 1, 2, 0, 2, 3, 4, 0, 3};
                break;
            case 12:
                localVertices = new Vector3[]
                { new Vector3(0, 0), new Vector3(1, 0), new Vector3(1, 0.5f), new Vector3(0, 0.5f) };

                localTriangles = new int[]
                { 0, 3, 2, 0, 2, 1};
                break;
            case 13:
                localVertices = new Vector3[]
                { new Vector3(0, 0), new Vector3(0, 1), new Vector3(0.5f, 1), new Vector3(1, 0.5f), new Vector3(1, 0) };

                localTriangles = new int[]
                { 0, 1, 2, 0, 2, 3, 0, 3, 4};
                break;
            case 14:
                localVertices = new Vector3[]
                { new Vector3(1, 1), new Vector3(1, 0), new Vector3(0, 0), new Vector3(0, 0.5f), new Vector3(0.5f, 1) };

                localTriangles = new int[]
                { 0, 1, 4, 1, 3, 4, 1, 2, 3};
                break;
            case 15:
                localVertices = new Vector3[]
                { new Vector3(0, 0), new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0) };

                localTriangles = new int[]
                { 0, 1, 2, 0, 2, 3};
                break;
        }

        foreach (Vector3 v in localVertices)
        {
            Vector3 newV = new Vector3(v.x + offsetX, 0, v.y + offsetY);
            m_vertices.Add(newV);
        }

        foreach (int t in localTriangles)
        {
            m_triangles.Add(t + vertexCount);
        }
    }
}
