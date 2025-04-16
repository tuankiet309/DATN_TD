using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPortal : MonoBehaviour
{
    [SerializeField] private float spawnCooldown;
    private float spawnTimer;

    [Space]

    [SerializeField] private List<Waypoint> waypointList;

    private List<GameObject> enemiesToCreate = new List<GameObject>();
    private List<GameObject> activeEnemies = new List<GameObject>();

    private void Awake()
    {
        CollectWaypoints();
    }

    private void Update()
    {
        if (CanMakeNewEnemy())
            CreateEnemy();
    }

    private bool CanMakeNewEnemy()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0 && enemiesToCreate.Count > 0)
        {
            spawnTimer = spawnCooldown;
            return true;
        }

        return false;
    }


    private void CreateEnemy()
    {
        GameObject randomEnemy = GetRandomEnemy();
        GameObject newEnemy = Instantiate(randomEnemy, transform.position, Quaternion.identity);

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.SetupEnemy(waypointList, this);

        activeEnemies.Add(newEnemy);
    }

    private GameObject GetRandomEnemy()
    {
        int randomIndex = Random.Range(0, enemiesToCreate.Count);
        GameObject choosenEnemy = enemiesToCreate[randomIndex];

        enemiesToCreate.Remove(choosenEnemy);

        return choosenEnemy;
    }

    public void AddEnemy(GameObject enemyToAdd) => enemiesToCreate.Add(enemyToAdd);
    public void RemoveActiveEnemy(GameObject enemyToRemove)
    {
        if(activeEnemies.Contains(enemyToRemove))
            activeEnemies.Remove(enemyToRemove);
    }

    public List<GameObject> GetActiveEnemies() => activeEnemies;


    [ContextMenu("Collect waypoints")]
    private void CollectWaypoints()
    {
        waypointList = new List<Waypoint>(); 

        foreach (Transform child in transform)
        {
            Waypoint waypoint = child.GetComponent<Waypoint>();

            if(waypoint != null)
                waypointList.Add(waypoint);
        }
    }
}
