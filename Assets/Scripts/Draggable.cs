using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    MeshRenderer m_meshRenderer;

    // Start is called before the first frame update
    private Color m_hoverColor = Color.yellow;
    private Color m_originalColor = Color.white;

    #region[Drag variables]
    private Vector3 m_startingPos   = new Vector3();
    private Vector3 m_offset        = new Vector3();
    private Vector3 m_screenPoint   = new Vector3();
    private Vector3 m_curPos        = new Vector3();
    #endregion
    //TODO - Tighten up the drag positioning
    //TODO - Have it snap to a grid
    private void OnMouseEnter()
    {
        if (m_meshRenderer == null)
            m_meshRenderer = GetComponent<MeshRenderer>();

        if(GameEngine.Level.Phase == LevelManager.ePhase.Build)
            m_meshRenderer.material.color = m_hoverColor;
    }

    private void OnMouseExit()
    {
        if (m_meshRenderer == null)
            m_meshRenderer = GetComponent<MeshRenderer>();

        if (GameEngine.Level.Phase == LevelManager.ePhase.Build)
            m_meshRenderer.material.color = m_originalColor;
    }
    
    void OnMouseDown()
    {
        m_startingPos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        m_offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_startingPos.z));
    }

    void OnMouseDrag()
    {
        m_screenPoint.x = Mathf.Round(Input.mousePosition.x);
        m_screenPoint.y = Mathf.Round(Input.mousePosition.y);
        m_screenPoint.z = Mathf.Round(m_startingPos.z);

        m_curPos = Camera.main.ScreenToWorldPoint(m_screenPoint) - m_offset;
        m_curPos.y = transform.position.y;

        if (GameEngine.Level.Phase == LevelManager.ePhase.Build)
            transform.position = m_curPos;
    }

    void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();

        if (m_meshRenderer != null)
        {
            if (m_meshRenderer.material != null)
                m_originalColor = m_meshRenderer.material.color;
            else
                Debug.LogWarningFormat("No mesh renderer material for ", transform.name);
        }
        else
            Debug.LogWarningFormat("No mesh renderer found for ", transform.name);
    }
}