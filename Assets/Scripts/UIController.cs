using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [System.Serializable]
    public struct Announcement
    {
        [SerializeField] public GameObject parent;
        [SerializeField] public Text text;
        [SerializeField] public Button next;
        [SerializeField] public Button retry;
    }

    [SerializeField] protected Text         m_coinText;
    [SerializeField] protected Text         m_breakText;
    [SerializeField] protected Announcement m_announcementUI;

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

    public void ShowAnnoucement(bool won)
	{
        switch(won)
		{
            case true: 
                m_announcementUI.text.text = "You cleared the level!";
                m_announcementUI.next.enabled = true;
                m_announcementUI.retry.enabled = false;
                break;

            case false: 
                m_announcementUI.text.text = "GAME OVER";
                m_announcementUI.next.enabled = false;
                m_announcementUI.retry.enabled = true;
                break;
		}
	}

    public void ToggleMenu(string menu, bool enabled)
    {
        GameObject menuObj;

        menu = menu.Replace(" ", string.Empty); //Remove any spaces
        menu = menu.ToLower();

        switch(menu.ToLower())
        {
            case "midgamemenu":
                m_midGameMenu.SetActive(enabled);
                break;

            case "mainmenu":
                menuObj = transform.Find("Main Menu").gameObject;
                if (menuObj != null)
                    menuObj.SetActive(enabled);
                else
                    Debug.LogWarning("Couldn't find the Main Menu gameobject");
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