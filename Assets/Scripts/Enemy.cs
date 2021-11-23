using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int target = 0;

    [SerializeField] private Transform exit;
    [SerializeField] private Transform[] wayPoints;
    [SerializeField] private float navigation;

    private Transform enemy;
    private float navigationTime = 0;



    private void Start()
    {
        enemy = GetComponent<Transform>();
    }



    private void Update()
    {
        if (wayPoints != null)
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
            Manager.Instance.removeEnemyFromScreen();
            Destroy(gameObject);
        }
    }
}
