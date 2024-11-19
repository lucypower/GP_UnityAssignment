using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

public class MarchingSquares : MonoBehaviour
{
    CellularAutomata m_CA;
    MeshFilter m_meshFilter;

    ProBuilderMesh m_pbMesh;
    public Material m_pink;
    public Material m_grey;

    float m_threshold = 0.5f;

    List<GameObject> m_pbShapes = new List<GameObject>();


    // probably don't need these variables anymore

    List<Vector3> m_vertices = new List<Vector3>();
    List<Vector3> m_reverseVertices = new List<Vector3>();
    List<int> m_triangles = new List<int>();
    List<int> m_reverseTriangles = new List<int>();

    CombineInstance[] m_combineInstances = new CombineInstance[2];
    Vector3[] m_pbVertices;

    private void Awake()
    {
        m_CA = GetComponent<CellularAutomata>();
        m_meshFilter = GetComponent<MeshFilter>();
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

                //for (int i = 0; i < localVertices.Length; i++)
                //{
                //    m_pbVertices[m_verticesCount] = new Vector3(localVertices[i].x + offsetX, 0, localVertices[i].y + offsetY);

                //    m_verticesCount++;
                //}

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
            m_reverseVertices.Add(newV);
        }

        foreach (int t in localTriangles)
        {
            m_triangles.Add(t + vertexCount);
            m_reverseTriangles.Add(t + vertexCount);
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

        go.AddComponent<MeshCollider>();

        if(isWeird)
        {
            go.transform.position = new Vector3(offsetX, 1, offsetY);
        }
        else
        {
            go.transform.position = new Vector3(offsetX, 0, offsetY);
        }
        go.transform.Rotate(90, 0, 0);
    }

    // probably don't need these anymore
    public void CreateMesh()
    {
        Mesh mesh = new Mesh()
        {
            name = "New Mesh"
        };

        MeshFilter top = CreateTopMesh();
        MeshFilter bottom = CreateBottomMesh();

        MeshFilter[] meshFilters = new MeshFilter[] { top, bottom };

        for (int i = 0; i < 2; i++)
        {
            m_combineInstances[i].mesh = meshFilters[i].sharedMesh;
            m_combineInstances[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);
        }

        MeshCollider meshCollider = this.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        
        mesh.CombineMeshes(m_combineInstances.ToArray());
        transform.GetComponent<MeshFilter>().sharedMesh = mesh;
    }

    public MeshFilter CreateTopMesh()
    {
        Mesh mesh = new Mesh()
        {
            name = "Dungeon Mesh"
        };        

        var vertices = m_vertices;
        var triangles = m_triangles;

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        GameObject go = new GameObject()
        {
            name = "Top"
        };

        go.transform.SetParent(transform);
        go.AddComponent<MeshRenderer>().material = m_pink;
        MeshFilter meshFilter = go.AddComponent<MeshFilter>();

        Debug.Log("mesh1");

        meshFilter.mesh = mesh;

        return meshFilter;
    }

    public MeshFilter CreateBottomMesh()
    {
        Mesh mesh = new Mesh()
        {
            name = "Dungeon Mesh2"
        };

        m_reverseTriangles = m_triangles;
        var triangleCount = m_reverseTriangles.Count / 3;

        for (int i = 0; i < triangleCount; i++)
        {
            var tmp = m_reverseTriangles[i * 3];
            m_reverseTriangles[i * 3] = m_reverseTriangles[i * 3 + 1];
            m_reverseTriangles[i * 3 + 1] = tmp;
        }

        var tempNormals = mesh.normals;

        for (int i = 0; i < mesh.normals.Length; i++)
        {
            tempNormals[i] = -mesh.normals[i];
        }

        var normals = mesh.normals.Concat(tempNormals);

        m_reverseVertices.Reverse();

        var vertices = m_vertices.Concat(m_reverseVertices);
        var triangles = m_triangles.Concat(m_reverseTriangles);



        mesh.vertices = vertices.ToArray();
        mesh.triangles = m_reverseTriangles.ToArray();
        mesh.normals = normals.ToArray();

        Debug.Log("mesh2");

        GameObject go = new GameObject()
        {
            name = "Bottom"
        };

        go.transform.SetParent(transform);
        go.AddComponent<MeshRenderer>().material = m_pink;
        MeshFilter meshFilter = go.AddComponent<MeshFilter>();

        Debug.Log("mesh1");

        meshFilter.mesh = mesh;

        return meshFilter;
    }
}
