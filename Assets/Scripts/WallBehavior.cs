using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallBehavior : MonoBehaviour
{
    //Config
    [SerializeField] int health = 5;
    [SerializeField] Sprite[] hitSprites;


    //Cached
    Collider2D myCollider;
    SpriteRenderer mySpriteRenderer;

    int damage = 0;
    double healthRatio = 1;
    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    void Update()
    {
        ProcessSprite();
    }
    public void TakeDamage(int strength)
    {
        damage = damage + strength;
       
        ProcessSprite();
            
    }

    void ProcessSprite()
    {
        healthRatio = (double) damage / (double) health;
        if (healthRatio <= .33)
            mySpriteRenderer.sprite = hitSprites[0];
        if (healthRatio >= .33)
            mySpriteRenderer.sprite = hitSprites[1];
        if(damage >= health)
        {
            damage = health;
            mySpriteRenderer.sprite = hitSprites[2];
        }
        ProcessWallState();
    }

    public void GetRepaired(int strength)
    {
        if (damage > 0)
            damage = damage - strength;
        if (damage < 0)
            damage = 0;
        ProcessSprite();
        ProcessWallState();
    }

    void ProcessWallState()
    {
        if (damage >= health)
            BreakDown();
        if (damage < health)
            Restore();
    }

    void BreakDown()
    {
        /*if (TryGetComponent<BoxCollider2D>(out BoxCollider2D myBoxCollider))
            myBoxCollider.enabled = false;
        if (TryGetComponent<PolygonCollider2D>(out PolygonCollider2D myPolygonCollider))
            myPolygonCollider.enabled = false;*/
        gameObject.layer = LayerMask.NameToLayer("Rubble");
    }
    void Restore()
    {
        /*if (TryGetComponent<BoxCollider2D>(out BoxCollider2D myBoxCollider))
            myBoxCollider.enabled = true;
        if (TryGetComponent<PolygonCollider2D>(out PolygonCollider2D myPolygonCollider))
            myPolygonCollider.enabled = true;*/
        gameObject.layer = LayerMask.NameToLayer("Structure");
    }

}
