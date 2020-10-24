using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyUpController : MonoBehaviour
{

    Animator enemyAnimator;
    Rigidbody2D rigidBody;
    public despawnUpController myDespawnUp;
    public playerController myPlayer;
    public float speed = 7f;
    private float positionX;
    private float positionY;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        myDespawnUp = FindObjectOfType<despawnUpController>();
        positionX = transform.position.x;
        positionY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        if (transform.position.y < myDespawnUp.transform.position.y) //go up
        {
            if ((transform.position.x < myPlayer.transform.position.x + 5f) && (transform.position.x > myPlayer.transform.position.x - 5f))
            {
                enemyAnimator.Play("enemyAgressive");
            }
            else
            {
                enemyAnimator.Play("enemyMove");
            }
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0.7f * speed);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "DespawnUp")
        {
            transform.position = new Vector3(positionX, positionY, 0f);
        }
    }
}
