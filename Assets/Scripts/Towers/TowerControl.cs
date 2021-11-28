using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float attackRadius;
    [SerializeField] private Projectile projectile;
    [SerializeField] private Enemy targetEnemy = null;
    [SerializeField] private float attackCounter;


    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();

        foreach (Enemy enemy in Manager.Instance.EnemyList)
        {
            if (Vector2.Distance(transform.position,enemy.transform.position) <= attackRadius)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }

    private Enemy GetNearestEnemy()
    {
        Enemy nearestEnemy = null;
        float smallestDistance = float.PositiveInfinity;

        foreach (Enemy enemy in GetEnemiesInRange())
        {
            if (Vector2.Distance(transform.position, enemy.transform.position) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.position, enemy.transform.position);
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }
}
