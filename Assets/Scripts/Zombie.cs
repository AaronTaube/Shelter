using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    
    //Which way the zombie travels on spawn
    Vector2 initialDirection;
    //Which way the zombie travels upon entering shed
    Vector2 chaseDirection;
    //Direction zombie is currently facing
    Vector2 direction;
    //Object zombie is touching
    GameObject target;

    //Bool Traits
    bool isOutside = true;
    bool facingUp = false;
    bool facingDown = true;
    bool facingLeft = false;
    bool facingRight = false;
    bool isAttacking = false;
    bool isDead = false;

    //Config
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] float attackSpeed = 1f;
    [SerializeField] int health = 3;
    [SerializeField] int strength = 1;
    [SerializeField] AudioClip hurtSound;
    [SerializeField] AudioClip attackSound;

    //Cached components
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    GameObject player;
    BoxCollider2D myCollider;
    SpawnController controller;
    // Start is called before the first frame update
    void Start()
    {

        //Cache components
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<BoxCollider2D>();
        player = GameObject.Find("Player");
        controller = GameObject.Find("SpawnController").GetComponent<SpawnController>();
        //Determine direction of shed
        RaycastHit2D check = Physics2D.Raycast(transform.position, Vector2.down, 20f, 1<<8);
        if(check.collider != null)
        {
            initialDirection = Vector2.down;
        }
        check = Physics2D.Raycast(transform.position, Vector2.right, 20f, 1 << 8);
        if (check.collider != null)
        {
            initialDirection = Vector2.right;
        }
        check = Physics2D.Raycast(transform.position, Vector2.left, 20f, 1 << 8);
        if (check.collider != null)
        {
            initialDirection = Vector2.left;
        }
        check = Physics2D.Raycast(transform.position, Vector2.up, 20f, 1 << 8);
        if (check.collider != null)
        {
            initialDirection = Vector2.up;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckLife();
        Movement();
        SetFacing();
        UpdateAnimator();

    }
    void UpdateAnimator()
    {
        myAnimator.SetBool("Attacking", isAttacking);
        myAnimator.SetBool("Dead", isDead);
        myAnimator.SetBool("Left", facingLeft);
        myAnimator.SetBool("Right", facingRight);
        myAnimator.SetBool("Up", facingUp);
        myAnimator.SetBool("Down", facingDown);
    }
    void Movement()
    {
        if (isDead)
        {
            myRigidBody.velocity = new Vector2(0, 0);
            return;
        }
        if (isAttacking)
        {
            myRigidBody.velocity = new Vector2(0, 0);//if detection poor, remove
            return;
        }
            
        if (isOutside)
        {
            Move(initialDirection);
            direction = initialDirection;
        }
            
        else
        {
            chaseDirection = ChooseDirection();
            Move(chaseDirection);
            direction = chaseDirection;
        }
            
    }
    private Vector2 ChooseDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 myPosition = transform.position;
        float difX = playerPosition.x - myPosition.x;
        float difY = playerPosition.y - myPosition.y;

        if(Mathf.Abs(difX) > Mathf.Abs(difY))
        {
            if (difX > 0)
                return Vector2.right;
            else
                return Vector2.left;
        }
        else
        {
            if (difY > 0)
                return Vector2.up;
            else
                return Vector2.down;
        }
    }
    private void Move(Vector2 direction)
    {
        Vector2 myVelocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);
        myRigidBody.velocity = myVelocity;
    }
    void SetFacing()
    {
        if(direction.x == 0)
        {
            if (direction.y == 1)
            {
                facingUp = true;
                facingDown = false;
                facingLeft = false;
                facingRight = false;
                return;
            }
                
        }
        else
        {
            if (direction.x == -1)
            {
                facingUp = false;
                facingDown = false;
                facingLeft = true;
                facingRight = false;
                return;
            }
         
            else if(direction.x == 1)
            {
                facingUp = false;
                facingDown = false;
                facingLeft = false;
                facingRight = true;
                return;
            }
        }
        facingUp = false;
        facingDown = true;
        facingLeft = false;
        facingRight = false;
        return;
    }
    
    void CheckLife()
    {
        if (health <= 0)
            isDead = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isAttacking = true;
        target = collision.gameObject;
        direction = AttackDirection(collision.transform.position);
        AudioSource.PlayClipAtPoint(attackSound, Camera.main.transform.position, .2f);

    }
    private Vector2 AttackDirection(Vector2 targetPosition)
    {
        Vector2 myPosition = transform.position;
        float difX = targetPosition.x - myPosition.x;
        float difY = targetPosition.y - myPosition.y;

        if (Mathf.Abs(difX) > Mathf.Abs(difY))
        {
            if (difX > 0)
                return Vector2.right;
            else
                return Vector2.left;
        }
        else
        {
            if (difY > 0)
                return Vector2.up;
            else
                return Vector2.down;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //isAttacking = false;
        target = null;
    }
    void Attack()
    {
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Structure")))
        {
            if (!target) return;
            WallBehavior targetScript = target.GetComponent<WallBehavior>();
            if (targetScript != null)
            {
                targetScript.TakeDamage(strength);
            }
        }
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            if (!target) return;
            PlayerMovement targetScript = target.GetComponent<PlayerMovement>();
            if(targetScript != null)
            {
                targetScript.TakeDamage(strength);
            }
                
        }
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Enemy")))
        {
            if (!target) return;
            Zombie targetScript = target.GetComponent<Zombie>();
            if (targetScript != null)
                targetScript.TakeDamage(strength);
        }
        if(target == null)
        {
            isAttacking = false;
        }
    }
    
    public void TakeDamage(int damage)
    {
        health = health - damage;
        AudioSource.PlayClipAtPoint(hurtSound, Camera.main.transform.position,.2f);

    }

    void Death()
    {
        controller.RemoveZombie();
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isOutside = false;
    }
}
