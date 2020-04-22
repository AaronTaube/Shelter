using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove : MonoBehaviour
{
    [SerializeField] float cookTime = 5f;

    float timeCooking = 0;
    bool isCooking = false;

    //Cached components
    Table table;
    Animator myAnimator;

    // Start is called before the first frame update
    void Start()
    {
        table = GameObject.Find("Table").GetComponent<Table>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCooking)
        {
            timeCooking += Time.deltaTime;
        }
        if(timeCooking >= cookTime)
        {
            FinishNoodles();
        }
        UpdateAnimator();
    }
    void UpdateAnimator()
    {
        myAnimator.SetBool("Cooking", isCooking);
    }

    public void CookNoodles()
    {
        isCooking = true;
    }
    void FinishNoodles()
    {
        isCooking = false;
        table.SpawnCookedNoodles();
        timeCooking = 0f;
    }
}
