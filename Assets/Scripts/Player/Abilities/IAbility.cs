using UnityEngine;
using System.Collections;

public abstract class IAbility : MonoBehaviour {

    public Player owner;


    void Start(){
        owner = GetComponent<Player>();
    }
    
    public abstract bool Cast();
    public abstract bool CanCast();
    
}
