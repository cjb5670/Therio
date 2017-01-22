using UnityEngine;
using System.Collections.Generic;

public class BoatPhysics : MonoBehaviour
{
    #region BoatPhysics Member Variables
    public GameObject m_underwaterObject;
    private Mesh m_underwaterMesh;
    private Rigidbody2D m_boatRB;
    private ModifyBoatMesh m_modifyBoatMesh;
    private float m_waterDensity = 1027f;
    #endregion

    #region BoatPhysics Methods
    void Start()
    {
        m_boatRB = gameObject.GetComponent<Rigidbody2D>();
        m_modifyBoatMesh = new ModifyBoatMesh(gameObject);
        m_underwaterMesh = m_underwaterObject.GetComponent<MeshFilter>().mesh;
    }
    
    void Update()
    {
        m_modifyBoatMesh.GenerateUnderwaterMesh();
        m_modifyBoatMesh.DisplayMesh(m_underwaterMesh, "Underwater Mesh", m_modifyBoatMesh.m_underwaterTriangleData);
    }

    void FixedUpdate()
    {
        if (m_modifyBoatMesh.m_underwaterTriangleData.Count > 0)
        {
            AddUnderwaterForces();
        }
    }

    void AddUnderwaterForces()
    {
        List<TriangleData> underwaterTriangleData = m_modifyBoatMesh.m_underwaterTriangleData;

        for (int i = 0; i < underwaterTriangleData.Count; i++)
        {
            // Calculate the force and apply it to the center of the triangles
            TriangleData triangleData = underwaterTriangleData[i];
            Vector3 buoyancyForce = BuoyancyForce(m_waterDensity, triangleData);
            m_boatRB.AddForceAtPosition(buoyancyForce, triangleData.m_center);

            // Debug lines
            Debug.DrawRay(triangleData.m_center, triangleData.m_normal * 3f, Color.white);
            Debug.DrawRay(triangleData.m_center, buoyancyForce.normalized * -3f, Color.blue);
        }
    }

    private Vector3 BuoyancyForce(float density, TriangleData triangleData)
    {
        //Buoyancy is a hydrostatic force - it's there even if the water isn't flowing or if the boat stays still

        // F_buoyancy = rho * g * V
        // rho - density of the mediaum you are in
        // g - gravity
        // V - volume of fluid directly above the curved surface 

        // V = z * S * n 
        // z - distance to surface
        // S - surface area
        // n - normal to the surface
        Vector3 buoyancyForce = density * Physics.gravity.y * 
                                triangleData.m_distanceToSurface * 
                                triangleData.m_area * 
                                triangleData.m_normal;
        buoyancyForce.x = 0f;
        buoyancyForce.z = 0f;

        return buoyancyForce;
    }
    #endregion
}
