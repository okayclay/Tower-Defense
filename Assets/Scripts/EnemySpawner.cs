using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> m_prefabs = new List<GameObject>();
    [SerializeField] int m_totalEnemies;
    [SerializeField] float m_spawnDelay;

    protected int m_enemyCount;

    public int EnemiesToSpawn {  get { return m_enemyCount; } }

    protected void AddEnemies(int amount)
	{
        m_totalEnemies += amount;
	}

	public void ResetCount()
	{
        AddEnemies(m_totalEnemies++);
        m_enemyCount = m_totalEnemies;
	}

	public IEnumerator Spawn()
    {
        int index = GameEngine.Randomizer.Next(0, m_prefabs.Count);
        GameObject enemy = Instantiate(m_prefabs[index], transform.position, Quaternion.identity);

        yield return new WaitForSeconds(m_spawnDelay);

        if (m_enemyCount > 0)
        {
            StartCoroutine(Spawn());
            m_enemyCount--;
        }
    }
}