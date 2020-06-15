using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine
{
    protected static UserProfile m_user;
    protected string m_profilePath;

    public static UserProfile User {  get { return m_user; } }

    public GameEngine()
    {
        m_user = new UserProfile();

        if(GetProfileData())
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
}