using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderScript : MonoBehaviour {

	public int offset;
	SpriteRenderer spriteRenderer;

	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update () {
		spriteRenderer.sortingOrder = (int) (offset -1 + transform.position.y * -2);
	}
}
