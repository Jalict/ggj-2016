using UnityEngine;
using System.Collections;

public class PrisonerSpawner : MonoBehaviour {

    public GameObject[] prisonerPrefabs;

    public int maxPrisoners = 5;
    public int initialPrisoners = 0;

    public float spawnInterval = 1;
    public int numSpawnedPerSpawn = 1;
    private float nextSpawn;
    private int numPrisoners;

    public float chainLength = 5;
    

    // Use this for initialization
    void Start () {
	   for (int i = 0; i < initialPrisoners; i++)
       {
            SpawnPrisoner();
        }
	}
	
	// Update is called once per frame
	void Update () {
	   if(numPrisoners < maxPrisoners && nextSpawn < Time.time){
    	   for (int i = 0; i < numSpawnedPerSpawn; i++)
                SpawnPrisoner();
        }
	}
    
    public void PrisonerDied(){
        numPrisoners--;
        nextSpawn = Time.time + spawnInterval;

    }
    
    public void SpawnPrisoner(){
        numPrisoners++;
        GameObject g = Instantiate(prisonerPrefabs[Random.Range(0, prisonerPrefabs.Length)], transform.position + Vector3.right * Random.Range(-chainLength, chainLength) + Vector3.up * Random.Range(0f,2f), Quaternion.identity) as GameObject;

        g.transform.SetParent(transform);

        Prisoner p = g.GetComponent<Prisoner>();

        p.chainEnd    = gameObject;
        p.spawner     = this;
        p.chainLength = chainLength;

        p.confusion         = Random.Range(.2f, .8f);
        p.tired             = Random.Range(.2f, .8f);
        p.restless          = Random.Range(.2f, .8f);
        p.decisionFrequency = Random.Range(2, 5);

        nextSpawn = Time.time + spawnInterval;
    }
}
