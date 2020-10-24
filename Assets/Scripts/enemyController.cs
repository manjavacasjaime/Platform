using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{

    Animator enemyAnimator;
    Rigidbody2D rigidBody;
    public despawnController myDespawn;
    public playerController myPlayer;
    public float speed = 7f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        myDespawn = FindObjectOfType<despawnController>();
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        if (transform.position.x > myDespawn.transform.position.x) //left
        {
            if ((transform.position.x < myPlayer.transform.position.x + 5f) && (transform.position.x > myPlayer.transform.position.x - 5f))
            {
                enemyAnimator.Play("enemyAgressive");
            } else
            {
                enemyAnimator.Play("enemyMove");
            }
            rigidBody.velocity = new Vector2(-0.7f * speed, rigidBody.velocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Despawn")
        {
            float myxPos = 0f;
            float myyPos = 0f;
            int rdmx = Random.Range(0, 3);
            int rdmy = Random.Range(0, 3);
            switch (rdmx)
            {
                case 0:
                    myxPos = 332.19f;
                    break;
                case 1:
                    myxPos = 338.05f;
                    break;
                case 2:
                    myxPos = 343.91f;
                    break;
            }

            switch (rdmy)
            {
                case 0:
                    myyPos = -1.5f;
                    break;
                case 1:
                    myyPos = 1.63f;
                    break;
                case 2:
                    myyPos = 4.76f;
                    break;
            }

            transform.position = new Vector3(myxPos, myyPos, 0f);
        }
    }
}
