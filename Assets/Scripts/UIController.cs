using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;

public class UIController : MonoBehaviour
{
    [SerializeField] protected Text m_coinText;
    [SerializeField] protected Text m_breakText;

    protected Text          m_waveLabel;
    protected GameObject    m_midGameMenu;
    protected Button        m_buildBtn;

    /// <summary>
    /// Set the values to variables
    /// </summary>
    public void Init()
    {
        m_midGameMenu = transform.Find("Mid Game").gameObject;
        m_waveLabel = transform.Find("Wave Label").GetComponent<Text>();
        m_buildBtn = m_midGameMenu.transform.Find("Button Holder/Defend").GetComponent<Button>();
    }

    public void ToggleMenu(string menu, bool enabled)
    {
        menu = menu.Replace(" ", string.Empty); //Remove any spaces
        switch(menu.ToLower())
        {
            case "midgamemenu":
                m_midGameMenu.SetActive(enabled);
                break;
        }
    }

    /// <summary>
    /// Update player coin amount
    /// </summary>
    public void UpdateCoins()
    {
        m_coinText.text = string.Format(" x {0}", GameEngine.User.Coins);
    }
    
    /// <summary>
    /// Update time left to build
    /// </summary>
    /// <param name="time">time left</param>
    public void UpdateTimer(float time)
    {
        m_breakText.text = string.Format("{0:#} seconds until wave begins!", time);
    }

    /// <summary>
    /// Keeps track of the wave count
    /// </summary>
    /// <param name="curWave">Wave number we're on</param>
    /// <param name="still">How many waves total</param>
    public void UpdateWaveLabel(int curWave, int still)
    {
        m_waveLabel.text = string.Format("Wave: {0}/{1}", curWave, still);
    }

    /// <summary>
    /// Cant build certain defenses if you cant afford ir
    /// </summary>
    /// <param name="amount">How much we need for a certain defense building</param>
    public void UpdateDefensesButton(int amount)
    {
        if (GameEngine.User.CanAfford(amount))
            m_buildBtn.interactable = true;
        else
            m_buildBtn.interactable = false;
    }
}