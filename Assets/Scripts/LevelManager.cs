﻿using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    private static System.Random m_random;

    [SerializeField] protected int m_numWaves;
    [SerializeField] protected List<EnemySpawner> m_spawners = new List<EnemySpawner>();

    protected static UIController m_ui;

    protected static Transform  m_endPoint;
    protected static bool       m_loaded = false;
    protected float             m_breakTimer;
    protected int               m_wavesNumber = 0;

    public static Transform         EndPoint    {  get { return m_endPoint; } }
    public static bool              Loaded      {  get { return m_loaded; } }
    public static System.Random     Randomizer  {  get { return m_random; } }
    public static UIController      UI          {  get { return m_ui; } }

    // TO DO - Set a different nav mesh types for placeables
    /// <summary>
    /// Start the wave after letting the player place defenses
    /// </summary>
    public void BeginRound()
    {
        if(m_wavesNumber < m_numWaves)
        {
            foreach(EnemySpawner spawner in m_spawners)
            {
                StartCoroutine( spawner.Spawn() );
            }
            m_wavesNumber++;
        }
        m_ui.UpdateWaveLabel(m_wavesNumber, m_numWaves);
    }
    
    /// <summary>
    /// Tell when the round is over
    /// </summary>
    /// <returns>true if there are no more enemies on the field</returns>
    bool RoundComplete()
    {
        foreach(EnemySpawner spawner in m_spawners) //Spawners aren't done, round not done
        {
            if (spawner.EnemiesToSpawn > 0)
                return false;
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");  //Enemies on the field theyre not dead so its not done
        if (enemies.Length > 0)
            return false;

        return true;
    }

    /// <summary>
    /// Give values to global variables
    /// </summary>
    void SetVariables()
    {
        m_random = new System.Random(System.DateTime.Now.Millisecond);
        m_breakTimer = 0;
        m_ui = GameObject.Find("Menu Canvas").GetComponent<UIController>();

        m_endPoint = transform.Find("Goal");
        if (m_endPoint == null)
            Debug.LogWarning("Couldnt find the destination for this level");
    }

    // Start is called before the first frame update
    void Start()
    {
        SetVariables();

        m_loaded = true;
    }
}
