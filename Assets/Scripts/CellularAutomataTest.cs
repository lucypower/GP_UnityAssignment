using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(MeshFilter), typeof(MeshRenderer))]
public class CellularAutomataTest : MonoBehaviour
{
    [SerializeField] int m_width, m_height;

    int[,] m_grid;

    float m_threshold = 0.5f;

    [SerializeField] Transform m_cube, m_cubeParent;
    [SerializeField] Material m_black, m_white;

    MeshFilter m_meshFilter;

    List<Vector3> m_vertices = new List<Vector3>();
    List<int> m_triangles = new List<int>();

    private void Start()
    {
        m_meshFilter = GetComponent<MeshFilter> ();

        SetGrid();
        GetNodeCorners();
        CreateMesh();
        CreateGrid();
    }

    public void SetGrid()
    {
        m_grid = new int[m_width + 1, m_height + 1];

        for (int i = 0; i < m_grid.GetLength(0); i++)
        {
            for (int j = 0;  j < m_grid.GetLength(1); j++)
            {
                m_grid[i, j] = Random.value > 0.52 ? 0 : 1;
            }
        }
    }

    public void CreateMesh()
    {
        Mesh mesh = new Mesh();

        mesh.vertices = m_vertices.ToArray();
        mesh.triangles = m_triangles.ToArray();

        m_meshFilter.mesh = mesh;
    }

    public void CreateGrid()
    {
        foreach (Transform child in m_cubeParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < m_grid.GetLength(0); i++)
        {
            for (int j = 0; j < m_grid.GetLength(1); j++)
            {
                Vector3 position = transform.TransformPoint(new Vector3(i, 0, j));
                Transform newCube = Instantiate(m_cube, position, new Quaternion(), m_cubeParent);
                newCube.GetComponent<MeshRenderer>().material = m_grid[i,j] == 1 ? m_black : m_white;
            }
        }
    }

    public void GetNodeCorners()
    {
        m_vertices.Clear();
        m_triangles.Clear();

        for (int i = 0; i < m_width; i++)
        {
            for (int j = 0; j < m_height; j++)
            {
                float a = m_grid[i, j];
                float b = m_grid[i + 1, j];
                float c = m_grid[i + 1, j + 1];
                float d = m_grid[i, j + 1];

                MarchSqaures(GetHeight(a), GetHeight(b), GetHeight(c), GetHeight(d), i, j);
            }
        }
    }

    public int GetHeight(float value)
    {
        return value < m_threshold ? 0 : 1;
    }

    public void MarchSqaures(int a, int b, int c, int d, float offsetX, float offsetY)
    {
        int value = a * 8 + b * 4 + c * 2 + d * 1;

        Vector3[] localVertices = new Vector3[6];
        int[] localTriangles = new int[6];

        int vertexCount = m_vertices.Count;

        //switch (value)
        //{
        //    case 0:
        //        return;
        //    case 1:
        //        localVertices = new Vector3[]
        //        { new Vector3(0, 0, 1f), new Vector3(0, 0, 0.5f), new Vector3(0.5f, 0, 1) };

        //        localTriangles = new int[]
        //        { 2, 1, 0};
        //        break;
        //    case 2:
        //        localVertices = new Vector3[]
        //        { new Vector3(1, 0, 1), new Vector3(1, 0, 0.5f), new Vector3(0.5f, 0, 1) };

        //        localTriangles = new int[]
        //        { 0, 1, 2};
        //        break;
        //    case 3:
        //        localVertices = new Vector3[]
        //        { new Vector3(0, 0, 0.5f), new Vector3(0, 0, 1), new Vector3(1, 0, 1), new Vector3(1, 0, 0.5f) };

        //        localTriangles = new int[]
        //        { 0, 1, 2, 0, 2, 3};
        //        break;
        //    case 4:
        //        localVertices = new Vector3[]
        //        { new Vector3(1, 0, 0), new Vector3(0.5f ,  0, 0), new Vector3(1, 0, 0.5f) };

        //        localTriangles = new int[]
        //        { 0, 1, 2};
        //        break;
        //    case 5:
        //        localVertices = new Vector3[]
        //        { new Vector3(0, 0, 0.5f), new Vector3(0,  0, 1), new Vector3(0.5f, 0,  1), new Vector3(1, 0, 0), new Vector3(0.5f, 0, 0), new Vector3(1, 0, 0.5f) };

        //        localTriangles = new int[]
        //        { 0, 1, 2, 3, 4, 5};
        //        break;
        //    case 6:
        //        localVertices = new Vector3[]
        //        { new Vector3(0.5f, 0, 0), new Vector3(0.5f, 0, 1), new Vector3(1 , 0, 1), new Vector3(1, 0, 0) };

        //        localTriangles = new int[]
        //        { 0, 1, 2, 0, 2, 3};
        //        break;
        //    case 7:
        //        localVertices = new Vector3[]
        //        { new Vector3(0, 0, 1), new Vector3(1, 0, 1), new Vector3(1, 0, 0), new Vector3(0.5f, 0, 0), new Vector3(0, 0, 0.5f) };

        //        localTriangles = new int[]
        //        { 2, 3, 1, 3, 4, 1, 4, 0, 1};
        //        break;
        //    case 8:
        //        localVertices = new Vector3[]
        //        { new Vector3(0, 0, 0.5f), new Vector3(0, 0, 0), new Vector3(0.5f, 0, 0) };

        //        localTriangles = new int[]
        //        { 2, 1, 0};
        //        break;
        //    case 9:
        //        localVertices = new Vector3[]
        //        { new Vector3(0, 0, 0), new Vector3(0.5f, 0, 0), new Vector3(0.5f, 0, 1), new Vector3(0, 0, 1) };

        //        localTriangles = new int[]
        //        { 1, 0, 2, 0, 3, 2};
        //        break;
        //    case 10:
        //        localVertices = new Vector3[]
        //        { new Vector3(0, 0, 0), new Vector3(0, 0, 0.5f), new Vector3(0.5f, 0, 0), new Vector3(1, 0, 1), new Vector3(0.5f, 0, 1), new Vector3(1, 0, 0.5f) };

        //        localTriangles = new int[]
        //        { 0, 1, 2, 5, 4, 3};
        //        break;
        //    case 11:
        //        localVertices = new Vector3[]
        //        { new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 1), new Vector3(1, 0, 0.5f), new Vector3(0.5f, 0, 0) };

        //        localTriangles = new int[]
        //        { 0, 1, 2, 0, 2, 3, 4, 0, 3};
        //        break;
        //    case 12:
        //        localVertices = new Vector3[]
        //        { new Vector3(0, 0, 0), new Vector3(1, 0, 0), new Vector3(1, 0, 0.5f), new Vector3(0, 0, 0.5f) };

        //        localTriangles = new int[]
        //        { 0, 3, 2, 0, 2, 1};
        //        break;
        //    case 13:
        //        localVertices = new Vector3[]
        //        { new Vector3(0, 0,  0), new Vector3(0,0 , 1), new Vector3(0.5f, 0, 1), new Vector3(1, 0, 0.5f), new Vector3(1, 0, 0) };

        //        localTriangles = new int[]
        //        { 0, 1, 2, 0, 2, 3, 0, 3, 4};
        //        break;
        //    case 14:
        //        localVertices = new Vector3[]
        //        { new Vector3(1, 0,  1), new Vector3(1, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0.5f), new Vector3(0.5f, 0, 1) };

        //        localTriangles = new int[]
        //        { 0, 1, 4, 1, 3, 4, 1, 2, 3};
        //        break;
        //    case 15:
        //        localVertices = new Vector3[]
        //        { new Vector3(0, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 1), new Vector3(1, 0, 0) };

        //        localTriangles = new int[]
        //        { 0, 1, 2, 0, 2, 3};
        //        break;
        //}


        switch (value)
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
            Vector3 newV = new Vector3((v.x + offsetX), 0, (v.y + offsetY));
            m_vertices.Add(newV);
        }

        foreach (int t in localTriangles)
        {
            m_triangles.Add(t + vertexCount);
        }
    }
}
