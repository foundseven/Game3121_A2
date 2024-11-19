using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Mathematics;

public class RotateCameraX : MonoBehaviour
{
    private float speed = 200;
    public GameObject player;
    public PlayerInputActions PlayerInputActions;
    public InputAction Look;

    void Awake()
    {
        PlayerInputActions = new PlayerInputActions();
        Look = PlayerInputActions.Player.Move;
    }
    void OnEnable()
    {
        PlayerInputActions.Enable();
    }
    void OnDisable()
    {
        PlayerInputActions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        // float horizontalInput = Input.GetAxis("Horizontal");
        // transform.Rotate(new float3(0, 1, 0), horizontalInput * speed * Time.deltaTime);
        transform.Rotate(new float3(0, 1, 0), Look.ReadValue<Vector2>().x * speed * Time.deltaTime);
        

        transform.position = player.transform.position; // Move focal point with player

    }
}
