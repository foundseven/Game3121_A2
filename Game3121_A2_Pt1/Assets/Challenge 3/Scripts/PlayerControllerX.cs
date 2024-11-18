using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;
public class PlayerControllerX : MonoBehaviour
{
    public bool gameOver;

    public float floatForce;
    private float gravityModifier = 1.5f;
    private Rigidbody playerRb;

    public ParticleSystem explosionParticle;
    public ParticleSystem fireworksParticle;

    private AudioSource playerAudio;
    public AudioClip moneySound;
    public AudioClip explodeSound;

    //Q 1. adding new input action refs
    [SerializeField]
    public PlayerInputActions playerInputAction;
    private InputAction playerMovement;

    //Q 6. creating floats for boundary
    public float boundaryYUpper = 14.0f;
    public float boundaryYLower = 1.0f;

    //Q 7. creating float for boundary and SFX
    public float bounceForce;
    public AudioClip bounceSound;

    private void Awake()
    {
        //Q 1. setting them up
        playerInputAction = new PlayerInputActions();
        playerMovement = playerInputAction.Player.FloatUp;
    }
    //Q 1. adding on enable on disable
    private void OnEnable()
    {
        Debug.Log("Input Actions Enabled");
        playerInputAction.Enable();
    }

    private void OnDisable()
    {
        Debug.Log("Input Actions Disabled");
        playerInputAction.Disable();
    }


    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        playerRb = GetComponent<Rigidbody>();

        // Apply a small upward force at the start of the game
        playerRb.AddForce(Vector3.up * 5, ForceMode.Impulse);
    }


    // Update is called once per frame
    void Update()
    {
        Debug.Log("Float Input: " + playerMovement.ReadValue<float>());

        //Q 1. apply the upward force
        if (playerMovement.ReadValue<float>() > 0 && !gameOver)
        {
            // Apply upward force to the balloon
            playerRb.AddForce(Vector3.up * floatForce);
        }
        //Q 6. call in update
        BoundaryCheck();

        //Q 7. bounce check
        if(transform.position.y <= boundaryYLower)
        {
            Bounce();
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // if player collides with bomb, explode and set gameOver to true
        if (other.gameObject.CompareTag("Bomb"))
        {
            explosionParticle.Play();
            playerAudio.PlayOneShot(explodeSound, 1.0f);
            gameOver = true;
            Debug.Log("Game Over!");
            Destroy(other.gameObject);
        } 

        // if player collides with money, fireworks
        else if (other.gameObject.CompareTag("Money"))
        {
            fireworksParticle.Play();
            playerAudio.PlayOneShot(moneySound, 1.0f);
            Destroy(other.gameObject);

        }

    }
    //Q 6. boundary check func
    public void BoundaryCheck()
    {
        //transform.position = new Vector3(
        //    transform.position.x,
        //    Mathf.Clamp(transform.position.y, boundaryYLower, boundaryYUpper),
        //    transform.position.z);

        float3 currentPosition = new float3(transform.position.x, transform.position.y, transform.position.z);
        currentPosition.y = math.clamp(currentPosition.y, boundaryYLower, boundaryYUpper);
        transform.position = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z);
    }

    //Q 7.bounce check func
    public void Bounce()
    {
        //reset vert vel
        playerRb.velocity = new Vector3(playerRb.position.x, 0, playerRb.velocity.z);
        playerRb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        playerAudio.PlayOneShot(bounceSound, 1.0f);
    }

}
