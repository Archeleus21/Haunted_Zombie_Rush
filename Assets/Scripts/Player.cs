using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] AudioClip jumpSFX;  //stores jumpsound
    [SerializeField] AudioClip deathSFX;  //stores death sound
    [SerializeField] int jumpPower;  //jumpPower
    [SerializeField] int knockBackX = -15;  //jumpPower
    [SerializeField] int knockBackY = 20;  //jumpPower
    
    //creates storage for components
    Animator animator;
    Rigidbody playerRB;
    AudioSource audioSource;

    private bool isJumping = false;  //detect when jumping

    //used for assertions/errors
    private void Awake()
    {
        //checks if audio fields are empty
        Assert.IsNotNull(jumpSFX);
        Assert.IsNotNull(deathSFX);
    }

    // Start is called before the first frame update
    void Start()
    {
        //gets components
        animator = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //disable controls
        if (!GameManager.instance.GameOver && GameManager.instance.GameStarted)
        {
            PlayerControls();
        }

        CollectedCoin();
    }


    void FixedUpdate()
    {
        PlayerJumpPhysics();
    }

    private void PlayerControls()
    {
        //button press to jump
        if (Input.GetMouseButtonDown(0))
        {
            animator.Play("Jump"); //runs animation
            GameManager.instance.PlayerStartedGame(); //tells gamemanager that the player started to play
            audioSource.PlayOneShot(jumpSFX); //plays jump sound
            audioSource.volume = 0.5f;  //sets volume to .3f
            playerRB.useGravity = true;  //turns gravity on
            isJumping = true;  //changes true when jumping
        }
    }

    private void PlayerJumpPhysics()
    {
        //runs once each time jump is pressed
        if (isJumping == true)
        {
            isJumping = false;
            playerRB.velocity = new Vector3(0f, 0f, 0f);  //reset velocity to zero preventing the downward velocity from increasing
            playerRB.AddForce(new Vector3(0f, jumpPower, 0f), ForceMode.Impulse);  //aplies upward force
        }
    }

    //checking collision
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Obstacle")
        {            
            playerRB.AddForce(new Vector3(knockBackX, knockBackY), ForceMode.Impulse);//knocks player back on collision
            playerRB.detectCollisions = false;  //disables collision to fall through ground
            audioSource.PlayOneShot(deathSFX);  //plays death sound
            audioSource.volume = .5f;  //adj volume
            GameManager.instance.PlayerCollided();  //lets the game manager know the game is over
            Destroy(gameObject, 3f);  //destroys player after a given time in seconds
        }
    }

    private void CollectedCoin()
    {
        if(GameManager.instance.CollectedCoin)
        {
            GameManager.instance.AddPlayerScore();
        }
    }

    void DestroyOldPlayer()
    {
        Destroy(gameObject);
    }
}
