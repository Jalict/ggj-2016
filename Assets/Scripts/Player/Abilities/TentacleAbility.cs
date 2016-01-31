using UnityEngine;
using System.Collections;

public class TentacleAbility : IAbility {

    public GameObject tentaclePrefab;

    public float cooldown = 1;
    public float duration = 0.8f;
    private float lastCast = 0;

    public float damage = 1;
    public float attackSpeed;
    public float width;
    public float height;

    public float projectileSpeed = 1;
    

	public override bool Cast(){
        if (lastCast + cooldown <= Time.time)
        {
            GameObject g = Instantiate(tentaclePrefab, owner.transform.position + (Mathf.Sign(owner.transform.localScale.x)*Vector3.right*width/2), Quaternion.identity) as GameObject;
            Tentacle tentacle = g.GetComponent<Tentacle>();
            tentacle.width = width;
            tentacle.height = height;
            tentacle.damage = damage;
            tentacle.attackSpeed = attackSpeed;
            g.transform.parent = owner.transform;

            lastCast = Time.time;

            Object.Destroy(g, duration);
       		
            return true;
        }
        return false;
	}
    
    public override bool CanCast(){
        return lastCast + cooldown <= Time.time;
    }
}
