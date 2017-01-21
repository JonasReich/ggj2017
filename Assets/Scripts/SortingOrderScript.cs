using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingOrderScript : MonoBehaviour {

	// Use this for initialization
	void Awake () {

        this.GetComponent<SpriteRenderer>().sortingOrder = -1 + (int)this.transform.position.y * -1;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
