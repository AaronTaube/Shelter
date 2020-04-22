using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //Config
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float workSpeed = 1f;
    [SerializeField] int fullHealth = 5;
    [SerializeField] int health = 5;
    [SerializeField] int strength = 1;
    [SerializeField] int healingFactor = 2;
    [SerializeField] float maxHunger = 60;//seconds
    [SerializeField] AudioClip hurtSound;

    //Cached components
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    BoxCollider2D frontCollider;
    GameObject myNoodles;

    //Direction Player is facing
    Vector2 direction;
    //Current interactable target
    GameObject target;

    //Where player's hunger level stands
    float hunger;

    //Bool Traits
    bool facingUp = false;
    bool facingDown = false;
    bool facingLeft = false;
    bool facingRight = false;
    bool isAttacking = false;
    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myNoodles = gameObject.transform.GetChild(0).gameObject;
        hunger = maxHunger;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("click");
            Interact();
        }

        Move();
        IncrementHunger();
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
        myAnimator.SetFloat("Speed", myRigidBody.velocity.magnitude);
    }
    private void Move()
    {
        if (isDead)
        {
            myRigidBody.velocity = new Vector2(0, 0);
            return;
        }
        if (isAttacking)
        {
            myRigidBody.velocity = new Vector2(0, 0);
            return;
        }
        float controlHorizontalThrow = Input.GetAxis("Horizontal");
        float controlVeritcalThrow = Input.GetAxis("Vertical");
        Vector2 playerVelocity = new Vector2(controlHorizontalThrow * moveSpeed, controlVeritcalThrow * moveSpeed);
        myRigidBody.velocity = playerVelocity;
        SetDirection();
        SetFacing();
    }
    void SetDirection()
    {
        Vector2 playerVelocity = myRigidBody.velocity;
        float xVelocity = playerVelocity.x;
        float yVelocity = playerVelocity.y;
        if (Mathf.Abs(xVelocity) > Mathf.Abs(yVelocity))
        {
            if (xVelocity > 0)
                direction = Vector2.right;
            else
                direction = Vector2.left;
        }
        else
        {
            if (yVelocity > 0)
                direction = Vector2.up;
            else if(yVelocity < 0)
                direction = Vector2.down;
        }
    }
    void SetFacing()
    {
        if (direction.x == 0)
        {
            if (direction.y == 1)
            {
                facingUp = true;
                facingDown = false;
                facingLeft = false;
                facingRight = false;
                return;
            }
            else if(direction.y == -1)
            {
                facingUp = false;
                facingDown = true;
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

            else if (direction.x == 1)
            {
                facingUp = false;
                facingDown = false;
                facingLeft = false;
                facingRight = true;
                return;
            }
        }/*
        facingUp = false;
        facingDown = true;
        facingLeft = false;
        facingRight = false;*/
        return;
    }
    
    private void Interact()
    {
        //Check for wall, stove, food, or enemy
        RaycastHit2D checkForEnemies = Physics2D.Raycast(transform.position, direction, 1f, 1 << 11);
        if (checkForEnemies.collider != null)
        {
            target = checkForEnemies.collider.gameObject;
            isAttacking = true;
            Debug.Log("Enemy");
            return;
        }
        int[] interactables = { 1 << 13, 1 << 12, 1 << 10 };
        RaycastHit2D checkForRubble = Physics2D.Raycast(transform.position, direction, 1f, 1 << 13);
        RaycastHit2D checkForStructure = Physics2D.Raycast(transform.position, direction, 1f, 1 << 10);
        RaycastHit2D checkForUtility = Physics2D.Raycast(transform.position, direction, 1f, 1 << 12);
        if(checkForRubble.collider != null)
        {
            target = checkForRubble.collider.gameObject;
            isAttacking = true;
            Debug.Log("Rubble");
            return;
        }
        if (checkForStructure.collider != null)
        {
            target = checkForStructure.collider.gameObject;
            isAttacking = true;
            Debug.Log("Structure");
            return;
        }
        if (checkForUtility.collider != null)
        {
            target = checkForUtility.collider.gameObject;
            UseUtility(target);
            return;
        }
    }
    void Repair(WallBehavior wall)
    {
        //reuses attack animation, though not an actual attack
        wall.GetRepaired(strength * healingFactor);
    }
    void UseUtility(GameObject utility)
    {
        if (utility.GetComponent<Ramen>())
        {
            myNoodles.SetActive(true);
        }
        if (utility.GetComponent<Stove>())
        {
            if (myNoodles.activeInHierarchy)
            {
                Cook(utility.GetComponent<Stove>());
            }
        }
        if (utility.GetComponent<Table>())
        {
            Eat(utility.GetComponent<Table>());
            
        }
    }
    void Cook(Stove stove)
    {
        myNoodles.SetActive(false);
        stove.CookNoodles();
    }
    void Swing()
    {
        if (!target) return;
        if (target.GetComponent<WallBehavior>())
        {
            Repair(target.GetComponent<WallBehavior>());
        }
        if (!target) return;
        if (target.GetComponent<Zombie>())
        {
            Attack(target.GetComponent<Zombie>());
        }
        isAttacking = false;
    }
    void Attack(Zombie zombie)
    {
        zombie.TakeDamage(strength);
    }
    void Eat(Table table)
    {
        if (table.CheckNoodles())
        {
            table.EatNoodles();
            hunger = maxHunger;
            health = fullHealth;
        }
            
    }
    void IncrementHunger()
    {
        hunger -= Time.deltaTime;
        if(hunger <= 0)
        {
            isDead = true;
        }
    }
   
    public void TakeDamage(int damage)
    {
        health = health - damage;
        AudioSource.PlayClipAtPoint(hurtSound, Camera.main.transform.position, .2f);
        if (health <= 0)
            isDead = true;
    }
    public int GetHealth()
    {
        return health;
    }
    public float GetHunger()
    {
        return hunger;
    }
    public float GetMaxHunger()
    {
        return maxHunger;
    }
}
