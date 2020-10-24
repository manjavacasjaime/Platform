using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDownController : MonoBehaviour
{

    Animator enemyAnimator;
    Rigidbody2D rigidBody;
    public despawnDownController myDespawnDown;
    public playerController myPlayer;
    public float speed = 7f;
    private float positionX;
    private float positionY;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        myDespawnDown = FindObjectOfType<despawnDownController>();
        positionX = transform.position.x;
        positionY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        if (transform.position.y > myDespawnDown.transform.position.y) //go down
        {
            if ((transform.position.x < myPlayer.transform.position.x + 5f) && (transform.position.x > myPlayer.transform.position.x - 5f))
            {
                enemyAnimator.Play("enemyAgressive");
            }
            else
            {
                enemyAnimator.Play("enemyMove");
            }
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, -0.7f * speed);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DespawnDown")
        {
            transform.position = new Vector3(positionX, positionY, 0f);
        }
    }
}
