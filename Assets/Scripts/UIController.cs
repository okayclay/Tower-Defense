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
    protected GameObject    m_buildBtn;

    public void Init()
    {
        m_midGameMenu = transform.Find("Mid Game").gameObject;
        m_waveLabel = transform.Find("Wave Label").GetComponent<Text>();
        m_buildBtn = m_midGameMenu.transform.Find("Button Holder/Defend").gameObject;
    }

    public void UpdateCoins()
    {
        m_coinText.text = string.Format(" x {0}", GameEngine.User.Coins);
    }
    
    public void UpdateTimer(float time)
    {
        m_breakText.text = string.Format("{0:#} seconds until wave begins!", time);
    }

    public void UpdateWaveLabel(int left, int still)
    {
        m_waveLabel.text = string.Format("Wave: {0}/{1}", left, still);
    }

    public void UpdateDefensesButton(int amount)
    {
        if (GameEngine.User.CanAfford(amount))
            m_buildBtn.SetActive(true);
        else
            m_buildBtn.SetActive(false);
    }
}