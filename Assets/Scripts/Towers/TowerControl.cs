using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerControl : MonoBehaviour
{
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float attackRadius;
    [SerializeField] private float attackCounter;

    [SerializeField] private Projectile projectile;
    [SerializeField] private Enemy targetEnemy = null;

    private bool isAttacking = false;



    private void Update()
    {
        attackCounter -= Time.deltaTime;

        // ????? ????????? ????
        if (targetEnemy == null || targetEnemy.IsDead)
        {
            Enemy nearestEnemy = GetNearestEnemy();
            if (nearestEnemy != null && Vector2.Distance(transform.position, nearestEnemy.transform.position) <= attackRadius)
            {
                targetEnemy = nearestEnemy;
            }
        }
        else
        {
            if (attackCounter <=0)
            {
                isAttacking = true;
                attackCounter = timeBetweenAttacks;
            }
            else
            {
                isAttacking = false;
            }
            
            if (Vector2.Distance(transform.position, targetEnemy.transform.position) > attackRadius)
            {
                targetEnemy = null;
            }
        }

    }

    public void FixedUpdate()
    {
        if (isAttacking == true)
        {
            Attack();
        }
    }

    public void Attack()
    {
        isAttacking = false;
        Projectile newProjectile = Instantiate(projectile) as Projectile;
        newProjectile.transform.localPosition = transform.localPosition;

        if (newProjectile.PType == projectileType.arrow)
        {
            Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Arrow);
        }
        else if (newProjectile.PType == projectileType.fireball)
        {
            Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.FireBall);
        }
        else if (newProjectile.PType == projectileType.rock)
        {
            Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Rock);
        }


        if (targetEnemy == null)
        {
            Destroy(newProjectile);
        }
        else
        {
            // move projectile to enemy
            StartCoroutine(MoveProjectile(newProjectile));
        }
    }

    IEnumerator MoveProjectile(Projectile projectile)
    {
        while (GetTargetDistance(targetEnemy) > 0.20f && projectile != null && targetEnemy != null)
        {
            var dir = targetEnemy.transform.localPosition - transform.localPosition;
            var angleDirection = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            projectile.transform.rotation = Quaternion.AngleAxis(angleDirection, Vector3.forward);
            projectile.transform.localPosition = Vector2.MoveTowards(projectile.transform.localPosition, targetEnemy.transform.localPosition, 5f * Time.deltaTime);

            yield return null;
        }

        if (projectile != null || targetEnemy == null)
        {
            Destroy(projectile);
        }
    }


    private float GetTargetDistance(Enemy thisEnemy)
    {
        if (thisEnemy == null)
        {
            thisEnemy = GetNearestEnemy();
            if (thisEnemy == null)
            {
                return 0f;
            }
        }


        return Mathf.Abs(Vector2.Distance(transform.localPosition, thisEnemy.transform.localPosition));
    }



    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();

        foreach (Enemy enemy in Manager.Instance.EnemyList)
        {
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) <= attackRadius)
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
            if (Vector2.Distance(transform.localPosition, enemy.transform.localPosition) < smallestDistance)
            {
                smallestDistance = Vector2.Distance(transform.localPosition, enemy.transform.localPosition);
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }
}
