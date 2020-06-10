using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> m_prefabs = new List<GameObject>();
    [SerializeField] int m_enemyCount;
    [SerializeField] float m_spawnDelay;

    public int EnemiesToSpawn {  get { return m_enemyCount; } }

    public IEnumerator Spawn()
    {
        int index = LevelManager.Randomizer.Next(0, m_prefabs.Count);
        GameObject enemy = Instantiate(m_prefabs[index], transform.position, Quaternion.identity);

        yield return new WaitForSeconds(m_spawnDelay);

        if (m_enemyCount > 0)
        {
            StartCoroutine(Spawn());
            m_enemyCount--;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
