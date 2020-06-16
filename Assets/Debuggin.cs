using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debuggin : MonoBehaviour
{
    [SerializeField] protected Text g;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        g.text = GameEngine.Level.TowerHealth.ToString();
    }
}
