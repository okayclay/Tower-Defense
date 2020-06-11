using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;

public class UIController : MonoBehaviour
{
    [SerializeField] protected Text m_coinText;

    protected Text m_waveLabel;
    protected Text m_breakText;
    protected GameObject m_midGameMenu;

    public void CreatePrefab(string prefabName)
    {
        //Load from resources then instantiate copy - KC
        GameObject prefab = Resources.Load<GameObject>(prefabName);
        GameObject copy = GameObject.Instantiate(prefab, new Vector3(0, 1, 0), Quaternion.identity);
    }

    public void Init()
    {
        m_midGameMenu = transform.Find("Mid Game").gameObject;
        m_waveLabel = transform.Find("Wave Label").GetComponent<Text>();
        m_breakText = transform.Find("Time Label").GetComponent<Text>();
    }

    public void UpdateWaveLabel(int left, int still)
    {
        m_waveLabel.text = string.Format("Wave: {0}/{1}", left, still);
    }

    public void UpdateTimer(float time)
    {
        m_breakText.text = string.Format("{0:#} seconds until wave begins!", time);
    }
}