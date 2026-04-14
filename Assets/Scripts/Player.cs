using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float jumpForce = 4;
    public float moveForce = 5;

    [Header("Sound Effects")] 
    public AudioClip jumpSound;
    public AudioClip walkSound;
    public AudioClip pointsSound;

    [Header("Gems")]
    public TextMeshProUGUI gemsCountText;

    [Header("Level Complete")]
    public GameObject levelComplete;
    public TextMeshProUGUI gemsCollectedText;
    public Button restartButton;

    private Rigidbody2D rb;
    bool canJump = true;
    private AudioSource audioSource;
    float walking = 0;
    int score = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Debug.Log(transform.position.y);
        walking -= Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canJump = false;
            audioSource.PlayOneShot(jumpSound);
        }

        if(Input.GetKey(KeyCode.A))
        {
            rb.AddForce(Vector2.left * moveForce);
            if(canJump && walking <= 0) 
            {
                audioSource.PlayOneShot(walkSound);
                walking = 1;
            }
        } 

        if(Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Vector2.right * moveForce);
            if(canJump && walking <= 0) 
            {
                audioSource.PlayOneShot(walkSound);
                walking = 1;
            }        
        }

        if(transform.position.y < -5)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {  
        if(other.gameObject.CompareTag("Block"))
        {
            canJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Block"))
        {
            canJump = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gem"))
        {
            score++;
            audioSource.PlayOneShot(pointsSound);
            Destroy(other.gameObject);
            gemsCountText.text = "GEMS: " + score;
        }

        if(other.CompareTag("Flag"))
        {
            LevelComplete();
        }
    }

    void LevelComplete()
    {
        levelComplete.SetActive(true);
        gemsCollectedText.text = "Gems Collected: " + score;
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}