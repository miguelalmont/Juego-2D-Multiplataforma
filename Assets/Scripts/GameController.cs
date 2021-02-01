using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState {Idle, Playing, Ended, Ready};

public class GameController : MonoBehaviour
{

    [Range(0f, 0.20f)]
    public float parallaxSpeed = 0.02f;
    public float scaleTime = 6f;
    public float scaleInc = .25f;

    private int points = 0;

    public RawImage background;
    public RawImage platform;
    public GameState gameState = GameState.Idle;
    public GameObject uiIdle;
    public GameObject uiScore;
    public GameObject uiGameOver;
    public GameObject player;
    public GameObject enemyGenerator;
    public Text pointsText;
    public Text recordText;
    public Text endPointsText;
    public Text endRecordText;
    private AudioSource musicPlayer;

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
        recordText.text = "BEST: " + GetMaxScore().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        bool userAction = (Input.GetKeyDown("up") || Input.GetMouseButtonDown(0));

        // Empieza el juego
        if(gameState == GameState.Idle && userAction) {
            gameState = GameState.Playing;
            uiIdle.SetActive(false);
            uiScore.SetActive(true);
            player.SendMessage("UpdateState", "PlayerRun");
            player.SendMessage("DustPlay");
            enemyGenerator.SendMessage("StartGenerator");
            musicPlayer.Play();
            InvokeRepeating("GameTimeScale", scaleTime, scaleTime);
            
        }

        // Si el juego esta en marcha
        else if(gameState == GameState.Playing){
            Parallax();
        }
        // Si el juego esta preparado para reiniciar
        else if (gameState == GameState.Ready) {
            uiScore.SetActive(false);
            SetEndPoints();
            uiGameOver.SetActive(true);
            if (userAction) {
                ResetTimeScale(1f);
                RestartGame();
            }
        }

    }

    void Parallax()
    {
        // Efecto parallax
        float finalSpeed = parallaxSpeed * Time.deltaTime;
        background.uvRect = new Rect(background.uvRect.x + finalSpeed, 0f, 1f, 1f);
        platform.uvRect = new Rect(platform.uvRect.x + finalSpeed * 4, 0f, 1f, 1f);

    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Main");
    }

    void GameTimeScale()
    {
        Time.timeScale = Time.timeScale + scaleInc;
        //Debug.Log("Ritmo Incrementando: " + Time.timeScale.ToString());
    }

    public void ResetTimeScale(float newtimeScale = 1f)
    {
        CancelInvoke("GameTimeScale");
        Time.timeScale = newtimeScale;
        //Debug.Log("Ritmo Reestablecido: " + Time.timeScale.ToString());
    }

    public void IncreasePoints()
    {
        pointsText.text = (++points).ToString();
        if (points >= GetMaxScore()) {
            recordText.text = "BEST: " + points.ToString();
            SaveScore(points);
        }
    }

    public int GetMaxScore()
    {
        return PlayerPrefs.GetInt("Max Points", 0);
    }

    public void SaveScore(int currentPoints)
    {
        PlayerPrefs.SetInt("Max Points", currentPoints);
    }

    public void SetEndPoints()
    {
        endPointsText.text = "YOUR SCORE: " + points.ToString();
        endRecordText.text = "BEST SCORE: " + GetMaxScore();
    }
}
