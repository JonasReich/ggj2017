using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //[SerializeField]
    public int PlayerId {
		get { return m_iPlayerId; }
		private set { m_iPlayerId = value; }
	}

	public int m_iPlayerId;
    private string m_sHorizontalAxisName;
    private string m_sVerticalAxisName;
	private KeyCode keyUp, keyDown, keyLeft, keyRight;

    // Use this for initialization
    void Awake () {

        m_sHorizontalAxisName = "HorizontalP" + m_iPlayerId;
        m_sVerticalAxisName = "VerticalP" + m_iPlayerId;
        Debug.Log(m_sHorizontalAxisName);

		if (m_iPlayerId == 0) {
			keyUp = KeyCode.UpArrow;
			keyDown = KeyCode.DownArrow;
			keyLeft = KeyCode.LeftArrow;
			keyRight = KeyCode.RightArrow;
		} else if (m_iPlayerId == 1) {
			keyUp = KeyCode.W;
			keyDown = KeyCode.S;
			keyLeft = KeyCode.A;
			keyRight = KeyCode.D;
		}
    }
	
	// Update is called once per frame
	void FixedUpdate () {

		float horizontal = Input.GetAxis(m_sHorizontalAxisName);
		if (Input.GetKey(keyRight))
			horizontal = 1f;
		else if (Input.GetKey(keyLeft))
			horizontal = -1f;
		float vertical = Input.GetAxis(m_sVerticalAxisName);
		if (Input.GetKey(keyUp))
			vertical = -1f;
		else if (Input.GetKey(keyDown))
			vertical = 1f;

        this.transform.position += new Vector3(
				Time.deltaTime * horizontal,
				Time.deltaTime * -1f * vertical);
	}


}
