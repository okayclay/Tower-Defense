using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Animator m_anim;

    [SerializeField] protected float m_startingHealth;

    protected float m_curHealth;

    // Start is called before the first frame update
    void Start()
    {
        m_anim = GetComponent<Animator>();
        m_curHealth = m_startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            m_anim.SetBool("Walk", true);
        }
    }
}
