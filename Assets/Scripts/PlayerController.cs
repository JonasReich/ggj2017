using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private int m_iPlayerId;
    [SerializeField]
    private float m_fStunDuration = 1.5f;
    [SerializeField]
    private GameManager GameManager;
    private bool m_bStunned = false;
    private CircleCollider2D Collider;
    private float m_fCooldown = 2.0f;
    private SpriteRenderer m_Sprite;
    [SerializeField]
    private GameObject m_Particle;

    // Use this for initialization
    void Awake () {
        Collider = this.GetComponent<CircleCollider2D>();
        m_Sprite = this.GetComponent<SpriteRenderer>();
        Collider.enabled = false;
        m_Particle.SetActive(false);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        Collider.enabled = false;
        
        m_fCooldown -= Time.deltaTime;

        if(!m_bStunned)
        {
            this.transform.position += new Vector3(Time.deltaTime * Input.GetAxis("HorizontalP" + m_iPlayerId), Time.deltaTime * -1 * Input.GetAxis("VerticalP" + m_iPlayerId));

            if (Input.GetButtonDown("A_P" + m_iPlayerId) && m_fCooldown < 0.0f)
            {
                m_Particle.SetActive(true);
                Debug.Log(m_Particle.activeInHierarchy);
                if (!IsInvoking())
                    Invoke("ResetParticle", 0.5f);
                Collider.enabled = true;
                m_fCooldown = 2.0f;
            }   
        }
	}

    void ResetParticle()
    {
        m_Particle.SetActive(false);
    }


    public void Stun(int AttackerID)
    {
        m_bStunned = true;
        Vector2 AttackerPos = GameManager.GetPlayerPos(AttackerID);
        Vector2 thisPos = GameManager.GetPlayerPos(m_iPlayerId);
        this.transform.position -= new Vector3(AttackerPos.x - thisPos.x, AttackerPos.y - thisPos.y).normalized;
        if (!IsInvoking())
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
            otherPC.Stun(m_iPlayerId);
        }
    }

    public void SetGameManager(GameManager Manager)
    {
        GameManager = Manager;
    }

    public void SetSprite(Sprite _Sprite)
    {
        m_Sprite.sprite = _Sprite;
    }

    public void SetPosition(Vector2 Pos)
    {
        this.transform.position = Pos;
    }
}