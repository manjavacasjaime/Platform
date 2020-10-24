using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fredController : MonoBehaviour
{
    //some player values
    public float speed = 7f;
    public float mylocalScaleX = 1f;
    public float mylocalScaleY = 1f;
    public float mylocalScaleZ = 1f;

    //groundCheckPoint values
    public Transform groundCheckPoint;
    public LayerMask groundLayer;
    private bool isTouchingGround;
    private Vector2 groundCheckPointA;
    private Vector2 groundCheckPointB;

    public fredNameController fredName;
    public fredNameController fredNameJump;
    public fredNameController nameReverse;
    private bool fredRunning = false;
    private bool allowMove = false;
    private bool despawnHitted = false;

    Animator fredAnimator;
    Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        fredAnimator = GetComponent<Animator>();
        transform.position = new Vector3(-28.57f, 9.26f, 0f);
        StartCoroutine("JustEnteredCoroutine");
    }

    // Update is called once per frame
    void Update()
    {
        //check ground
        checkGround();

        if (!isTouchingGround)
        {
            fredAnimator.Play("fredJumpAnimation");
            fredName.gameObject.SetActive(false);
            fredNameJump.gameObject.SetActive(true);
        } else if (fredRunning && allowMove)
        {
            rigidBody.velocity = new Vector2(0.7f * speed, rigidBody.velocity.y);
            fredAnimator.Play("fredRunAnimation");
            fredNameJump.gameObject.SetActive(false);
            fredName.gameObject.SetActive(true);
        } else
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            fredAnimator.Play("fredIdleAnimation");
            fredNameJump.gameObject.SetActive(false);
            if (!despawnHitted)
            {
                fredName.gameObject.SetActive(true);
            } else
            {
                nameReverse.gameObject.SetActive(true);
            }
        }
    }

    void checkGround()
    {
        groundCheckPointA = new Vector2(groundCheckPoint.position.x - mylocalScaleX, groundCheckPoint.position.y);
        groundCheckPointB = new Vector2(groundCheckPoint.position.x + mylocalScaleX, groundCheckPoint.position.y);//+ (mylocalScaleY / 3f)
        isTouchingGround = Physics2D.OverlapArea(groundCheckPointA, groundCheckPointB, groundLayer);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "FredRun")
        {
            fredRunning = true;
        }
        else if (other.tag == "FredDespawn")
        {
            allowMove = false;
            despawnHitted = true;
            transform.position = new Vector3(368.02f, -1.69f, 0f);
            transform.localScale = new Vector3(-mylocalScaleX, mylocalScaleY, mylocalScaleZ);
        }
    }

    public IEnumerator JustEnteredCoroutine()
    {
        yield return new WaitForSeconds(1.25f);
        allowMove = true;
    }
}
