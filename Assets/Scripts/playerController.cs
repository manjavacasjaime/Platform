using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    //some player values
    public float speed = 7f;
    public float jumpSpeed = 7f;
    public float movement = 0f;
    public float mylocalScaleX = 1f;
    public float mylocalScaleY = 1f;
    public float mylocalScaleZ = 1f;

    private float spawnPosX = -18.48f;
    private float spawnPosY = 10.58f;

    private bool dying = false;
    private int AButtonPressed = 0;
    private int BeginTouched = 0;

    public Joystick joystick;
    private bool allowMove = false;
    public fredController fredCharacter;

    //groundCheckPoint values
    public Transform groundCheckPoint;
    public LayerMask groundLayer;
    private bool isTouchingGround;
    private Vector2 groundCheckPointA;
    private Vector2 groundCheckPointB;

    //attack values
    private bool isAttacking;
    private bool isAttacked;
    private int frameCount;
    private int damage = 0;

    public GameObject attackHitBox;

    Animator playerAnimator;
    Rigidbody2D rigidBody;
    SpriteRenderer spriteRenderer;
    private AndroidJavaObject javaClass;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        //esto lo coge para girar el personaje (y cambiar orderInLayer)
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackHitBox.SetActive(false);
        javaClass = new AndroidJavaObject("hypernova.hithchhiker.myguide.unityconnectionlibrary.UnityConnectionActivity");
        StartCoroutine("JustEnteredCoroutine");
    }

    // Update is called once per frame
    void Update()
    {
        //check ground
        checkGround();

        if (isTouchingGround && isAttacked && frameCount == 3)
        {
            //stop attack displacement
            isAttacked = false;
        }

        if (frameCount < 3)
        {
            frameCount++;
        }

        movementControls();

        //jump
        if (isAttacked)
        {
            playerAnimator.Play("jumpAttackedAnimation");
        } else if (!isTouchingGround && !isAttacking) playerAnimator.Play("playerJumpAnimation");

        //attackControls();
    }

    public void jumpButton()
    {
        if (isTouchingGround && allowMove)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);
        }
        AButtonPressed++;
    }

    void checkGround()
    {
        groundCheckPointA = new Vector2(groundCheckPoint.position.x - mylocalScaleX, groundCheckPoint.position.y);
        groundCheckPointB = new Vector2(groundCheckPoint.position.x + mylocalScaleX, groundCheckPoint.position.y);//+ (mylocalScaleY / 3f)
        isTouchingGround = Physics2D.OverlapArea(groundCheckPointA, groundCheckPointB, groundLayer);
    }

    void movementControls()
    {
        //left-right controller
        //movement = Input.GetAxis("Horizontal"); //// ESTO SE CAMBIA POR EL DE ABAJO
        movement = joystick.Horizontal;

        //movement
        if (movement > 0f && allowMove) //right
        {
            rigidBody.velocity = new Vector2(0.7f * speed, rigidBody.velocity.y);
            //spriteRenderer.flipX = false; //ESTO VALE SI NO VAS A HACER ATAQUES MELEE
            transform.localScale = new Vector3(mylocalScaleX, mylocalScaleY, mylocalScaleZ);
            if (isTouchingGround && !isAttacking) playerAnimator.Play("playerRunAnimation");
        }
        else if (movement < 0f && allowMove) //left
        {
            rigidBody.velocity = new Vector2(-0.7f * speed, rigidBody.velocity.y);
            //spriteRenderer.flipX = true; //ESTO VALE SI NO VAS A HACER ATAQUES MELEE
            transform.localScale = new Vector3(-mylocalScaleX, mylocalScaleY, mylocalScaleZ);
            if (isTouchingGround && !isAttacking) playerAnimator.Play("playerRunAnimation");
        }
        else if (!isAttacked) //idle
        {
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            if (isTouchingGround && !isAttacking) playerAnimator.Play("playerIdleAnimation");
        }

        if (Input.GetButtonDown("Jump") && isTouchingGround && allowMove)                    //// ESTO
        {                                                                       //// SE
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpSpeed);  //// QUITA
        }                                                                       ////
    }

    void attackControls()
    {
        //attack
        /*if (Input.GetKeyDown(KeyCode.E) && !isAttacking)
        {
            isAttacking = true;

            //choose random btw 1 and 5
            int index = UnityEngine.Random.Range(1, 6);
            playerAnimator.Play("playerAnimationAttack" + index);
            attackHitBox.SetActive(true); // ATACAAA!

            Invoke("ResetAttack", 0.2f); // o en vez de esto, podrías hacer una corutina cuya primer línea sea el yield
        }*/
    }

    /*void ResetAttack()
    {
        attackHitBox.SetActive(false); // para de atacar
        isAttacking = false;
    }*/

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            if (damage < 2)
            {
                //attack displacement
                rigidBody.velocity = new Vector2(-0.7f * speed, jumpSpeed);
                isAttacked = true;
                damage++;
                frameCount = 0;
            } else
            {
                damage = 0;
                //esconde player
                spriteRenderer.sortingOrder = 0;
                //despues de unos segundos, lo devuelve al principio
                dying = true;
                StartCoroutine("RespawnCoroutine");
            }
        } else if (other.tag == "LevelManager" && !dying)
        {
            damage = 0;
            spriteRenderer.sortingOrder = 0;
            StartCoroutine("RespawnCoroutine");
        }
        else if (other.tag == "End")
        {
            allowMove = false;
            StartCoroutine("EndCoroutine");
        }
        else if (other.tag == "Checkpoint")
        {
            spawnPosX = 178.17f;
            spawnPosY = 6.02f;
        }
        else if (other.tag == "Begin")
        {
            BeginTouched++;
            if (BeginTouched == 4 && AButtonPressed == 10) transform.position = new Vector3(361.88f, -1.53f, 0f);
        }
    }

    public IEnumerator EndCoroutine()
    {
        yield return new WaitForSeconds(3.75f);
        javaClass.Call("setPlatformFinishedTrue");
        Application.Quit();
    }

    public IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(2f);
        transform.position = new Vector3(spawnPosX, spawnPosY, 0f);
        spriteRenderer.sortingOrder = 3;
        dying = false;
        damage = 0;
    }

    public IEnumerator JustEnteredCoroutine()
    {
        yield return new WaitForSeconds(3.25f);
        allowMove = true;
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MobilePlatform")
        {
            //speed = speed * 2;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "MobilePlatform")
        {
            //speed = speed * 2;
        }
    }*/
}
