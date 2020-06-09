using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    Animator m_anim;

    [SerializeField] protected float m_startingHealth;

    protected float         m_curHealth;
    protected NavMeshAgent  m_agent;

    void BeginWalking()
    {
        m_anim.SetBool("Run", true);
        m_agent.SetDestination(LevelManager.EndPoint.position);
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        m_anim = GetComponent<Animator>();
        m_curHealth = m_startingHealth;
        m_agent = GetComponent<NavMeshAgent>();

        yield return new WaitUntil(() => LevelManager.Loaded);

        BeginWalking();
    }

    private void Update()
    {
        
        //m_anim.SetBool("Run", false);
    }
}
