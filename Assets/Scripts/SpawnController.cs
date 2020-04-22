using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [SerializeField] int maxZombies = 5;

    int zombieCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddZombie()
    {
        zombieCount++;
    }
    public void RemoveZombie()
    {
        zombieCount--;
    }
    public int GetCount()
    {
        return zombieCount;
    }
    public int GetMax()
    {
        return maxZombies;
    }
}
