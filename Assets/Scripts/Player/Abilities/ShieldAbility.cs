using UnityEngine;
using System.Collections;

public class ShieldAbility : IAbility {

    public float cooldown = 1;
    public float activeTime = 1;
    private float lastCast = 0;

    public bool isActivated = false;
    

    public void Update(){
        //test
        if(Input.GetKey("s")){
            this.Cast();
        }
    }
    
    public override bool Cast(){
       if (!isActivated && lastCast + cooldown <= Time.time)
       {
            isActivated = true;
            Debug.Log("Shield Up!");
            StartCoroutine(stopShieldIn(activeTime));
            return true;
       }    
       return false;
    }
    
    IEnumerator stopShieldIn(float time){
        yield return new WaitForSeconds(time);
        lastCast = Time.time;
        isActivated = false;
        Debug.Log("Shield Down!");

    }
    
}
