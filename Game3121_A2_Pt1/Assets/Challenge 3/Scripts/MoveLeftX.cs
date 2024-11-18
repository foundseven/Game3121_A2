using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class MoveLeftX : MonoBehaviour
{
    public float speed;
    private PlayerControllerX playerControllerScript;
    private float leftBound = -10;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerControllerX>();
    }

    // Update is called once per frame
    void Update()
    {
        // If game is not over, move to the left
        //Q 2. added ! to make it move
        if (!playerControllerScript.gameOver)
        {
            //transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
            //left direction
            float3 moveDirection = new float3(-1, 0, 0);
            float3 movement = moveDirection * speed * Time.deltaTime;

            //apply the movement - convert back for unity's transform
            transform.Translate(new Vector3(movement.x, movement.y, movement.z), Space.World);
        }

        // If object goes off screen that is NOT the background, destroy it
        if (transform.position.x < leftBound && !gameObject.CompareTag("Background"))
        {
            Destroy(gameObject);
        }

    }
}
