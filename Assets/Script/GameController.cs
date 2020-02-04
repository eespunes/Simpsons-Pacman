using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int normal, initialNormal, special, initialSpecial;
    private int score, life, level;

    private float time, initialTime, startTime;
    private float v, initialV;

    private bool eat, otherTime, start;

    private Grid grid;
    private PacMan pac;
    private Node actual;
    public static GameController gc;

    public GameObject fruit;
    public GameObject startPanel, endPanel, WinPanel;
    private GameObject fruitInstantiated;

    public Text scoreText, lifeText, levelText;

    public AudioClip initialSong, win, lose, move, specialCandy;
    private AudioSource audioSource;

    void Awake()
    {
        if (gc == null)
        {
            gc = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (gc != this)
        {
            Destroy(gameObject);
        }
        audioSource = GetComponent<AudioSource>();
        level = 1;
        life = 3;
        start = false;
        otherTime = false;
        v = 0.1f;
        initialV = v;
        score = 0;
        time = 10;
        initialTime = time;
        normal = 100;
        startTime = initialSong.length;
        initialNormal = normal;
        special = 5000;
        initialSpecial = special;
        Invoke("Fruit", 30);
        scoreText.text = score.ToString();
        lifeText.text = life.ToString();
        levelText.text = level.ToString();
        startPanel.SetActive(true);
        NewLevel();
    }

    private void OnLevelWasLoaded(int level)
    {
        NewLevel();
        v *= 1.05f;
        normal *= 2;
        special *= 2;
        time *= 0.95f;
    }

    void Update()
    {

        if (grid.extras.transform.childCount <= 0 && start)
        {
            start = false;
            WinPanel.SetActive(true);
            audioSource.clip = win;
            audioSource.Play();
            Invoke("ChangeLevel", 2);
        }
    }

    private void NewLevel()
    {
        audioSource.clip = initialSong;
        audioSource.Play();
        Invoke("StartLevel", startTime);
        grid = GameObject.Find("Grid").GetComponent<Grid>();
    }
    public void ChangeLevel()
    {
        WinPanel.SetActive(false);
        startPanel.SetActive(true);
        level++;
        levelText.text = level.ToString();
        life++;
        SceneManager.LoadScene("Level");
    }
    public void StartLevel()
    {
        startPanel.SetActive(false);
        start = true;
    }
    public void RestartLevel()
    {
        endPanel.SetActive(false);
        SceneManager.LoadScene("Level");
    }

    public void EatingTime()
    {
        if (!eat)
            eat = true;
        else
        {
            otherTime = true;
        }
        Invoke("ChangeEat", 10);
    }
    public void ChangeEat()
    {
        if (!otherTime)
            eat = false;
        else
            otherTime = false;
    }

    public void AddNormalPoint(int x, int y, int ghost)
    {
        //IF IS A GHOST-- x,y and ghost are 0
        if (!audioSource.isPlaying)
        {
            audioSource.clip = move;
            audioSource.Play();
        }
        score += normal;
        scoreText.text = score.ToString();
        if (ghost != 0)
            grid.AddNotOccupied(x, y);
    }
    public void AddSpecialPoint(int x, int y)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = move;
            audioSource.Play();
        }
        score += special;
        scoreText.text = score.ToString();
        grid.AddNotOccupied(x, y);
    }

    public void Fruit()
    {
        int alea = UnityEngine.Random.Range(0, grid.NotOccupied.Count);
        actual = grid.NotOccupied[alea];
        grid.NotOccupied.RemoveAt(alea);
        actual.Occupied = true;
        fruitInstantiated = Instantiate(fruit, actual.Coord, Quaternion.Euler(90, 0, 0));
        fruitInstantiated.transform.parent = grid.extras.transform;
        if (!audioSource.isPlaying)
        {
            audioSource.clip = specialCandy;
            audioSource.Play();
        }
        Invoke("DestroyFruit", time);
    }
    public void DestroyFruit()
    {
        Destroy(fruitInstantiated);
        grid.AddNotOccupied(actual.Pos_x, actual.Pos_y);
        Invoke("Fruit", 30);
    }

    public void LoseLife()
    {
        life--;

        if (life < 0)
        {
            score = 0;
            life = 3;
            v = initialV;
            v /= 1.05f;
            normal = initialNormal;
            normal /= 2;
            special = initialSpecial;
            special /= 2;
            time = initialTime;
            time /= 0.95f;
            level = 1;
            start = false;
            eat = false;
            otherTime = false;
            scoreText.text = score.ToString();
            lifeText.text = life.ToString();
            levelText.text = level.ToString();
            endPanel.SetActive(true);
            audioSource.clip = lose;
            audioSource.Play();
            Invoke("RestartLevel", 2);
        }
        else
        {
            lifeText.text = life.ToString();
            ResetPositions();
            eat = false;
            otherTime = false;
            start = false;
            startPanel.SetActive(true);
            audioSource.clip = initialSong;
            audioSource.Play();
            Invoke("StartLevel", startTime);
        }
    }
    private void ResetPositions()
    {
        GameObject go = GameObject.Find("Ghosts");
        GameObject[] ghosts = new GameObject[go.transform.childCount];
        for (int i = 0; i < ghosts.Length; i++)

            Destroy(go.transform.GetChild(i).gameObject);
        if (pac == null)
            pac = GameObject.Find("Homer").GetComponent<PacMan>();
        pac.ResetPosition();

        grid.CreateGhosts();
    }

    //Gets and setters
    public int Score
    {
        get
        {
            return score;
        }

        set
        {
            score = value;
        }
    }
    public bool Eat
    {
        get
        {
            return eat;
        }
    }
    public bool Start1
    {
        get
        {
            return start;
        }

        set
        {
            start = value;
        }
    }
    public bool OtherTime
    {
        get
        {
            return otherTime;
        }
    }
    public float V
    {
        get
        {
            return v;
        }
    }
}
