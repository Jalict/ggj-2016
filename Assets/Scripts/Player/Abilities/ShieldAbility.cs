using UnityEngine;
using System.Collections;

public class ShieldAbility : IAbility {

    private float cooldown;
    private float lastCast = 0;
    
    public bool canCast = false;
    
    public ShieldAbility(float cooldown){
        this.cooldown = cooldown;
    }
    
    public override bool Cast(){
       if (cooldown <= 0)
       {
           return true;
       }
       
       return false;
    }
    
}
