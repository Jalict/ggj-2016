using UnityEngine;
using System.Collections;

[RequireComponent(typeof (SpriteRenderer))]

public class AltarSprite : MonoBehaviour {
	public Sprite[] sprites = new Sprite[4];
	public SpriteRenderer spriteRend;

	void Start () {
		spriteRend = gameObject.GetComponent<SpriteRenderer>();
		spriteRend.enabled = false;
	}

	public void ChangeSprite(int index){
		spriteRend.sprite = sprites[index];
	}
}
