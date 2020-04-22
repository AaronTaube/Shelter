using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    bool spawn = false;
    //Config
    [SerializeField] float minSpawnDelay = 1f;
    [SerializeField] float maxSpawnDelay = 5f;
    [SerializeField] Zombie zombiePrefab;
    //Cached
    Zombie spawnedUnit;
    SpawnController controller;
    int maxZombies;
    // Start is called before the first frame update
    private void Start()
    {
        controller = GameObject.Find("SpawnController").GetComponent<SpawnController>();
        maxZombies = controller.GetMax();
    }
    IEnumerator Spawning()
    {
        while (spawn)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            SpawnZombie(zombiePrefab);
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
        if (!spawn && !spawnedUnit)
        {
            spawn = true;
            StartCoroutine(Spawning());
        }
        

    }

    private void SpawnZombie(Zombie myZombie)
    {
        int count = controller.GetCount();
        if (count >= maxZombies)
            return;
        spawnedUnit = Instantiate(myZombie, transform.position, Quaternion.identity) as Zombie;
        controller.AddZombie();
        spawn = false;
    }
}
