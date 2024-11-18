﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
            playerRb.AddForce(Vector3.up * floatForce);  // Modify floatForce as needed
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

}
