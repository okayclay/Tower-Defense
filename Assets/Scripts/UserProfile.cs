using System.Xml;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UserProfile
{
    protected int m_coins;

    public int Coins {  get { return m_coins; } }
   
    /// <summary>
   /// Constructor
   /// </summary>
    public UserProfile()
    {
    }

    /// <summary>
    /// Create a new profile with default data
    /// </summary>
    /// <param name="coinAmount">default amount</param>
    public UserProfile CreateProfile(int coinAmount = 50)
    {
        m_coins = coinAmount;
        Debug.Log("New profile created");
        Save();

        return this;
    }

    public void BuyDefense(int cost)
    {
        m_coins -= cost;
    }

    /// <summary>
    /// Can it afford a certain defense building?
    /// </summary>
    /// <param name="cost">cost of the defense building</param>
    /// <returns>true if we can afford it</returns>
    public bool CanAfford(int cost)
    {
        if (m_coins >= cost)
            return true;

        return false;
    }

    /// <summary>
    /// Searches the data folder for a save file
    /// </summary>
    /// <param name="name">name of the save file we're looking for</param>
    public void LoadProfile(string name)
    {
        string curFile = Application.persistentDataPath + "/" + name + ".xml";
        string text = string.Empty;

#if UNITY_WEBGL

        TextAsset textAsset;
        textAsset = Resources.Load( "Profiles/" + name ) as TextAsset;
        text = textAsset.text;

#elif UNITY_ANDROID || UNITY_IOS

        if (File.Exists(curFile))
            text = File.ReadAllText(curFile);
#endif

        if (!string.IsNullOrEmpty(text))
        {
            XmlDocument document = new XmlDocument();
            document.LoadXml(text);
            ParseXML(document.DocumentElement);
            Debug.Log("Profile Loaded");
        }
        else
            Debug.LogErrorFormat("UserProfile.LoadUsers() error: {0}", name);
    }

    /// <summary>
    /// Load data from last save
    /// </summary>
    /// <param name="root">xml data's root element</param>
    protected void ParseXML(XmlElement root)
    {
        XmlElement coinElement;

        coinElement = root.SelectSingleNode("Coins") as XmlElement;

        if (coinElement != null)
        {
            m_coins = int.Parse(coinElement.GetAttribute("Amount"));
        }
        else
            Debug.LogWarning("Could not find user node in xml file");
    }

    /// <summary>
    /// This calls the save profile function
    /// </summary>
    public void Save()
    {
#if UNITY_ANDROID || UNITY_IOS
        SaveProfile(Application.persistentDataPath + "/Profile.xml");
#endif
    }
    
    /// <summary>
    /// Update and save xml save file
    /// </summary>
    protected void SaveProfile(string fileName)
    {
        XmlDocument doc = new XmlDocument();
        XmlElement profileElement, coinElement;

        profileElement = doc.CreateElement("User");

        coinElement = doc.CreateElement("Coins");
        coinElement.SetAttribute("Amount", m_coins.ToString());

        profileElement.AppendChild(coinElement);
        doc.AppendChild(profileElement);

        doc.Save(fileName);
        Debug.Log("Game saved");
    }
}