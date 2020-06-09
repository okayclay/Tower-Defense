using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    enum Mode
    {
        None,
        Move,
        Attack
    }

    Animator    m_anim;
    Mode        m_mode = Mode.None;

    [SerializeField] protected float m_startingHealth;

    protected float         m_curHealth;
    protected NavMeshAgent  m_agent;
    protected bool          m_pathSet = false;

    void BeginWalking()
    {
        ChangeMode(Mode.Move);
        m_pathSet = true;
    }

    void ChangeMode(Mode newMode)
    {
        m_mode = newMode;

        switch(newMode)
        {
            case Mode.Move:
                m_anim.SetBool("Run", true);
                m_agent.SetDestination(LevelManager.EndPoint.position);
                break;

            case Mode.Attack:
                m_anim.SetBool("Run", false);
                m_anim.SetBool("Attack", true);
                break;
        }
    }

    bool FinishedPath()
    {
        if (!m_agent.pathPending && m_pathSet)   //no longer making the path
        {
            if (m_agent.remainingDistance <= m_agent.stoppingDistance)  //at stopping distance
            {
                if (!m_agent.hasPath || m_agent.velocity.sqrMagnitude == 0f)    //no more path because you made it to the end
                {
                    return true;
                }
            }
        }

        return false;
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
        switch(m_mode)
        {
            case Mode.Move:
                if (FinishedPath())
                    ChangeMode(Mode.Attack);
                break;
        }
    }
}
