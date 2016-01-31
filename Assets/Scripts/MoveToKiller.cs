using UnityEngine;
using System.Collections;

public class MoveToKiller : MonoBehaviour {
    private float _t;
    public float _spawnTime;
    private float _distance;


    public Player Killer;
    public Player Prey;

    public Vector3 killerPosition;
    private Vector3 killedPosition;
    // Use this for initialization
    void Start () {
        _t = 0;
        _spawnTime = 5;
        killedPosition = transform.position;
        _distance = 1 / Vector3.Distance(killedPosition, killerPosition);
    }
	
	// Update is called once per frame
	void Update () {
        if (_t < 0.98f)
        {
            transform.position = Vector3.Lerp(new Vector3(0,0,0), killerPosition, _t);

            _t += Time.deltaTime / (_spawnTime / 1.6f); //NO IDEA LOL
        }
        else {
            DestroyImmediate(gameObject);
        }
    }
    public void InstantiateParticles(Player Killer, Player Prey)
    {
        killedPosition = Prey.transform.position;
        killerPosition = Killer.transform.position;

    }
}
