using UnityEngine;
using System.Collections.Generic;

public class ModifyBoatMesh
{
    #region ModifyBoatMesh Member Variables
    public List<TriangleData> m_underwaterTriangleData = new List<TriangleData>();
    public Vector3[] m_boatVerticesGlobal;
    private Transform m_boatTransform;
    private Vector3[] m_boatVertices;
    private int[] m_boatTriangles;
    private float[] m_allDistancesToWater;
    #endregion

    #region ModifyBoatMesh Methods
    public ModifyBoatMesh(GameObject boatObject)
    {
        m_boatTransform = boatObject.transform;
        m_boatVertices = boatObject.GetComponent<MeshFilter>().mesh.vertices;
        m_boatTriangles = boatObject.GetComponent<MeshFilter>().mesh.triangles;
        m_boatVerticesGlobal = new Vector3[m_boatVertices.Length];
        m_allDistancesToWater = new float[m_boatVertices.Length];
    }

    public void GenerateUnderwaterMesh()
    {
        // Reset
        m_underwaterTriangleData.Clear();

        // Find all the distances to water
        for (int i = 0; i < m_boatVertices.Length; i++)
        {
            Vector3 globalPosition = m_boatTransform.TransformPoint(m_boatVertices[i]);
            m_boatVerticesGlobal[i] = globalPosition;
            m_allDistancesToWater[i] = WaterController.current.DistanceToWater(globalPosition, Time.time);
        }

        // Add the triangles that are below the water
        AddTriangles();
    }

    private void AddTriangles()
    {
        // List that stores data necessary to sort vertices based on distance to water
        List<VertexData> vertexData = new List<VertexData>();
        vertexData.Add(new VertexData());
        vertexData.Add(new VertexData());
        vertexData.Add(new VertexData());

        // Loop through all triangles (3 verts at a time)
        int i = 0;
        while (i < m_boatTriangles.Length)
        {
            // Loop through 3 vertices
            for (int x = 0; x < 3; x++)
            {
                vertexData[x].m_index = x;
                vertexData[x].m_distance = m_allDistancesToWater[m_boatTriangles[i]];
                vertexData[x].m_globalVertexPosition = m_boatVerticesGlobal[m_boatTriangles[i]];
                i++;
            }

            // All vertices are above the water
            if (vertexData[0].m_distance > 0f && 
                vertexData[1].m_distance > 0f && 
                vertexData[2].m_distance > 0f)
            {
                continue;
            }

            // All vertices underwater
            if (vertexData[0].m_distance < 0f &&
                vertexData[1].m_distance < 0f &&
                vertexData[2].m_distance < 0f)
            {
                Vector3 p1 = vertexData[0].m_globalVertexPosition;
                Vector3 p2 = vertexData[1].m_globalVertexPosition;
                Vector3 p3 = vertexData[2].m_globalVertexPosition;
                m_underwaterTriangleData.Add(new TriangleData(p1, p2, p3));
            }
            // One or two vertices are underwater
            else
            {
                vertexData.Sort((x, y) => x.m_distance.CompareTo(y.m_distance));
                vertexData.Reverse();

                if (vertexData[0].m_distance > 0f &&
                    vertexData[1].m_distance < 0f &&
                    vertexData[2].m_distance < 0f)
                {
                    AddTrianglesOneAboveWater(vertexData);
                }
                else if (vertexData[0].m_distance > 0f &&
                         vertexData[1].m_distance > 0f &&
                         vertexData[2].m_distance < 0f)
                {
                    AddTrianglesTwoAboveWater(vertexData);
                }
            }
        }
    }

    private void AddTrianglesOneAboveWater(List<VertexData> vertexData)
    {
        // H is at position 0
        // Right of H is M
        // Left of H is L

        Vector3 H = vertexData[0].m_globalVertexPosition;
        Vector3 M = Vector3.zero;
        Vector3 L = Vector3.zero;

        // Index of M
        int M_index = vertexData[0].m_index - 1;
        if (M_index < 0)
        {
            M_index = 2;
        }

        // Get the heights of the water
        float h_H = vertexData[0].m_distance;
        float h_M = 0f;
        float h_L = 0f;

        if (vertexData[1].m_index == M_index)
        {
            M = vertexData[1].m_globalVertexPosition;
            L = vertexData[2].m_globalVertexPosition;

            h_M = vertexData[1].m_distance;
            h_L = vertexData[2].m_distance;
        }
        else
        {
            M = vertexData[2].m_globalVertexPosition;
            L = vertexData[1].m_globalVertexPosition;

            h_M = vertexData[2].m_distance;
            h_L = vertexData[1].m_distance;
        }

        // Calculate where to cut the triangle to form a square

        // Point I_M
        Vector3 MH = H - M;
        float t_M = -h_M / (h_H - h_M);
        Vector3 MI_M = t_M * MH;
        Vector3 I_M = MI_M + M;

        // Point I_L
        Vector3 LH = H - L;
        float t_L = -h_L / (h_H - h_L);
        Vector3 LI_L = t_L * LH;
        Vector3 I_L = LI_L + L;

        m_underwaterTriangleData.Add(new TriangleData(M, I_M, I_L));
        m_underwaterTriangleData.Add(new TriangleData(M, I_L, L));
    }
    
    private void AddTrianglesTwoAboveWater(List<VertexData> vertexData)
    {
        // H and M are above the water
        // H is after the vertice that's below water, which is L
        // So we know which one is L because it is last in the sorted list
        Vector3 L = vertexData[2].m_globalVertexPosition;
        Vector3 H = Vector3.zero;
        Vector3 M = Vector3.zero;

        // Index of H
        int H_index = vertexData[2].m_index + 1;
        if (H_index > 2)
        {
            H_index = 0;
        }

        // Get the heights of the water
        float h_L = vertexData[2].m_distance;
        float h_H = 0f;
        float h_M = 0f;

        if (vertexData[1].m_index == H_index)
        {
            H = vertexData[1].m_globalVertexPosition;
            M = vertexData[0].m_globalVertexPosition;

            h_H = vertexData[1].m_distance;
            h_M = vertexData[0].m_distance;
        }
        else
        {
            H = vertexData[0].m_globalVertexPosition;
            M = vertexData[1].m_globalVertexPosition;

            h_H = vertexData[0].m_distance;
            h_M = vertexData[1].m_distance;
        }

        // Calculate where to cut the triangle to form a square

        //Point J_M
        Vector3 LM = M - L;
        float t_M = -h_L / (h_M - h_L);
        Vector3 LJ_M = t_M * LM;
        Vector3 J_M = LJ_M + L;

        //Point J_H
        Vector3 LH = H - L;
        float t_H = -h_L / (h_H - h_L);
        Vector3 LJ_H = t_H * LH;
        Vector3 J_H = LJ_H + L;

        m_underwaterTriangleData.Add(new TriangleData(L, J_H, J_M));
    }

    public void DisplayMesh(Mesh mesh, string name, List<TriangleData> trianglesData)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        // Build the mesh
        for (int i = 0; i < trianglesData.Count; i++)
        {
            Vector3 p1 = m_boatTransform.InverseTransformPoint(trianglesData[i].m_p1);
            Vector3 p2 = m_boatTransform.InverseTransformPoint(trianglesData[i].m_p2);
            Vector3 p3 = m_boatTransform.InverseTransformPoint(trianglesData[i].m_p3);

            vertices.Add(p1);
            triangles.Add(vertices.Count - 1);
            vertices.Add(p2);
            triangles.Add(vertices.Count - 1);
            vertices.Add(p3);
            triangles.Add(vertices.Count - 1);
        }

        // Remove the old mesh
        mesh.Clear();
        mesh.name = name;
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateBounds();
    }
    #endregion

    // Helper class to store triangle data
    private class VertexData
    {
        public int m_index;
        public float m_distance;
        public Vector3 m_globalVertexPosition;
    }
}
