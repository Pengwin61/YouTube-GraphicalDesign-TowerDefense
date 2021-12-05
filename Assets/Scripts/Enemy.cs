using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int target = 0;

    [SerializeField] private Transform exit;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float navigation;

    [SerializeField] private int health;

    [SerializeField] private bool isDead = false;
    [SerializeField] private int rewardAmount;




    private Animator animator;
    private Collider2D enemyCollider;

    private Transform enemy;
    private float navigationTime = 0;


    public bool IsDead
    {
        get
        {
            return isDead;
        }
    }



    private void Start()
    {
        enemy = GetComponent<Transform>();
        enemyCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        Manager.Instance.RegisterEnemy(this);
    }



    private void Update()
    {
        if (wayPoints != null && isDead == false)
        {
            navigationTime += Time.deltaTime;
            if (navigationTime > navigation)
            {
                if (target < wayPoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, navigationTime);
                }

                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exit.position, navigationTime);
                }
                navigationTime = 0;
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MoviengPoint")
        {
            target += 1;
        }
        else if (collision.tag == "Finish")
        {
            Manager.Instance.RoundEscaped += 1;
            Manager.Instance.TotalEscaped += 1;
            Manager.Instance.UnregisterEnemy(this);
            Manager.Instance.IsWaveOver();
        }
        else if (collision.tag == "Projectile")
        {
            Projectile newP = collision.gameObject.GetComponent<Projectile>();
            EnemyHit(newP.AttackDamage);
            Destroy(collision.gameObject);
        }
    }


    public void EnemyHit(int hitPoints)
    {
        if (health - hitPoints > 0)
        {
            health -= hitPoints;
            //Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Hit);
            animator.Play("Hurt");
        }
        else
        {
            animator.SetTrigger("didDie");
            // die
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        enemyCollider.enabled = false;
        Manager.Instance.TotalKilled += 1;
        Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.Death);
        Manager.Instance.IsWaveOver();
        Manager.Instance.addMoney(rewardAmount);
    }
}
