using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] protected int      m_level;        //Related to ammo type
    [SerializeField] protected float    m_damage;       //Higher the level, higher the damage
    [SerializeField] protected int      m_cost;         //Higher cost too
    [SerializeField] protected int      m_lineOfSight;  //Maybe larger line of sight, idk

    protected Enemy m_closestEnemy; //The target

    /// <summary>
    /// Make an invisible sphere and anything that walks in gets shot
    /// </summary>
    protected void CheckDistanceFromEnemy()
    {
        Collider[] collisions = Physics.OverlapSphere(transform.position, m_lineOfSight);
        foreach(Collider collider in collisions)
        {
            switch(collider.tag)
            {
                case "Enemy":
                    m_closestEnemy = collider.GetComponent<Enemy>();
                    break;
            }
        }
    }

    protected void FixedUpdate()
    {
        CheckDistanceFromEnemy();
    }
    
    /// <summary>
    /// Debug. Shows the sphere youre working with
    /// </summary>
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position, m_lineOfSight);
    }
}