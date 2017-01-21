using UnityEngine;
using NONE;

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
    private Rigidbody2D m_Rigidbody;

	public float knockBackTime = 2f;

	public GameObject Level;

    private string m_sHorizontalAxisName;
    private string m_sVerticalAxisName;
	private KeyCode keyUp, keyDown, keyLeft, keyRight, keyAttack;

	private Vector2 levelSize, levelPos;

    void Awake ()
	{
        Collider = this.GetComponent<CircleCollider2D>();
        m_Sprite = this.GetComponent<SpriteRenderer>();
        m_Rigidbody = this.GetComponent<Rigidbody2D>();
        Collider.enabled = false;
        m_Particle.SetActive(false);

		var tr = Level.GetComponent<TextureReader>();
		levelSize = tr.GetSize();
		Debug.Log(levelSize);
		levelPos = tr.GetPos();
    }

	public void Initialize(int id) {
		m_iPlayerId = id;
        m_sHorizontalAxisName = "HorizontalP" + m_iPlayerId;
        m_sVerticalAxisName = "VerticalP" + m_iPlayerId;
        Debug.Log(m_sHorizontalAxisName);

		Debug.Log(m_iPlayerId);
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
					Time.deltaTime * -1 * vertical).normalized * 3f;
			wrapAround();
		} else {
            m_Rigidbody.velocity = Vector3.zero;
		}

		if (Input.GetButtonDown("A_P" + m_iPlayerId) && m_fCooldown < 0.0f)
		{
			m_Particle.SetActive(true);
                Debug.Log(m_Particle.activeInHierarchy);
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
    }


    public void Stun(int AttackerID)
    {
		if (m_bStunned)
			return;
		Debug.Log("Player " + this.GetId() + " has been stunned");
        m_bStunned = true;
        Vector2 AttackerPos = GameManager.GetPlayerPos(AttackerID);
        Vector2 thisPos = GameManager.GetPlayerPos(m_iPlayerId);
        this.transform.position -= new Vector3(
				AttackerPos.x - thisPos.x, AttackerPos.y - thisPos.y).normalized;
		wrapAround();
        if (!IsInvoking())
            Invoke("Reset", m_fStunDuration);
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

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController otherPC = other.GetComponent<PlayerController>();

        if(otherPC != null && otherPC.GetId() != m_iPlayerId && Collider.isActiveAndEnabled)
            otherPC.Stun(m_iPlayerId);
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
