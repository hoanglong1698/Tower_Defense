using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{   
    public TowerBtn ClickedBtn { get; set; }

    private int currency;

    public int Currency
    {
        get
        {
            return currency;
        }

        set
        {
            this.currency = value;
            this.currencyTxt.text = value.ToString() + " <color=yellow>$</color>";
        }
    }

    [SerializeField] private Text currencyTxt;

    public ObjectPool Pool { get; set; }

    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
    }

    private int wave = 0;

    [SerializeField] private Text waveTxt;

    [SerializeField] private GameObject waveBtn;

    public bool WaveActive { get => activeMonsters.Count > 0; }

    private List<Monster> activeMonsters = new List<Monster>();

    private bool gameOver = false;

    [SerializeField] private GameObject gameOverMenu;

    private int lives;

    [SerializeField] private Text livesTxt;
    public int Lives
    {
        get
        {
            return lives;
        }

        set
        {
            this.lives = value;

            livesTxt.text = lives.ToString();

            if (lives <= 0)
            {
                this.lives = 0;

                GameOver();
            }
        }
    }

    private Tower selectedTower;

    // Start is called before the first frame update
    void Start()
    {
        Lives = 5;
        Currency = 100;
    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape();
    }

    public void PickTower(TowerBtn towerBtn)
    {
        if (Currency >= towerBtn.Price && !WaveActive)
        {
            this.ClickedBtn = towerBtn;
            Hover.Instance.Activate(towerBtn.Sprite);
        }
    }

    public void BuyTower()
    {
        if (Currency >= ClickedBtn.Price)
        {
            Currency -= ClickedBtn.Price;
            Hover.Instance.Deactivate();
        }
    }

    private void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Hover.Instance.Deactivate();
        }
    }

    public void StartWave()
    {
        wave++;

        waveTxt.text = string.Format("Wave: <color=lime>{0}</color>", wave);

        StartCoroutine(SpawnWave());

        waveBtn.SetActive(false);
    }

    private IEnumerator SpawnWave()
    {
        LevelManager.Instance.GeneratePath();

        for (int i = 0; i < wave; i++)
        {
            int monsterIndex = Random.Range(0, 4);

            string type = string.Empty;

            switch (monsterIndex)
            {
                case 0:
                    type = "BlueMonster";
                    break;
                case 1:
                    type = "RedMonster";
                    break;
                case 2:
                    type = "GreenMonster";
                    break;
                case 3:
                    type = "PurpleMonster";
                    break;
            }

            Monster monster = Pool.GetObject(type).GetComponent<Monster>();
            monster.Spawn();

            activeMonsters.Add(monster);

            yield return new WaitForSeconds(2.5f);
        }
        //    monster.Spawn(health);

        //    if (wave % 3 == 0)
        //    {
        //        health += 5;
        //    }
    }

    public void RemoveMonster(Monster monster)
    {
        activeMonsters.Remove(monster);

        if (!WaveActive && !gameOver)
        {
            waveBtn.SetActive(true);
        }
    }

    public void GameOver()
    {
        if (!gameOver)
        {
            gameOver = true;

            gameOverMenu.SetActive(true);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SelectTower(Tower tower)
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = tower;

        selectedTower.Select();

        //sellText.text = "+ " + (selectedTower.Price / 2);

        //upgradePanel.SetActive(true);
    }

    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }

        //upgradePanel.SetActive(false);

        selectedTower = null;
    }
}
