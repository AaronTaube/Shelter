using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField] Sprite[] tableSprites;

    bool hasNoodles = false;

    //Cached
    SpriteRenderer mySpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SpawnCookedNoodles()
    {
        mySpriteRenderer.sprite = tableSprites[1];
        hasNoodles = true;
    }

    public bool CheckNoodles()
    {
        return hasNoodles;
    }
    public void EatNoodles()
    {
        mySpriteRenderer.sprite = tableSprites[0];
        hasNoodles = false;
    }
}
