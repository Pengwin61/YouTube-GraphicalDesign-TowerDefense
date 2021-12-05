using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum gameStatus
{
    next,
    play,
    gameover,
    win
}


public class Manager : Loader<Manager>
{
    private const float spawnDelay = 0.5f;


    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private Enemy[] enemies;


    [SerializeField] private Text totalMoneyLabel;
    [SerializeField] private Text currentWaveLabel;
    [SerializeField] private Text playButtonLabel;
    [SerializeField] private Button playButton;
    [SerializeField] private Text totalEscapedLabel;

    [Header("Wave")]
    [SerializeField] private int totalWaves = 5;
    [SerializeField] private int waveNumber = 0;

    [Header("Enemy")]
    [SerializeField] private int totalEnemies = 5;
    [SerializeField] private int enemiesPerSpawn;



    
    [Header("In Inspector")]
   [SerializeField] private int totalMoney = 10;
   [SerializeField] private int totalEscaped = 0;
   [SerializeField] private int roundEscaped = 0;
   [SerializeField] private int totalKilled = 0;


    private int enemiesToSpawn = 0;



    AudioSource audioSource;

    public gameStatus currentState = gameStatus.play;


    public List<Enemy> EnemyList = new List<Enemy>();



    public int TotalEscaped
    {
        get
        {
            return totalEscaped;
        }
        set
        {
            totalEscaped = value;
        }

    }

    public int RoundEscaped
    {
        get
        {
            return roundEscaped;
        }
        set
        {
            roundEscaped = value;
        }
    }

    public int TotalKilled
    {
        get
        {
            return totalKilled;
        }
        set
        {
            totalKilled = value;
        }
    }

    public int TotalMoney
    {
        get
        {
            return totalMoney;
        }
        set
        {
            totalMoney = value;
            totalMoneyLabel.text = TotalMoney.ToString();
        }
    }

    public AudioSource AudioSource
    {
        get
        {
            return audioSource;
        }
    }


    private void Start()
    {
        playButton.gameObject.SetActive(false);
        ShowMenu();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        HandleEscape();
    }


    IEnumerator Spawn()
    {
        if (enemiesPerSpawn > 0 && EnemyList.Count < totalEnemies)
        {
            for (int i = 0; i < enemiesPerSpawn; i++)
            {
                if (EnemyList.Count < totalEnemies)
                {
                    Enemy newEnemy = Instantiate(enemies[Random.Range(0,enemiesToSpawn)]) as Enemy;
                    newEnemy.transform.position = spawnPoint.transform.position;
                }
            }

            yield return new WaitForSeconds(spawnDelay);
            StartCoroutine(Spawn());
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        EnemyList.Add(enemy);
    }

    public void UnregisterEnemy(Enemy enemy)
    {
        EnemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    
    public void DestroyEnemies()
    {
        foreach (Enemy enemy in EnemyList)
        {
            Destroy(enemy.gameObject);
        }
        EnemyList.Clear();
    }

    public void addMoney(int amount)
    {
        TotalMoney += amount;
    }

    public void subtractMoney(int amount)
    {
        TotalMoney -= amount;
    }
    

    public void IsWaveOver()
    {
        totalEscapedLabel.text = "Escaped " + TotalEscaped + " / 10";

        if ((RoundEscaped + TotalKilled) == totalEnemies)
        {
            if (waveNumber <= enemies.Length)
            {
                enemiesToSpawn = waveNumber;
            }
            
            SetCurrentGameState();
            ShowMenu();
        }
    }

    public void SetCurrentGameState()
    {
        if (totalEscaped >= 10)
        {
            currentState = gameStatus.gameover;
            Debug.Log("ÂÛ ÏÐÎÈÃÐÀËÈ");
        }
        else if (waveNumber == 0 && (RoundEscaped + TotalKilled) == 0)
        {
            currentState = gameStatus.play;
        }
        else if (waveNumber >= totalWaves)
        {
            currentState = gameStatus.win;
            Debug.Log("Âîëíû çàêîí÷èëèñü, ÂÛ ÂÛÉÃÐÀËÈ");
        }
        else
        {
            currentState = gameStatus.next;
            Debug.Log("Ñëåäóþùàÿ âîëíà");
        }
    }

    public void PlayButtonPressed()
    {
        switch (currentState)
        {
            case gameStatus.next:
                waveNumber += 1;
                totalEnemies += waveNumber;
                break;

            default:
                totalEnemies = 5;
                TotalEscaped = 0;
                TotalMoney = 10;
                enemiesToSpawn = 0;
                TowerManager.Instance.DestroyAllTowers();
                TowerManager.Instance.RenameTagBuildSide();
                totalMoneyLabel.text = TotalMoney.ToString();
                totalEscapedLabel.text = "Escaped " + TotalEscaped + " / 10";
                audioSource.PlayOneShot(SoundManager.Instance.NewGame);
                break;
        }
        DestroyEnemies();
        TotalKilled = 0;
        RoundEscaped = 0;
        currentWaveLabel.text = "Wave " + (waveNumber + 1);
        StartCoroutine(Spawn());
        playButton.gameObject.SetActive(false);
    }

    public void ShowMenu()
    {
        switch (currentState)
        {
            case gameStatus.next:
                playButtonLabel.text = "Next wave";
                break;
            case gameStatus.play:
                playButtonLabel.text = "Play Game";
                break;
            case gameStatus.gameover:
                playButtonLabel.text = "Game Over";
                AudioSource.PlayOneShot(SoundManager.Instance.GameOver);
                break;
            case gameStatus.win:
                playButtonLabel.text = "Win!";
                break;
            default:
                break;
        }
        playButton.gameObject.SetActive(true);
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TowerManager.Instance.DisableDrag();
            TowerManager.Instance.towerBtnPressed = null;
        }
    }
}
