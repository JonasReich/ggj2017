using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private int m_iPlayerId;

	// Use this for initialization
	void Awake () {


		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        this.transform.position += new Vector3(Time.deltaTime * Input.GetAxis("HorizontalP" + m_iPlayerId), Time.deltaTime * Input.GetAxis("VerticalP" + m_iPlayerId));
		
	}
}
