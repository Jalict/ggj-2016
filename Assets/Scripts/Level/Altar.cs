using UnityEngine;
using System.Collections;

public class Altar : MonoBehaviour {
	public Sprite[] sprites = new Sprite[6];
	public AltarSprite altarSprite;

	// Use this for initialization
	void Start () {
		altarSprite = transform.GetChild(0).GetComponent<AltarSprite>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
