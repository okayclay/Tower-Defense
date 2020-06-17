using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

    [SerializeField] protected int m_totalWaves;
    [SerializeField] protected List<EnemySpawner> m_spawners = new List<EnemySpawner>();
    [SerializeField] protected float m_towerStartHealth;
    [SerializeField] protected float m_secsToBuild;

    protected ePhase    m_phase = ePhase.None;
    protected Transform m_endPoint;
    protected bool      m_loaded = false;
    protected float     m_breakTimer;
    protected int       m_waveNum;
    protected float     m_curHealth;
    protected Image     m_towerHealthBar;

    public Transform         EndPoint    {  get { return m_endPoint; } }
    public bool              Loaded      {  get { return m_loaded; } }
    public ePhase            Phase       {  get { return m_phase; } }

    public float TowerHealth {  get { return m_curHealth; } }

    // TO DO - Set different nav mesh types for placeables 
    /// <summary>
    /// Start the wave after letting the player place defenses
    /// </summary>
    protected void BeginWave()
    {
        if(m_waveNum < m_totalWaves)
        {
            foreach(EnemySpawner spawner in m_spawners)
            {
                StartCoroutine( spawner.Spawn() );
            }
            m_waveNum++;
        }

        StopTimer();
        GameEngine.UI.UpdateWaveLabel(m_waveNum, m_totalWaves);
        GameEngine.UI.ToggleMenu("Mid Game Menu", false);
    }
    
    protected void Build()
    {
        m_breakTimer = m_secsToBuild;
        GameEngine.UI.UpdateDefensesButton(10);
        GameEngine.UI.ToggleMenu("Mid Game Menu", true);
    }

    /// <summary>
    /// Places towers on the field
    /// </summary>
    /// <param name="tower">Which tower</param>
    public void CreateTower(Towers tower)
    {
        //Load from resources then instantiate copy - KC
        GameObject prefab = Resources.Load<GameObject>(tower.name);
        GameObject.Instantiate(prefab, new Vector3(0, 1, 0), Quaternion.identity);
        GameEngine.User.UpdateCoins(-tower.Cost);
        GameEngine.UI.UpdateDefensesButton(tower.Cost);
    }

    /// <summary>
    /// Called from Inspector
    /// </summary>
    /// <param name="phase">which phase to change to</param>
    public void ChangePhase(string phase)
    {
        switch(phase.ToLower())
        {
            case "build":   ChangePhase(ePhase.Build);          break;
            case "play":    ChangePhase(ePhase.Play);           break;
            default:        Debug.LogWarning("Invalid phase");  break;
        }
    }

    /// <summary>
    /// Called from string function to actually change the phase
    /// </summary>
    /// <param name="phase">Phase changing to</param>
    protected void ChangePhase(ePhase phase)
    {
        m_phase = phase;

        switch(phase)
        {
            case ePhase.Build:
                Build();
                break;

            case ePhase.Play:
                BeginWave();
                break;
        }
        Debug.LogFormat("Changing to {0} mode", m_phase);
    }

    /// <summary>
    ///StartMenu's button callback
    /// </summary>
    public void Play()
    {
        GameEngine.UI.ToggleMenu("Mid Game Menu", false);
        GameEngine.UI.UpdateWaveLabel(m_waveNum, m_totalWaves);
        GameEngine.UI.UpdateCoins();
        GameEngine.UI.UpdateDefensesButton(10);
        ChangePhase(ePhase.Build);
    }

    /// <summary>
    /// Give values to global variables
    /// </summary>
    void SetVariables()
    {
        GameEngine g = new GameEngine();
        m_breakTimer = 0;
        m_curHealth = m_towerStartHealth;

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

    /// <summary>
    /// Stops the countdown so the round can begin
    /// </summary>
    void StopTimer()
    {
        m_breakTimer = 0;
    }

    /// <summary>
    /// Checks if theres still time left to build
    /// </summary>
    /// <returns>true if the time is up</returns>
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
                if(WaveComplete())
                {
                    ChangePhase(ePhase.Build);
                }
                break;

            case ePhase.Build:
                if (!TimesUp())
                {
                    m_breakTimer -= Time.deltaTime;
                    GameEngine.UI.UpdateTimer(m_breakTimer);
                }
                else
                    ChangePhase(ePhase.Play);
                break;
        }
    }

    public void UpdateTowerHealth(int amount)
    {
        m_curHealth += amount;

        if (m_curHealth <= 0)
            m_curHealth = 0;

        UpdateTowerHealthUI();     
    }

    protected void UpdateTowerHealthUI()
    {
        Vector2 v2;
        v2.y = .5f;
        v2.x = (m_curHealth / m_towerStartHealth) * 3;
        m_towerHealthBar.rectTransform.sizeDelta = v2;

        if (m_curHealth > (m_towerStartHealth / 2))
            m_towerHealthBar.color = Color.green;
        else
        {
            if (m_curHealth <= (m_towerStartHealth * .33f))
                m_towerHealthBar.color = Color.red;
            else
                m_towerHealthBar.color = Color.yellow;
        }
    }

    /// <summary>
    /// Tell when the round is over
    /// </summary>
    /// <returns>true if there are no more enemies on the field</returns>
    bool WaveComplete()
    {
        foreach (EnemySpawner spawner in m_spawners) //Spawners aren't done, round not done
        {
            if (spawner.EnemiesToSpawn > 0)
                return false;
        }

        if (m_curHealth <= 0)
            return true;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");  //Enemies on the field theyre not dead so its not done
        if (enemies.Length > 0)
            return false;

        return true;
    }
}