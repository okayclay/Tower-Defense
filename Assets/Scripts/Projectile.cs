using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float m_damage;

    public float Damage {  get { return m_damage; } }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.transform.tag)
        {
            case "Enemy":
                Destroy(this.gameObject);
                break;
        }
    }

    protected void Start()
    {
        Destroy(this.gameObject, 3f);
    }
}