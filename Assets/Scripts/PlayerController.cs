using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private int m_iPlayerId;
    [SerializeField]
    private float m_fStunDuration = 1.5f;
    private bool m_bStunned = false;
    private CircleCollider2D Collider;

    private string m_sHorizontalAxisName;
    private string m_sVerticalAxisName;
	private KeyCode keyUp, keyDown, keyLeft, keyRight;

    // Use this for initialization
    void Awake ()
	{
        Collider = this.GetComponent<CircleCollider2D>();
        //Collider.enabled = false;

        m_sHorizontalAxisName = "HorizontalP" + m_iPlayerId;
        m_sVerticalAxisName = "VerticalP" + m_iPlayerId;
        Debug.Log(m_sHorizontalAxisName);

		if (m_iPlayerId == 0)
		{
			keyUp = KeyCode.UpArrow;
			keyDown = KeyCode.DownArrow;
			keyLeft = KeyCode.LeftArrow;
			keyRight = KeyCode.RightArrow;
		}
		else if (m_iPlayerId == 1)
		{
			keyUp = KeyCode.W;
			keyDown = KeyCode.S;
			keyLeft = KeyCode.A;
			keyRight = KeyCode.D;
		}
    }
	
	// Update is called once per frame
	void FixedUpdate ()
	{
        //Collider.enabled = false;

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

        if(!m_bStunned)
		{
			this.transform.position += new Vector3(
					Time.deltaTime * horizontal,
					Time.deltaTime * -1f * vertical);
            if (Input.GetButtonDown("A_P" + m_iPlayerId))
                Collider.enabled = true;
		}
	}

    public void Stun()
    {
		if (m_bStunned)
			return;
		Debug.Log("Player " + this.GetId() + " has been stunned");
        m_bStunned = true;
        if(!IsInvoking())
            Invoke("Reset", m_fStunDuration);
    }

    public void Reset()
    {
        m_bStunned = false;
        //Collider.enabled = false;
    }

    public void SetId(int id)
    {
        m_iPlayerId = id;
    }

    public int GetId()
    {
        return m_iPlayerId;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController otherPC = other.GetComponent<PlayerController>();

        if(otherPC != null && otherPC.GetId() != m_iPlayerId && Collider.isActiveAndEnabled)
            otherPC.Stun();
    }

}
