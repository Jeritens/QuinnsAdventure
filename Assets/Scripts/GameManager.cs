using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int score;
    [SerializeField]
    Spawner spawner;

    [SerializeField]
    Player player;
    public List<SpinningTop> tops = new List<SpinningTop>();
    public float points;
    public TextMeshProUGUI ScoreText;
    public GameObject GameOverPanel;
    public TextMeshProUGUI HighScore;
    public bool paused = false;

    static public GameManager instance;
    public bool[] powerUps = new bool[5];
    public int extraLifes = 0;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
        }
        instance = this;
        score = 0;
    }

    public void addSpinningTop(SpinningTop top)
    {
        score++;
        tops.Add(top);
        ScoreText.text = score.ToString();
        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        //spawner.spawnSpinningTop();
    }

    public void stoppedSpinning()
    {
        if (extraLifes > 0)
        {
            extraLifes--;
            SpinAll(1000f);
        }
        else
        {
            paused = true;
            GameOverPanel.SetActive(true);
            HighScore.text = "Score: " + score.ToString() + "\n \nHigh Score: " + PlayerPrefs.GetInt("HighScore");
            //Debug.Log("one stopped spinning \t score: " + score);
        }

    }
    public void SpinAll(float rpm)
    {
        foreach (SpinningTop top in tops)
        {
            top.speedUp(rpm);
        }
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        foreach (SpinningTop top in tops)
        {
            points += top.pointsPerSecond * Time.deltaTime;
        }
    }
    public void AddPoints(float amount)
    {
        points += amount;
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }



}
