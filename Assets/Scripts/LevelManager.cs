using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    protected NavMeshSurface    m_surface;
    protected static Transform  m_endPoint;

    [SerializeField] protected GameObject m_obstacle;

    public int width = 10;
    public int height = 10;

    public static Transform EndPoint {  get { return m_endPoint; } }

    void GetGridSize()
    {

    }

    void GenerateLevel()
    {
        // Loop over the grid
        for (int x = 0; x <= width; x += 2)
        {
            for (int y = 0; y <= height; y += 2)
            {
                // Should we place a wall?
                if (Random.value > .7f)
                {
                    // Spawn a wall
                    Vector3 pos = new Vector3(x - width / 2f, 1f, y - height / 2f);
                    Instantiate(m_obstacle, pos, Quaternion.identity, transform);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_endPoint = transform.Find("Goal");
        if (m_endPoint == null)
            Debug.LogWarning("Couldnt find the destination for this level");

        if (GameObject.Find("NavMesh") != null)
            m_surface = GameObject.Find("NavMesh").GetComponent<NavMeshSurface>();
        else
            Debug.LogWarning("Couldn't find Nav Mesh Object");

        GenerateLevel();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
