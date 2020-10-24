using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mobileGroundController : MonoBehaviour
{

    Rigidbody2D rigidBody;
    private int mobileValue = 1;
    public float speed = 7f;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //movement
        rigidBody.velocity = new Vector2(mobileValue * 0.7f * speed, rigidBody.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "MobileInversor")
        {
            mobileValue = -mobileValue;
        }
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(null);
        }
    }*/
}
