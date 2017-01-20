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

    // Use this for initialization
    void Awake () {
        Collider = this.GetComponent<CircleCollider2D>();
        Collider.enabled = false;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Collider.enabled = false;

        if(!m_bStunned)
        {
            this.transform.position += new Vector3(Time.deltaTime * Input.GetAxis("HorizontalP" + m_iPlayerId), Time.deltaTime * -1 * Input.GetAxis("VerticalP" + m_iPlayerId));

            if (Input.GetButtonDown("A_P" + m_iPlayerId))
                Collider.enabled = true;
        }
	}

    public void Stun()
    {
        m_bStunned = true;
        if(!IsInvoking())
            Invoke("Reset", m_fStunDuration);
    }

    public void Reset()
    {
        m_bStunned = false;
        Collider.enabled = false;
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
        {
            otherPC.Stun();
        }
    }
}
