using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
	public Sprite[] sprites;
	
	void Awake () {
		var randomIndex = UnityEngine.Random.Range(0, sprites.Length);
		GetComponent<SpriteRenderer>().sprite = sprites[randomIndex];
	}
}
