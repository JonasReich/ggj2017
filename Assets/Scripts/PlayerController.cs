using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private int m_iPlayerId;

    private string m_sHorizontalAxisName;
    private string m_sVerticalAxisName;

    // Use this for initialization
    void Awake () {

        m_sHorizontalAxisName = "HorizontalP" + m_iPlayerId;
        m_sVerticalAxisName = "VerticalP" + m_iPlayerId;
        Debug.Log(m_sHorizontalAxisName);
		

    }
	
	// Update is called once per frame
	void FixedUpdate () {

        this.transform.position += new Vector3(Time.deltaTime * Input.GetAxis(m_sHorizontalAxisName), Time.deltaTime * -1 * Input.GetAxis(m_sVerticalAxisName));
		
	}
}
