using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    private System.Random m_random;

    protected NavMeshSurface    m_surface;
    protected static Transform  m_endPoint;
    protected Transform         m_obstacleParent;

    protected int           m_planeWidth;
    protected int           m_planeDepth;
    protected Vector3       m_obstacleSize;
    protected Vector3       m_v3;

    protected static bool   m_loaded = false;

    [SerializeField] protected Transform m_obstacle;

    public static Transform EndPoint    {  get { return m_endPoint; } }
    public static bool      Loaded      {  get { return m_loaded; } }

    /* TO DO - Get plane size, multiply by number of planes to get grid size. Then generate level 
     Be able to randomly place end point on grid 
    Be able to 'draw' path
    Set a different nav mesh types for placeables
     */

    /// <summary>
    /// Gets the size of all the planes I have in the scene
    /// </summary>
    void GetPlaneSize()
    {
        Transform plane = GameObject.Find("Plane").transform;
        Renderer r;

        if (plane == null)
            Debug.LogWarning("Couldn't find a single plane on this field");
        else
        {
            r = plane.GetComponent<Renderer>();

            if (r == null)
                Debug.LogWarningFormat("{0} doesn't have a renderer", plane.name);
            else
            {
                m_planeWidth = (int)r.bounds.size.x;
                m_planeDepth = (int)r.bounds.size.z;
            }
        }
    }
    
    /// <summary>
    /// Randomly creates a level during runtime based on the size of the grid
    /// </summary>
    void GenerateLevel()
    {
        int halfWidth, halfDepth;
        float a, b;

        halfDepth = (m_planeDepth / 2);
        halfWidth = (m_planeWidth / 2);

        // Loop over the grid
        for (int x = -halfWidth; x < m_planeWidth + halfWidth; x+=2)
        {
            a = (x + m_obstacleSize.x); //So theres some space between blocks

            for (int y = -halfDepth; y < m_planeDepth + halfDepth; y+=2)
            {
                b = (y + m_obstacleSize.z);
                int num = m_random.Next(0, 10);

                // Should we place a wall?
                if (num < 6f)
                {
                    // Spawn a wall
                    m_v3.x = (a - halfWidth);
                    m_v3.y = .5f;
                    m_v3.z = (b - halfDepth);

                    Instantiate(m_obstacle, m_v3, Quaternion.identity, m_obstacleParent );
                }
            }
        }
    }

    void SetVariables()
    {
        m_random = new System.Random(System.DateTime.Now.Millisecond);

        m_obstacleParent = GameObject.Find("Obstacles").transform;
        if (m_obstacleParent == null)
            Debug.LogWarning("Couldn't find the Obstacles gameobject");

        m_endPoint = transform.Find("Goal");
        if (m_endPoint == null)
            Debug.LogWarning("Couldnt find the destination for this level");

        if (GameObject.Find("NavMesh") != null)
            m_surface = GameObject.Find("NavMesh").GetComponent<NavMeshSurface>();
        else
            Debug.LogWarning("Couldn't find Nav Mesh Object");

        m_obstacleSize = m_obstacle.GetComponent<Renderer>().bounds.size;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetVariables();
        GetPlaneSize();
        GenerateLevel();
        m_surface.BuildNavMesh();

        m_loaded = true;
    }
}
