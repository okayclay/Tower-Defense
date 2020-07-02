using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameEngine
{
    private static System.Random    m_random;

    protected static LevelManager m_level;
    protected static UserProfile m_user;
    protected static UIController m_ui;

    protected string m_profilePath;

    public static LevelManager  Level       {  get { return m_level; } }
    public static System.Random Randomizer  { get { return m_random; } }
    public static UIController  UI          { get { return m_ui; } }
    public static UserProfile   User        {  get { return m_user; } }

    public GameEngine()
    {
        m_user = new UserProfile();
        m_random = new System.Random(System.DateTime.Now.Millisecond);

        GrabClasses();

        if (GetProfileData())
        {
            m_user.LoadProfile(m_profilePath);
        }
        else
        {
            m_user.CreateProfile();
        }
    }

    /// <summary>
    /// Looks for previous save game
    /// </summary>
    /// <returns>true if an xml file is found</returns>
    protected bool GetProfileData()
    {
#if UNITY_WEBGL

        foreach (TextAsset profile in Resources.LoadAll<TextAsset>( "Profiles" ))
            m_profiles.Add( profile.name );
#elif UNITY_ANDROID || UNITY_IOS
        System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Application.persistentDataPath);

        foreach (var path in dir.GetFiles("*.xml"))
        {
            string[] splitDir = path.ToString().Split('\\');
            string m_temp = splitDir[splitDir.Length - 1];
            m_profilePath = m_temp.Remove(m_temp.Length - 4);
            return true;
        }

        return false;
#endif
    }

    protected void GrabClasses()
    {
        m_ui = GameObject.Find("Menu Canvas").GetComponent<UIController>();
        if (m_ui != null)
            m_ui.Init();
        else
            Debug.LogError("Coudln't find UI");

        m_level = GameObject.Find("Level").GetComponent<LevelManager>();
    }
    
    /// <summary>
    /// The level ended, either proceed to next one or give player a chance to redo this one
    /// </summary>
    /// <param name="won">If the level eneded because the player survived all the waves</param>
    public void LevelOver(bool won)
    {
        Time.timeScale = 0;
        m_ui.ShowAnnoucement(won);
    }
}