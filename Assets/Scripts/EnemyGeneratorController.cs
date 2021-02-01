using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneratorController : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float generatorTimer = 2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }

    public void StartGenerator()
    {
        InvokeRepeating("CreateEnemy", 1.5f, generatorTimer);
    }

    public void CancelGenerator(bool clean = false)
    {
        CancelInvoke("CreateEnemy");
        if (clean) {
            Object[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in allEnemies) {
                Destroy(enemy);
            }
        }
    }
}
