using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunCollisions : MonoBehaviour {

	public GameObject RadioStation;
	private StationCapture station;
    private ParticleCollisions ParticleCollision;
    private PlayerController PlayerControll;
    bool hit = false;
    float timer = 0;

    private GameManager game;
    public AudioClip[] sounds;

    public bool NotSetAlready = true;
    public int ParticlesForLvl2 = 5;
    private bool CastRing = false;


    private float m_fCooldown = 2;


    // Use this for initialization
    void Start () {
		station = RadioStation.GetComponent<StationCapture>();
        ParticleCollision = station.GetComponent<ParticleCollisions>();
        game = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
	
    void FixedUpdate()
    {
        m_fCooldown -= Time.deltaTime;
    }
	// Update is called once per frame
	void Update () {
        if (station.Level ==1)
            ParticleCollision.StunWaveDelay = 5;

        if (station.Level == 2)
        {
            if (NotSetAlready)
            {
                
                var PS = this.GetComponent<ParticleSystem>();
                var ParticleSystemMain = PS.main;

                if (!GetComponent<ParticleSystem>().IsAlive(false))
                {
                    NotSetAlready = false;
                    print("Is Alive");
                    ParticleSystemMain.maxParticles = ParticlesForLvl2;
                    var ParticleCollisionModulePrivate = PS.collision;
                    ParticleCollisionModulePrivate.bounce = 0.9f;
                    ParticleCollisionModulePrivate.minKillSpeed = 0;

                }
            }
        }
        if (station.Level == 3)
        {
            if (NotSetAlready)
            {
                NotSetAlready = false;
                var PS = this.GetComponent<ParticleSystem>();
                var ParticleSystemMain = PS.main;
                ParticleSystemMain.maxParticles = 450;
                ParticleCollision.StunWaveDelay = int.MaxValue;
                var ParticleCollisionModulePrivate = PS.collision;
                ParticleCollisionModulePrivate.minKillSpeed = 26;
                }
            if (Input.GetButtonDown("A_P" + gameObject.GetComponentInParent<StationCapture>().owner) && m_fCooldown < 0.0f)
            {
                if (CastRing == false) {
                    CastRing = true;
                    ParticleCollision.StunWaveDelay = 0;

                }
            }
            if (GetComponent<ParticleSystem>().IsAlive(false))
            ParticleCollision.StunWaveDelay = int.MaxValue;
            CastRing = false;
        }
        if (station.Level == 4)
        {
            ParticleCollision.StunWaveDelay = 2f;
        }

           
    

        if (station.Level >= 5)
        {
            game.SetGameFinished();
            game.Lvl5WinnerID = station.owner;

            //game.PlayerArray[station.owner].Points++;
            //Debug.Log("Player " + station.owner + " has " + game.PlayerArray[station.owner].Points + " Points");
        }

        if (game.gameFinished)
            return;

        timer += Time.deltaTime;
        if (timer >= 30)
        {
            if (hit == false)
            {
                station.IncLvl(1);
                ParticleCollision.DecStunWaveDelay(station.Level / 2);
            }
            timer = 0;
        }
		
	}

    void OnParticleCollision(GameObject other) {
		if (other == RadioStation || game.gameFinished)
			return;

		PlayerController pc = (PlayerController)
				other.GetComponent<PlayerController>();
		if (pc == null)
			return;

		//Debug.Log("inhere");

		if (station.GetOwner() != pc.GetId() && !hit)
        {
            if(hit == false) {
            PlayRandomSound();
            station.IncLvl(1);
            ParticleCollision.DecStunWaveDelay(station.Level / 2);
            hit = true;
            }
            
            if (!IsInvoking())
                Invoke("ResetHit", 2);
        }
			
    }

    void ResetHit()
    {
        hit = false;
    }

    void PlayRandomSound()
    {
        if (GetComponent<AudioSource>().isPlaying)
            return;
            GetComponent<AudioSource>().clip = sounds[Random.Range(0, sounds.Length)];
            GetComponent<AudioSource>().Play();        
    }

}
