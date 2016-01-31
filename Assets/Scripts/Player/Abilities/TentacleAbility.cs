using UnityEngine;
using UnityEditor.Animations;
using System.Collections;
using XInputDotNetPure;

public class TentacleAbility : IAbility {

    public float cooldown = 1;
    private float lastCast = 0;

    public float damage = 1;

    public float projectileSpeed = 1;
    
    private PlayerIndex playerIndexRef;
    private Player playerRef;

    public GameObject tentacle;
    public float length;
    public float height;

	public override bool Cast(){
        if (lastCast + cooldown <= Time.time)
        {
            playerRef = GetComponent<Player>();
            playerIndexRef = playerRef.playerIndex;
            lastCast = Time.time;

       		return true;
        }
        return false;
	}
}
