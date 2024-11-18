using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;
    private GameObject focalPoint;

    public bool hasPowerup;
    public GameObject powerupIndicator;
    public PlayerInputActions PlayerInputActions;
    public InputAction Boost;
    private InputAction move;
    public int powerUpDuration = 5;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup

    private float BoostStrength = 10;

    public GameObject BoostEffect;
    public ParticleSystem BoostParticles;
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }
    void Awake()
    {
        PlayerInputActions = new PlayerInputActions();
        
        Boost = PlayerInputActions.Player.Boost;
        move = PlayerInputActions.Player.Move;

        BoostEffect = GameObject.Find("Smoke_Particle");
        if (BoostEffect != null)
        BoostParticles = BoostEffect.GetComponent<ParticleSystem>();
        
    }


    void OnEnable()
    {
        PlayerInputActions.Enable();
    }

    void BoostPowerup()
    {

        playerRb.AddForce(focalPoint.transform.forward * BoostStrength, ForceMode.Force);
        
    }



    void OnDisable()
    {
        PlayerInputActions.Disable();
    }

    void Update()
    {
        // Add force to player in direction of the focal point (and camera)
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);

        if (Boost.IsPressed())
        {
           BoostPowerup();
           BoostEffect.transform.position = transform.position;
           BoostParticles.Play();
        }

    }
    void FixedUpdate()
    {
        float2 moveInput = move.ReadValue<Vector2>();

        
        float3 forward = new float3(focalPoint.transform.forward.x, 0, focalPoint.transform.forward.z);
        float3 forceDirection = forward * moveInput.y * speed * Time.fixedDeltaTime;

        playerRb.AddForce((Vector3)forceDirection, ForceMode.Force);
    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            float3 awayFromPlayer = other.gameObject.transform.position - transform.position;

            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }
        }
    }



}
