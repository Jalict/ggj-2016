using UnityEngine;
using System.Collections;

public class Tentacle : MonoBehaviour {
	Player owner;
	public float width;
	public float height;
	public float damage;
	public float attackSpeed;
	private float timeStamp;
    public Player sender;

	// Use this for initialization
	void Start () {
		timeStamp = attackSpeed + Time.time;
		gameObject.transform.localScale = new Vector2(width, height);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col){
		if(col.gameObject.CompareTag("Player") && col.gameObject != owner && timeStamp < Time.time){
			Player player = col.gameObject.GetComponent<Player>();
			player.OnHit(damage);
			timeStamp += attackSpeed;

	        if(player.OnHit(damage)){
	            sender.KilledPlayer(player);
	        }
		}
		

        if(col.gameObject.tag == "Prisoner"){
            if (col.gameObject.GetComponent<Prisoner>().OnHit(damage))
            {
                sender.KilledPrisoner(col.gameObject.GetComponent<Prisoner>());
            }
        }
	}
}
