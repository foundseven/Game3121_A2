using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class EnemyX : MonoBehaviour
{
    public float speed;
    private Rigidbody enemyRb;
    private GameObject playerGoal;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        // Q 5. finding the gameobject in the scene for the ball to go towards the goal
        playerGoal = GameObject.Find("Player Goal");
    }

    // Update is called once per frame
    void Update()
    {
        // Set enemy direction towards player goal and move there
        float3 lookDirection = math.normalize((float3)(playerGoal.transform.position - transform.position));
        enemyRb.AddForce((Vector3)(lookDirection * speed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision other)
    {
        // If enemy collides with either goal, destroy it
        if (other.gameObject.name == "Enemy Goal")
        {
            Destroy(gameObject);
        } 
        else if (other.gameObject.name == "Player Goal")
        {
            Destroy(gameObject);
        }
    }
}
