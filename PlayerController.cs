using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //references the player animator of the same name created in unity
    Animator anim;
    //creates a gameobject to be used in our scroll class
    public static GameObject player;

    //stores which platform the player is on
    public static GameObject currentPlatform;

    //array of all sfx sounds used in game
    public static AudioSource[] sfx;

    //this will limit when the player is able to turn left or right
    bool canTurn = false;

    //used for reference when turnint
    Vector3 startPosition;
    Rigidbody rb;

    //magic variables
    public GameObject magic;
    public Transform magicStartPosition;
    Rigidbody mRb;

    //kick variables
    public GameObject kick;
    public Transform kickStartPosition;
    Rigidbody kRb;

    int livesLeft;
    public Texture aliveIcon;
    public Texture deadIcon;
    public RawImage[] icons;

    public GameObject gameOverPanel;
    public Text highScore;

    bool falling = false;

    public float leftRightSpeed;


    void RestartGame()
    {
        SceneManager.LoadScene("ScrollingWorld", LoadSceneMode.Single);
    }

    //we have to start off not dead
    public static bool isDead = false;

    //tracks which platform through collision
    private void OnCollisionEnter(Collision other)
    {
        //if you hit fire or a wall, or if you fall of the map, you die
        //used the fire tag for falling off the map since it has the isDead outcome
        if ((falling || other.gameObject.tag == "Fire" || other.gameObject.tag == "Wall") && !isDead)
        {
            if (falling)
                anim.SetTrigger("isFalling");
            else
            //falls over dead
            anim.SetTrigger("isDead");
            isDead = true;
            sfx[3].Play();
            //lose a life
            livesLeft--;
            //set the lives left display
            PlayerPrefs.SetInt("lives", livesLeft);

            if (livesLeft > 0)
                //reset
                Invoke("RestartGame", 2);
            else
            {
                icons[0].texture = deadIcon;
                gameOverPanel.SetActive(true);

                //after game over, stores last score
                PlayerPrefs.SetInt("lastscore", PlayerPrefs.GetInt("score"));
                //checks if it's higher than current high score, and if so, replaces it
                if (PlayerPrefs.HasKey("highestscore"))
                {
                    int hs = PlayerPrefs.GetInt("highestscore");
                    if (hs < PlayerPrefs.GetInt("score"))
                        PlayerPrefs.SetInt("highscore", PlayerPrefs.GetInt("score"));
                }
                else
                    //this will run if it's the first time playing and there are no previous scores
                    PlayerPrefs.SetInt("highestscore", PlayerPrefs.GetInt("score"));
            }
        }
        else
            currentPlatform = other.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        mRb = magic.GetComponent<Rigidbody>();
        sfx = GameObject.FindWithTag("gamedata").GetComponentsInChildren<AudioSource>();

        player = this.gameObject;
        startPosition = player.transform.position;
        //creates first platform attached to starting platform
        GenerateWorld.RunDummy();

        if (PlayerPrefs.HasKey("highscore"))
            highScore.text = "High Score: " + PlayerPrefs.GetInt("highscore");
        else
            highScore.text = "High Score: 0";

        isDead = false;
        livesLeft = PlayerPrefs.GetInt("lives");

        //changes lives left icon from aliveIcon to deadIcon
        for(int i = 0; i < icons.Length; i++)
        {
            if (i >= livesLeft)
                icons[i].texture = deadIcon;
        }

    }

    //magic start position is set to the player's right hand in unity
    void CastMagic()
    {
        magic.transform.position = magicStartPosition.position;
        //magic is set to inactive in unity and needs to be activated
        magic.SetActive(true);
        //resets the velocity if it gets kicked at a weird angle by an obstacle
        mRb.velocity = Vector3.zero;
        mRb.AddForce(this.transform.forward * 4000);
        //then magic needs to be deactivated
        Invoke("KillMagic", 1);
    }

    //deactivates CastMagic
    void KillMagic()
    {
        magic.SetActive(false);

    }

    void Kick()
    {
        kick.transform.position = kickStartPosition.position;
        kick.SetActive(true);
        kRb.AddForce(this.transform.forward * 10);
        Invoke("KillKick", .5f);
    }

    void KillKick()
    {
        kick.SetActive(false);
    }

    //each of these are called upon in Unity to play at a specific animation point/event
    void Footstep1()
        {
        sfx[5].Play();
        }

    void Footstep2()
    {
        sfx[6].Play();
    }

    void Magic()
    {
        sfx[2].Play();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other is BoxCollider && GenerateWorld.lastPlatform.tag != "platformTSection")
            GenerateWorld.RunDummy();

        //unlike other triggers, the turn collider is a sphere so it will
        //only be activated when we want it to be
        if (other is SphereCollider)
            canTurn = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other is SphereCollider)
            canTurn = false;
    }
    //stops the jump animation action, must be set in the animator for jump in unity
    void StopJump()
    {
        anim.SetBool("isJumping", false);
    }
    //stops the magic action, must be set in animator for jump in unity
    void StopMagic()
    {
        anim.SetBool("isMagic", false);
    }

    void StopKick()
    {
        anim.SetBool("isKicking", false);
    }
    // Update is called once per frame
    void Update()
    {
        //if you're dead, you can't move, jump, cast spells, etc.
        if (isDead) return;

        if(currentPlatform != null)
        {
            if(this.transform.position.y < (currentPlatform.transform.position.y - 10))
            {
                falling = true;
                OnCollisionEnter(null);
            }
        }

        //begins jump animation when space is pressed and prevents magic casting while jumping
        if (Input.GetKeyDown(KeyCode.Space) && anim.GetBool("isJumping") == false && anim.GetBool("isMagic") == false)
        {
            anim.SetBool("isJumping", true);
            //moves rigid body connected to character when you jump
            rb.AddForce(Vector3.up * 200);
            sfx[7].Play();
        }
        //begins magic animation when M is pressed and prevents jumping during magic casting
        else if (Input.GetKeyDown(KeyCode.M))
        {
            anim.SetBool("isMagic", true);
        }



        //vector3 makes it transform on the spot
        //90 degree turn is clockwise, so character goes right
        //these don't need a stop method because transform only occurs once per key press
        else if (Input.GetKeyDown(KeyCode.RightArrow) && canTurn)
        {
            this.transform.Rotate(Vector3.up * 90);
            //runDummy has to be instantiated differently for turns so that the platforms
            //generate where you're turning instead of in front of you.
            GenerateWorld.dummyTraveller.transform.forward = -this.transform.forward;
            GenerateWorld.RunDummy();

            if (GenerateWorld.lastPlatform.tag != "platformTSection")
                GenerateWorld.RunDummy();

            //resets player to start position on x axis after turn
            this.transform.position = new Vector3(startPosition.x,
                                                    this.transform.position.y,
                                                    startPosition.z);
        }

        //-90 degree turn is counterclockwise, turns left on the spot
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && canTurn)
        {
            this.transform.Rotate(Vector3.up * -90);
            GenerateWorld.dummyTraveller.transform.forward = -this.transform.forward;
            GenerateWorld.RunDummy();

            if (GenerateWorld.lastPlatform.tag != "platformTSection")
                GenerateWorld.RunDummy();

            this.transform.position = new Vector3(startPosition.x,
                                                    this.transform.position.y,
                                                    startPosition.z);
        }

        //since left and right arrows are used for turning, we'll use A and D for moving side to side
        //these also don't need stop methods, as they occur once per key press
        //else if (Input.GetKeyDown(KeyCode.A))
        //{
        //    this.transform.Translate(-0.5f, 0, 0);
        //}

        //else if (Input.GetKeyDown(KeyCode.D))
        //{
        //    this.transform.Translate(0.5f, 0, 0);
        //}

        //smooth movement left and right, hold down key to move, release to stop
        else if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * Time.deltaTime * leftRightSpeed);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.left * Time.deltaTime * leftRightSpeed * -1);
        }
    }
}
