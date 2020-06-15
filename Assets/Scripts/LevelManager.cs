﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public enum ePhase
    {
        None,
        Build,
        Play
    }

    private static System.Random m_random;
    protected static GameEngine m_engine;

    [SerializeField] protected int m_totalWaves;
    [SerializeField] protected List<EnemySpawner> m_spawners = new List<EnemySpawner>();
    [SerializeField] protected int m_towerHealth;
    [SerializeField] protected float m_secsToBuild;

    protected static UIController m_ui;

    protected ePhase            m_phase = ePhase.None;
    protected static Transform  m_endPoint;
    protected static bool       m_loaded = false;
    protected float             m_breakTimer;
    protected int               m_waveNum;
    protected Image             m_towerHealthBar;

    public static Transform         EndPoint    {  get { return m_endPoint; } }
    public static bool              Loaded      {  get { return m_loaded; } }
    public static System.Random     Randomizer  {  get { return m_random; } }
    public static UIController      UI          {  get { return m_ui; } }

    // TO DO - Set different nav mesh types for placeables 
    /// <summary>
    /// Start the wave after letting the player place defenses
    /// </summary>
    protected void BeginRound()
    {
        if(m_waveNum < m_totalWaves)
        {
            foreach(EnemySpawner spawner in m_spawners)
            {
                StartCoroutine( spawner.Spawn() );
            }
            m_waveNum++;
        }
        m_ui.UpdateWaveLabel(m_waveNum, m_totalWaves);
    }
    
    public void CreateTower(Towers tower)
    {
        //Load from resources then instantiate copy - KC
        GameObject prefab = Resources.Load<GameObject>(tower.name);
        GameObject copy = GameObject.Instantiate(prefab, new Vector3(0, 1, 0), Quaternion.identity);
        GameEngine.User.BuyDefense(tower.Cost);
        m_ui.UpdateDefensesButton(tower.Cost);
        m_ui.UpdateCoins();
    }

    public void ChangePhase(string phase)
    {
        switch(phase.ToLower())
        {
            case "build":   ChangePhase(ePhase.Build);          break;
            case "play":    ChangePhase(ePhase.Play);           break;
            default:        Debug.LogWarning("Invalid phase");  break;
        }
    }

    protected void ChangePhase(ePhase phase)
    {
        m_phase = phase;

        switch(phase)
        {
            case ePhase.Build:
                m_breakTimer = m_secsToBuild;
                break;

            case ePhase.Play:
                BeginRound();
                break;
        }
    }

    public void Play()
    {
        m_ui.UpdateWaveLabel(m_waveNum, m_totalWaves);
        m_ui.UpdateCoins();
        m_ui.UpdateDefensesButton(10);
        ChangePhase(ePhase.Build);
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
        m_engine = new GameEngine();

        m_random = new System.Random(System.DateTime.Now.Millisecond);
        m_breakTimer = 0;

        m_ui = GameObject.Find("Menu Canvas").GetComponent<UIController>();
        if (m_ui == null)
            Debug.LogWarning("Didn't find UI controller");
        else
            m_ui.Init();

        m_endPoint = transform.Find("Goal");
        if (m_endPoint == null)
            Debug.LogWarning("Couldnt find the destination for this level");
        else
            m_towerHealthBar = m_endPoint.Find("Canvas/Health Background/Health").GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetVariables();
        m_loaded = true;
    }

    void StopTimer()
    {
        m_breakTimer = 0;
    }

    bool TimesUp()
    {
        if (m_breakTimer <= 0)
            return true;

        return false;
    }

    private void Update()
    {
        switch(m_phase)
        {
            case ePhase.Play:
                if(RoundComplete())
                {
                    ChangePhase(ePhase.Build);
                }
                break;

            case ePhase.Build:
                if (!TimesUp())
                {
                    m_breakTimer -= Time.deltaTime;
                    m_ui.UpdateTimer(m_breakTimer);
                }
                else
                    ChangePhase(ePhase.Play);
                break;
        }
    }

    public void UpdateTowerHealth()
    {

    }
}