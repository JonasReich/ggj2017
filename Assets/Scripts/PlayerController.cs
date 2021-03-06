﻿using UnityEngine;
using NONE;

public class PlayerController : MonoBehaviour {

	public SpriteRenderer circleSpriteRenderer;

	[SerializeField]
    private int m_iPlayerId;
    [SerializeField]
    private float m_fStunDuration = 0.5f;
    [SerializeField]
    private GameManager GameManager;
    private bool m_bStunned = false;
    public CircleCollider2D Collider;
    public float m_fCooldown = 2f;
    private SpriteRenderer m_Sprite;
    [SerializeField]
    private GameObject m_Particle;
    private Rigidbody2D m_Rigidbody;

    public float MovementSpeedFaktor = 4f;

    public int Points;

    public float knockBackTime = 2f;

	public GameObject Level;

    private string m_sHorizontalAxisName;
    private string m_sVerticalAxisName;
	private KeyCode keyUp, keyDown, keyLeft, keyRight, keyAttack;

	private Vector2 levelSize, levelPos;

    void Awake ()
	{
        m_fCooldown = 0;
        m_Sprite = this.GetComponent<SpriteRenderer>();
        m_Rigidbody = this.GetComponent<Rigidbody2D>();
        Collider.enabled = false;
        m_Particle.SetActive(false);

		var tr = Level.GetComponent<TextureReader>();
		levelSize = tr.GetSize();
		levelPos = tr.GetPos();
    }

	public void Initialize(int id) {
		//Debug.Log(levelSize);
		m_iPlayerId = id;
        m_sHorizontalAxisName = "HorizontalP" + m_iPlayerId;
        m_sVerticalAxisName = "VerticalP" + m_iPlayerId;
        //Debug.Log(m_sHorizontalAxisName);

		//Debug.Log(m_iPlayerId);
		if (m_iPlayerId == 0)
		{
			keyUp = KeyCode.UpArrow;
			keyDown = KeyCode.DownArrow;
			keyLeft = KeyCode.LeftArrow;
			keyRight = KeyCode.RightArrow;
			keyAttack = KeyCode.F;
		}
		else if (m_iPlayerId == 1)
		{
			keyUp = KeyCode.W;
			keyDown = KeyCode.S;
			keyLeft = KeyCode.A;
			keyRight = KeyCode.D;
			keyAttack = KeyCode.RightControl;
		}

		circleSpriteRenderer.color = GameManager.PlayerColor[m_iPlayerId];
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Collider.enabled = false;
        
        m_fCooldown -= Time.deltaTime;

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
            m_Rigidbody.velocity = new Vector3(
					Time.deltaTime * horizontal,
					Time.deltaTime * -1 * vertical).normalized * MovementSpeedFaktor;
			wrapAround();
		} else {
            m_Rigidbody.velocity = Vector3.zero;
		}

		if (Input.GetButtonDown("A_P" + m_iPlayerId) && m_fCooldown < 0.0f && !m_bStunned)
		{
            this.GetComponent<AudioSource>().Play();
            MovementSpeedFaktor = 10f;
            m_Particle.SetActive(true);
                //Debug.Log(m_Particle.activeInHierarchy);
                if (!IsInvoking())
                    Invoke("ResetParticle", knockBackTime);
                Collider.enabled = true;
		}

		bool attack = Input.GetButtonDown("A_P" + m_iPlayerId)
				|| Input.GetKeyDown(keyAttack);

		if (attack && m_fCooldown < 0.0f)
		{

			m_Particle.SetActive(true);
			Debug.Log(m_Particle.activeInHierarchy);
			if (!IsInvoking())
				Invoke("ResetParticle", knockBackTime);
			Collider.enabled = true;
			m_fCooldown = 2.0f;
		}   
	}

	void wrapAround() {
		Vector2 pos = m_Rigidbody.position;
		if (pos.x >= levelPos.x + levelSize.x)
			pos.x = levelPos.x;
		else if (pos.x < levelPos.x)
			pos.x = levelPos.x + levelSize.x;

		if (pos.y >= levelPos.y + levelSize.y)
			pos.y = levelPos.y;
		else if (pos.y < levelPos.y)
			pos.y = levelPos.y + levelSize.y;
		m_Rigidbody.position = pos;
	}

    void ResetParticle()
    {
        m_Particle.SetActive(false);
        MovementSpeedFaktor = 4f;
    }


    public void Stun(int AttackerID)
    {
		if (m_bStunned)
			return;
		//Debug.Log("Player " + this.GetId() + " has been stunned");
        m_bStunned = true;
        Vector2 AttackerPos = GameManager.GetPlayerPos(AttackerID);
        Vector2 thisPos = GameManager.GetPlayerPos(m_iPlayerId);
        this.transform.position -= new Vector3(
				AttackerPos.x - thisPos.x, AttackerPos.y - thisPos.y).normalized;
		wrapAround();
        if (!IsInvoking())
            Invoke("Reset", m_fStunDuration);
    }

	public void MoveToSpawn() {
        if (m_iPlayerId < 2)
            SetPosition(new Vector2(1 + m_iPlayerId, 2 + m_iPlayerId));
        if (m_iPlayerId == 2)
            SetPosition(new Vector2(m_iPlayerId, m_iPlayerId));
        if (m_iPlayerId == 3)
            SetPosition(new Vector2(1, m_iPlayerId));
    }

    public void Reset()
    {
        m_bStunned = false;
    }

    public void SetId(int id)
    {
        m_iPlayerId = id;
    }

    public int GetId()
    {
        return m_iPlayerId;
    }

    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    PlayerController otherPC = other.GetComponent<PlayerController>();
    //
    //    if(otherPC != null && otherPC.GetId() != m_iPlayerId && Collider.isActiveAndEnabled)
    //        otherPC.Stun(m_iPlayerId);
    //}

    public void SetGameManager(GameManager Manager)
    {
        GameManager = Manager;
    }

	public void SetLevel(TextureReader level) {
		levelSize = level.GetSize();
		levelPos = level.GetPos();
		//Debug.Log("Level size: " + levelSize);
	}

    public void SetPosition(Vector2 Pos)
    {
        transform.position = Pos;
    }

    public void SetSprite(Sprite _sprite)
    {
        m_Sprite.sprite = _sprite;
    }


    public int GetPoints()
    {


        return Points;
    }
}
