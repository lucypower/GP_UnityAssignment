using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.ProBuilder;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class MarchingSquares : MonoBehaviour
{
    CellularAutomata m_CA;
    MeshFilter m_meshFilter;
    ProBuilderMesh m_pbMesh;

    List<GameObject> m_pbShapes = new List<GameObject>();
    List<Vector3> m_vertices = new List<Vector3>();
    List<int> m_triangles = new List<int>();

    GameObject m_walls;
    public Material m_grey;

    List<ProBuilderMesh> m_meshesToCombine = new List<ProBuilderMesh>();
    List<ProBuilderMesh> m_combinedMesh;

    public bool m_combineMeshes;

    private void Awake()
    {
        m_CA = GetComponent<CellularAutomata>();
        m_meshFilter = GetComponent<MeshFilter>();

        m_walls = new GameObject()
        {
            name = "Walls"
        };
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
        return value < 0.5f ? 0 : 1;
    }

    public void CreateTriangles(int a, int b, int c, int d, int offsetX, int offsetY)
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

                CreateProBuilderShape(localVertices, "case 1", offsetX, offsetY, true);

                localTriangles = new int[]
                { 2, 1, 0};
                break;
            case 2:
                localVertices = new Vector3[]
                { new Vector3(1, 1), new Vector3(1, 0.5f), new Vector3(0.5f, 1) };

                CreateProBuilderShape(localVertices, "case 2", offsetX, offsetY, false);

                localTriangles = new int[]
                { 0, 1, 2};
                break;
            case 3:
                localVertices = new Vector3[]
                { new Vector3(0, 0.5f), new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0.5f) };

                CreateProBuilderShape(localVertices, "case 3", offsetX, offsetY, false);

                localTriangles = new int[]
                { 0, 1, 2, 0, 2, 3};
                break;
            case 4:
                localVertices = new Vector3[]
                { new Vector3(1, 0), new Vector3(0.5f, 0), new Vector3(1, 0.5f) };

                CreateProBuilderShape(localVertices, "case 4", offsetX, offsetY, false);

                localTriangles = new int[]
                { 0, 1, 2};
                break;
            case 5:
                localVertices = new Vector3[]
                { new Vector3(0, 0.5f), new Vector3(0, 1), new Vector3(0.5f, 1), new Vector3(1, 0), new Vector3(0.5f, 0), new Vector3(1, 0.5f) };

                CreateProBuilderShape(localVertices, "case 5", offsetX, offsetY, false); // TODO: case 5 and 10 don't spawn in 

                localTriangles = new int[]
                { 0, 1, 2, 3, 4, 5};
                break;
            case 6:
                localVertices = new Vector3[]
                { new Vector3(0.5f, 0), new Vector3(0.5f, 1), new Vector3(1, 1), new Vector3(1, 0) };

                CreateProBuilderShape(localVertices, "case 6", offsetX, offsetY, false);

                localTriangles = new int[]
                { 0, 1, 2, 0, 2, 3};
                break;
            case 7:
                localVertices = new Vector3[]
                { new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0), new Vector3(0.5f, 0), new Vector3(0, 0.5f) };

                CreateProBuilderShape(localVertices, "case 7", offsetX, offsetY, false);

                localTriangles = new int[]
                { 2, 3, 1, 3, 4, 1, 4, 0, 1};
                break;
            case 8:
                localVertices = new Vector3[]
                { new Vector3(0, 0.5f), new Vector3(0, 0), new Vector3(0.5f, 0) };

                CreateProBuilderShape(localVertices, "case 8", offsetX, offsetY, true);

                localTriangles = new int[]
                { 2, 1, 0};
                break;
            case 9:
                localVertices = new Vector3[]
                { new Vector3(0, 0), new Vector3(0.5f, 0), new Vector3(0.5f, 1), new Vector3(0, 1) };

                CreateProBuilderShape(localVertices, "case 9", offsetX, offsetY, true);

                localTriangles = new int[]
                { 1, 0, 2, 0, 3, 2};
                break;
            case 10:
                localVertices = new Vector3[]
                { new Vector3(0, 0), new Vector3(0, 0.5f), new Vector3(0.5f, 0), new Vector3(1, 1), new Vector3(0.5f, 1), new Vector3(1, 0.5f) };

                CreateProBuilderShape(localVertices, "case 10", offsetX, offsetY, false);

                localTriangles = new int[]
                { 0, 1, 2, 5, 4, 3};
                break;
            case 11:
                localVertices = new Vector3[]
                { new Vector3(0, 0), new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0.5f), new Vector3(0.5f, 0) };

                CreateProBuilderShape(localVertices, "case 11", offsetX, offsetY, false);

                localTriangles = new int[]
                { 0, 1, 2, 0, 2, 3, 4, 0, 3};
                break;
            case 12:
                localVertices = new Vector3[]
                { new Vector3(0, 0), new Vector3(1, 0), new Vector3(1, 0.5f), new Vector3(0, 0.5f) };

                CreateProBuilderShape(localVertices, "case 12", offsetX, offsetY, true);

                localTriangles = new int[]
                { 0, 3, 2, 0, 2, 1};
                break;
            case 13:
                localVertices = new Vector3[]
                { new Vector3(0, 0), new Vector3(0, 1), new Vector3(0.5f, 1), new Vector3(1, 0.5f), new Vector3(1, 0) };

                CreateProBuilderShape(localVertices, "case 13", offsetX, offsetY, false);

                localTriangles = new int[]
                { 0, 1, 2, 0, 2, 3, 0, 3, 4};
                break;
            case 14:
                localVertices = new Vector3[]
                { new Vector3(1, 1), new Vector3(1, 0), new Vector3(0, 0), new Vector3(0, 0.5f), new Vector3(0.5f, 1) };

                CreateProBuilderShape(localVertices, "case 14", offsetX, offsetY, false);

                localTriangles = new int[]
                { 0, 1, 4, 1, 3, 4, 1, 2, 3};
                break;
            case 15:
                localVertices = new Vector3[]
                { new Vector3(0, 0), new Vector3(0, 1), new Vector3(1, 1), new Vector3(1, 0) };

                CreateProBuilderShape(localVertices, "case 15", offsetX, offsetY, false);

                localTriangles = new int[]
                { 0, 1, 2, 0, 2, 3};
                break;
        }

        foreach (Vector3 v in localVertices)
        {
            Vector3 newV = new Vector3(v.x + offsetX, 0.5f, v.y + offsetY);
            m_vertices.Add(newV);
            newV = new Vector3(v.x + offsetX, 0.5f, v.y + offsetY);
        }

        foreach (int t in localTriangles)
        {
            m_triangles.Add(t + vertexCount);
        }

    }

    void CreateProBuilderShape(Vector3[] vertices, string triName, int offsetX, int offsetY, bool isWeird)
    {
        var go = new GameObject()
        {
            name = triName
        };

        m_pbShapes.Add(go);

        go.transform.SetParent(transform);

        m_pbMesh = go.AddComponent<ProBuilderMesh>();

        go.GetComponent<MeshRenderer>().material = m_grey;

        go.GetComponent<ProBuilderMesh>().CreateShapeFromPolygon(vertices, 1, false);

        if(isWeird)
        {
            go.transform.position = new Vector3(offsetX, 1, offsetY);
        }
        else
        {
            go.transform.position = new Vector3(offsetX, 0, offsetY);
        }
        go.transform.Rotate(90, 0, 0);

        m_meshesToCombine.Add(go.GetComponent<ProBuilderMesh>());
    }

    public void CombinePBMeshes()
    {        
        m_combinedMesh = CombineMeshes.Combine(m_meshesToCombine, m_meshesToCombine.First());

        for (int i = 1; i < m_meshesToCombine.Count; i++)
        {
            Destroy(m_meshesToCombine[i].gameObject);
        }

        foreach (ProBuilderMesh mesh in m_combinedMesh)
        {
            mesh.transform.SetParent(m_walls.transform);
            mesh.name = "Walls Mesh";
            mesh.AddComponent<MeshCollider>();
        }
    }
}
