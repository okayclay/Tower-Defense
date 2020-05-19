using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    MeshRenderer m_meshRenderer;

    // Start is called before the first frame update
    private Color m_hoverColor = Color.yellow;
    private Color m_originalColor;

    //---------------Drag variables---------------//
    private Vector3 m_startingPos   = new Vector3();
    private Vector3 m_offset        = new Vector3();
    private Vector3 m_screenPoint   = new Vector3();
    private Vector3 m_curPos        = new Vector3();

    private void OnMouseEnter()
    {
        m_meshRenderer.material.color = m_hoverColor;
    }

    private void OnMouseExit()
    {
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
        transform.position = m_curPos;
    }

    void Start()
    {
        m_meshRenderer = GetComponent<MeshRenderer>();

        if(m_meshRenderer != null)
            m_originalColor = m_meshRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

