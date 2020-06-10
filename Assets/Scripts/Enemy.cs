using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    enum Mode
    {
        None,
        Move,
        Attack,
        Death
    }

    Animator    m_anim;
    Mode        m_mode = Mode.None;

    [SerializeField] protected float m_startingHealth;
    [SerializeField] protected Image m_healthBar;

    protected float         m_curHealth;
    protected NavMeshAgent  m_agent;
    protected bool          m_pathSet = false;

    /// <summary>
    /// Get the enemy to follow the path
    /// </summary>
    void BeginWalking()
    {
        ChangeMode(Mode.Move);
        m_pathSet = true;
    }

    /// <summary>
    /// Switches animation 
    /// </summary>
    /// <param name="newMode">the next animaion to play</param>
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

            case Mode.Death:
                m_anim.SetBool("Run", false);
                m_anim.SetBool("Attack", false);
                m_anim.SetTrigger("Die");
                Die();
                break;
        }
    }

    /// <summary>
    /// Stop walking and play death animation
    /// </summary>
    void Die()
    {
        m_agent.isStopped = true;
        Destroy(gameObject, 6f);
    }

    /// <summary>
    /// Is the enemy at the destination
    /// </summary>
    /// <returns>true if reached the end</returns>
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

    void OnCollisionEnter(Collision collision)
    {
        switch(collision.transform.tag)
        {
            case "Projectile":
                UpdateHealth( -collision.transform.GetComponent<Projectile>().Damage );
                break;
        }
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        m_anim      = GetComponent<Animator>();
        m_curHealth = m_startingHealth;
        m_agent     = GetComponent<NavMeshAgent>();

        yield return new WaitUntil(() => LevelManager.Loaded);

        BeginWalking();
    }

    void Update()
    {
        switch(m_mode)
        {
            case Mode.Move:
                if (FinishedPath())
                    ChangeMode(Mode.Attack);
                break;
        }
    }

    void UpdateHealth(float amount)
    {
        m_curHealth += amount;

        if (m_curHealth <= 0)
            ChangeMode(Mode.Death);

        //Now the UI
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        Vector2 v2;
        v2.y = .5f;
        v2.x = (m_curHealth / m_startingHealth) * 3;
        m_healthBar.rectTransform.sizeDelta = v2;

        if (m_curHealth > (m_startingHealth / 2))
            m_healthBar.color = Color.green;
        else
        {
            if (m_curHealth <= (m_startingHealth * .33f))
                m_healthBar.color = Color.red;
            else
                m_healthBar.color = Color.yellow;
        }
    }
}
