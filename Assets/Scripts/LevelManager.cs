using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static Transform endPoint;

    // Start is called before the first frame update
    void Start()
    {
        endPoint = transform.Find("Goal");

        if (endPoint == null)
            Debug.LogWarning("Couldnt find the destination for this level");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
