using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioPlayer;

    public GameObject game;
    public GameObject enemyGenerator;
    public AudioClip jumpClip;
    public AudioClip dieClip;
    public AudioClip pointClip;
    public ParticleSystem dust;

    private float startY;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        startY = transform.position.y;
        DustStop();
    }

    // Update is called once per frame
    void UpdateState(string state = null)
    {
        if (state != null) {
            animator.Play(state);
        }
    }

    void Update()
    {
        bool gamePlaying = game.GetComponent<GameController>().gameState == GameState.Playing;
        bool userAction = (Input.GetKeyDown("up") || Input.GetMouseButtonDown(0));
        bool isGrounded = transform.position.y == startY;

        if (gamePlaying && userAction && isGrounded) {
            UpdateState("PlayerJump");
            audioPlayer.clip = jumpClip;
            audioPlayer.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            UpdateState("PlayerDie");
            game.GetComponent<GameController>().gameState = GameState.Ended;
            enemyGenerator.SendMessage("CancelGenerator", true);
            game.SendMessage("ResetTimeScale", 1f);
            DustStop();
            game.GetComponent<AudioSource>().Stop();
            audioPlayer.clip = dieClip;
            audioPlayer.Play();
            game.GetComponent<GameController>().gameState = GameState.Ready;
            
        }
        else if (other.tag == "PointCollider") {
            game.SendMessage("IncreasePoints");
            audioPlayer.clip = pointClip;
            audioPlayer.Play();
        }
    }

    void GameReady()
    {
        game.GetComponent<GameController>().gameState = GameState.Ready;
    }

    public void DustPlay()
    {
        dust.Play();
    }

    public void DustStop()
    {
        dust.Stop();
    }
}
