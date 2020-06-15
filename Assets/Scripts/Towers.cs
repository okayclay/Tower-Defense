using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Towers : Draggable
{
    public enum ElementTypes
    {
        None,   //Default
        Fire,
        Electric,
        Ice,
        Biological
    }

    [SerializeField] protected int          m_level;        //Related to ammo type
    [SerializeField] protected int          m_cost;         //Higher cost too
    [SerializeField] protected int          m_lineOfSight;  //Maybe larger line of sight, idk

    [SerializeField] protected GameObject   m_ammoPrefab;
    [SerializeField] protected float        m_shootPower;
    [SerializeField] protected float        m_shootDelay;

    public int Cost {  get { return m_cost; } }

    protected float     m_shootTimer;
    protected Transform m_shootNode;
    protected Transform m_closestEnemy; //The target
    
    protected void Attack()
    {
        m_shootTimer += Time.deltaTime;
        transform.LookAt(m_closestEnemy.transform);

        if (m_shootTimer > m_shootDelay)
        {
            GameObject bullet = Instantiate(m_ammoPrefab, m_shootNode.position, Quaternion.identity );      //Create the bullet 
            bullet.GetComponent<Rigidbody>().velocity = transform.TransformDirection(0, 0, m_shootPower);   //Shoot
            m_shootTimer = 0;                                                                               //Reset timer
        }
    }

    /// <summary>
    /// Makes a invisible sphere around the tower, and whatever enemy steps in is the target
    /// </summary>
    /// <returns>if an enemy is nearby</returns>
    protected bool CheckDistanceFromEnemy()
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, m_lineOfSight);
        foreach(Collider collider in collisions)
        {
            switch(collider.tag)
            {
                case "Enemy":
                    if (!collider.GetComponent<Enemy>().IsDead)
                    {
                        m_closestEnemy = collider.transform;
                        return true;
                    }
                    break;
            }
        }
        return false;
    }

    protected void FixedUpdate()
    {
        if(CheckDistanceFromEnemy())
        {
            Attack();
        }
    }
    
    /// <summary>
    /// Debug. Shows the sphere youre working with
    /// </summary>
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, m_lineOfSight);
    }

    protected void Start()
    {
        m_shootNode = transform.Find("Shoot Node");
    }
}