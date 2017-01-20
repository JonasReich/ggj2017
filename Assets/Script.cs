using UnityEngine;

public class Script : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void Collide(Transform collider) {
	}

	void OnCollisionEnter2D(Collision2D collider) {
		Debug.Log("collision enter");
	}

}
