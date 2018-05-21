using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour {

	Rigidbody2D rb;
    float idleTime = 0;
    public Transform player;
    public AudioClip audio_clip;
    AudioSource audio_src;
    ParticleSystem particles_sys;
    SpriteRenderer aspect;
	// Use this for initialization
	void Start () {
		rb = this.gameObject.GetComponent<Rigidbody2D> ();
        audio_src = this.gameObject.GetComponent<AudioSource>();
        particles_sys = this.gameObject.GetComponent<ParticleSystem>();
        aspect = this.gameObject.GetComponent<SpriteRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
		if (rb.velocity == Vector2.zero) {
            if (idleTime == 0)
            {
                idleTime = Time.time;
            }
            else
            {
                if (Time.time - idleTime > 5)
                {
                    StartCoroutine(Death());
                }
            }
			
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Terrain" || other.tag == "Bloc") {

            StartCoroutine(Death());
		}

        if (other.tag == "Player")
        {
            if (other.transform != player)
            {
                other.gameObject.GetComponent<Player>().Vie = other.gameObject.GetComponent<Player>().Vie - 5;
                if (other.gameObject.GetComponent<Player>().Player2 == false)
                {
                    other.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(-250, 0));
                }else
                {
                    other.gameObject.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(250, 0));
                }
            }
        }
	}

    IEnumerator Death()
    {
        audio_src.clip = audio_clip;
        audio_src.Play();
        particles_sys.Play();
        aspect.color = Color.clear;
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        yield return new WaitForSeconds(0.4f);
        Destroy(this.gameObject);

    }
}
