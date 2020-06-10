using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;

public class UIController : MonoBehaviour
{
    [SerializeField] protected Text m_waveLabel;

    public void CreatePrefab(string prefabName)
    {
        //Load from resources then instantiate copy - KC
        GameObject prefab = Resources.Load<GameObject>(prefabName);
        GameObject copy = GameObject.Instantiate(prefab, new Vector3(0, 1, 0), Quaternion.identity);
    }
    
    public void UpdateWaveLabel(int left, int still)
    {
        m_waveLabel.text = string.Format("Wave: {0}/{1}", left, still);
    }
}
