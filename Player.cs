﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	public bool Player2;

	public int Vie;
	public int Munitions;

	public Rigidbody2D p_rb;
	public float v_saut;
	public float vitesse;
	public float v_saut_max;
	public float vitesse_max;

	public Color couleur_Joueur;

	public bool construction;
	public GameObject constructionMenu;
	public GameObject selecteur;

	public Text prix;

	public GameObject blocPrefab;

    public SpriteRenderer aspect_joueur_coloré;
	public SpriteRenderer aspect_joueur_fixe;
	public SpriteRenderer aspect_selecteur;

    public bool Flip;

    public Sprite sprite;
    public Sprite sprite_inv;

    public Color redAlpha;
	public Color grayAlpha;
	public Color greenAlpha;
	public Color yellowAlpha;

	public int BlockingLayer;

	public bool SnowPile;

    public float timeShoot;
    public float valueYShoot;

	public GameObject snowballPrefab;

    public GameController gameController;

    public Slider slider_shoot;

    public Animator animator;

    public AudioSource audio_src;
    public AudioClip son_mort;
    public AudioClip son_ramasser;

	// Use this for initialization
	public void Start () {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        aspect_joueur_fixe = GetComponent<SpriteRenderer>();
		aspect_joueur_coloré = this.transform.GetChild (0).GetComponent<SpriteRenderer> ();

        animator = GetComponent<Animator>();

        audio_src = GetComponent<AudioSource>();

        //Valeurs initials
		Vie = 100;
		Munitions = 10;

		p_rb = this.gameObject.GetComponent<Rigidbody2D> ();

		constructionMenu.SetActive (false);

		aspect_joueur_coloré.color = couleur_Joueur;
	}
	
	// Update is called once per frame
	void Update () {
        float deltaTime = Time.time - timeShoot;
        valueYShoot = (float)(deltaTime + 0.05f * Mathf.Exp(deltaTime));
        

        //Orientation du joueur (droite ou gauche celon la direction)
        if (Flip == true)
        {
            //ANIM -> INVERSE
            aspect_joueur_coloré.flipX = true;
            aspect_joueur_fixe.flipX = true;

        }
        else
        {
            aspect_joueur_coloré.flipX = false;
            aspect_joueur_fixe.flipX = false;
        }


        //Si la vie est égale ou inférieure à 0 on tue le joueur
        if (Vie <= 0 && p_rb.isKinematic == false)
        {
            audio_src.clip = son_mort;
            this.gameObject.transform.rotation = new Quaternion(0, 0, 90, 0);
            audio_src.Play();
            gameController.endGame();
        }

        //MOUVEMENT DU JOUEUR
		if (Player2 == false) {
            //JOUEUR 1
			if (construction == false) {
                /* CONTROL JOUEUR 1 NORMAL */
				if (Input.GetKey (KeyCode.Z)) {
					if (p_rb.velocity.y == 0) {
						p_rb.AddForce (Vector2.up * v_saut);
                        //SAUT
					}
				}

				if (Input.GetKey(KeyCode.Q)){
                    Flip = true;
                    p_rb.AddForce (Vector2.left * vitesse);
                    //GAUCHE
				}

				if (Input.GetKey(KeyCode.D)){
                    Flip = false;
					p_rb.AddForce (Vector2.right * vitesse);
                    //DROITE
				}

                //ACCROUPI
				if (Input.GetKey (KeyCode.S)) {
					p_rb.AddForce (Vector2.up * -v_saut);
                    animator.SetBool("Crouch", true);
                    this.transform.localScale = new Vector3 (1, 0.5f, 1);
                } else {
                    animator.SetBool("Crouch", false);
                    this.transform.localScale = new Vector3 (1, 1, 1);
                }

                //TIR
                if (Input.GetKeyDown(KeyCode.E))
                {
                    timeShoot = Time.time;
                }

                //TIR BIS (temps d'appuie pour jauger la puissance)
                if (Input.GetKey(KeyCode.E))
                {
                    slider_shoot.value = valueYShoot;
                    animator.SetBool("Throw", true);
                }
                else
                {
                    slider_shoot.value = 0;
                    animator.SetBool("Throw", false);
                    
                }

                if (Input.GetKeyUp(KeyCode.E))
                {
                    tir_ramasser(timeShoot);
                }

                //CONSTRUIRE
                if (Input.GetKeyDown (KeyCode.A)) {
					ouvrir_construction ();
				}

			} else {
                /* CONTROL JOUEUR 1 CONSTRUCTION*/
				aspect_selecteur = selecteur.GetComponent<SpriteRenderer> ();
				RaycastHit2D center;
				RaycastHit2D left;
				RaycastHit2D right;
				RaycastHit2D up;
				RaycastHit2D down;

                //Detection d'obstacle
				center = Physics2D.Raycast (new Vector2(selecteur.transform.position.x + 0.5f, selecteur.transform.position.y - 0.5f), Vector2.zero, 0.4f);
				left = Physics2D.Raycast (new Vector2(selecteur.transform.position.x, selecteur.transform.position.y - 0.5f), Vector2.left, 0.4f);
				right = Physics2D.Raycast (new Vector2(selecteur.transform.position.x + 1f, selecteur.transform.position.y - 0.5f), Vector2.right, 0.4f);
				up = Physics2D.Raycast (new Vector2(selecteur.transform.position.x + 0.5f, selecteur.transform.position.y), Vector2.up, 0.4f);
				down = Physics2D.Raycast (new Vector2(selecteur.transform.position.x + 0.5f, selecteur.transform.position.y - 1f), Vector2.down, 0.4f);

				int x = 0;
				int y = 0;

                //HAUT
				if (Input.GetKey (KeyCode.Z)) {
					if (up.transform == null || up.transform.gameObject.layer != BlockingLayer || up.transform.gameObject.tag == "Bloc") {
						y = y+1;
					}
				}

                //GAUCHE
				if (Input.GetKey(KeyCode.Q)){
					if (left.transform == null || left.transform.gameObject.layer != BlockingLayer || left.transform.gameObject.tag == "Bloc") {
						x= x-1;
					}
				}

                //DROITE
				if (Input.GetKey(KeyCode.D)){
					if (right.transform == null || right.transform.gameObject.layer != BlockingLayer || right.transform.gameObject.tag == "Bloc") {
						x=x+1;
					}
				}

                //BAS
				if (Input.GetKey (KeyCode.S)) {
					if (down.transform == null || down.transform.gameObject.layer != BlockingLayer || down.transform.gameObject.tag == "Bloc") {
						y=y-1;
					}
				}

                //CENTRE (couleur du selecteur)
				if (center.transform == null) {
					if (left.transform != null && left.transform.gameObject.layer == BlockingLayer || right.transform != null && right.transform.gameObject.layer == BlockingLayer || up.transform != null && up.transform.gameObject.layer == BlockingLayer || down.transform != null && down.transform.gameObject.layer == BlockingLayer) {
							aspect_selecteur.color = greenAlpha;
							prix.text = "3";
					} else {
						aspect_selecteur.color = redAlpha;
					}
				} else if (center.transform.gameObject.tag == "Bloc") {
					prix.text = "5";
					aspect_selecteur.color = yellowAlpha;
				}

                //CONSTRUCTION
				if (Input.GetKeyDown(KeyCode.E)){
					if (center.transform == null) {
						if (left.transform != null && left.transform.gameObject.layer == BlockingLayer || right.transform != null && right.transform.gameObject.layer == BlockingLayer || up.transform != null && up.transform.gameObject.layer == BlockingLayer || down.transform != null && down.transform.gameObject.layer == BlockingLayer) {
							construire ();
						}
					} else if (center.transform.gameObject.tag == "Bloc") {
						upgrade (center.transform.gameObject);
					}
				}

                //FERMETURE DU MENU
				if (Input.GetKeyDown (KeyCode.A)) {
					ouvrir_construction ();
				}

                selecteur.transform.position = new Vector2(Mathf.Clamp(selecteur.transform.position.x + (x * Time.fixedDeltaTime * vitesse * 0.6f), -14, -1), Mathf.Clamp(selecteur.transform.position.y + (y * Time.fixedDeltaTime * vitesse * 0.6f), 0, 19));

            }
            /*FIN JOUEUR 1 */
		} else {
            /* JOUEUR 2 */
            if (construction == false)
            {
                /* CONTROL JOUEUR 2 NORMAL */

                //SAUT
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    if (p_rb.velocity.y == 0)
                    {
                        p_rb.AddForce(Vector2.up * v_saut);
                    }
                }

                //GAUCHE
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    Flip = true;
                    p_rb.AddForce(Vector2.left * vitesse);
                }

                //DROITE
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    Flip = false;
                    p_rb.AddForce(Vector2.right * vitesse);
                }

                //ACCROUPI
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    p_rb.AddForce(Vector2.up * -v_saut);
                    this.transform.localScale = new Vector3(1, 0.5f, 1);
                    animator.SetBool("Crouch", true);
                }
                else
                {
                    this.transform.localScale = new Vector3(1, 1, 1);
                    animator.SetBool("Crouch", false);
                }

                //TIR
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    timeShoot = Time.time;
                }

                //TIR BIS (jauge de puissance)
                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    tir_ramasser(timeShoot);
                }

                if (Input.GetKey(KeyCode.Mouse0))
                {
                    slider_shoot.value = valueYShoot;
                    animator.SetBool("Throw", true);
                }
                else
                {
                    slider_shoot.value = 0;
                    animator.SetBool("Throw", false);
                }

                //CONSTRUCTION
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    ouvrir_construction();
                }

            }
            else
            {
                /* CONTROL JOUEUR 2 CONSTRUCTION */
                aspect_selecteur = selecteur.GetComponent<SpriteRenderer>();
                RaycastHit2D center;
                RaycastHit2D left;
                RaycastHit2D right;
                RaycastHit2D up;
                RaycastHit2D down;

                //LASER DE DEBUG
                Debug.DrawRay(new Vector2(selecteur.transform.position.x, selecteur.transform.position.y - 0.5f), Vector2.left, Color.red);
                Debug.DrawRay(new Vector2(selecteur.transform.position.x + 1, selecteur.transform.position.y - 0.5f), Vector2.right, Color.blue);
                Debug.DrawRay(new Vector2(selecteur.transform.position.x + 0.5f, selecteur.transform.position.y), Vector2.up, Color.green);
                Debug.DrawRay(new Vector2(selecteur.transform.position.x + 0.5f, selecteur.transform.position.y - 1f), Vector2.down, Color.yellow);

                //DETECTION D'OBSTACLES
                center = Physics2D.Raycast(new Vector2(selecteur.transform.position.x + 0.5f, selecteur.transform.position.y - 0.5f), Vector2.zero, 0.4f);

                right = Physics2D.Raycast(new Vector2(selecteur.transform.position.x, selecteur.transform.position.y - 0.5f), Vector2.left, 0.4f);
                left = Physics2D.Raycast(new Vector2(selecteur.transform.position.x + 1f, selecteur.transform.position.y - 0.5f), Vector2.right, 0.4f);
                up = Physics2D.Raycast(new Vector2(selecteur.transform.position.x + 0.5f, selecteur.transform.position.y), Vector2.up, 0.4f);
                down = Physics2D.Raycast(new Vector2(selecteur.transform.position.x + 0.5f, selecteur.transform.position.y - 1f), Vector2.down, 0.4f);


                int x = 0;
                int y = 0;

                //HAUT

                if (Input.GetKey(KeyCode.UpArrow))
                {
                    if (up.transform == null || up.transform.gameObject.layer != BlockingLayer || up.transform.gameObject.tag == "Bloc")
                    {
                        y = y + 1;
                    }
                }

                //GAUCHE
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (left.transform == null || left.transform.gameObject.layer != BlockingLayer || left.transform.gameObject.tag == "Bloc")
                    {
                        x = x - 1;
                    }
                }

                //DROITE
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (right.transform == null || right.transform.gameObject.layer != BlockingLayer || right.transform.gameObject.tag == "Bloc")
                    {
                        x = x + 1;
                    }
                }

                //BAS
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    if (down.transform == null || down.transform.gameObject.layer != BlockingLayer || down.transform.gameObject.tag == "Bloc")
                    {
                        y = y - 1;
                    }
                }

                //CENTRE (couleur du selecteur)
                if (center.transform == null)
                {
                    if (left.transform != null && left.transform.gameObject.layer == BlockingLayer || right.transform != null && right.transform.gameObject.layer == BlockingLayer || up.transform != null && up.transform.gameObject.layer == BlockingLayer || down.transform != null && down.transform.gameObject.layer == BlockingLayer)
                    {
                        aspect_selecteur.color = greenAlpha;
						prix.text = "3";
                    }
                    else
                    {
                        aspect_selecteur.color = redAlpha;
                    }
                }
                else if (center.transform.gameObject.tag == "Bloc")
                {
                    aspect_selecteur.color = yellowAlpha;
					prix.text = "5";
                }

                //CONSTRUIRE
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (center.transform == null)
                    {
                        if (left.transform != null && left.transform.gameObject.layer == BlockingLayer || right.transform != null && right.transform.gameObject.layer == BlockingLayer || up.transform != null && up.transform.gameObject.layer == BlockingLayer || down.transform != null && down.transform.gameObject.layer == BlockingLayer)
                        {
                            construire();
                        }
                    }
                    else if (center.transform.gameObject.tag == "Bloc")
                    {
                        upgrade(center.transform.gameObject);
                    }
                }

                //FERMETURE DU MENU
                if (Input.GetKeyDown(KeyCode.Mouse1))
                {
                    ouvrir_construction();
                }
               
                selecteur.transform.position = new Vector2(Mathf.Clamp(selecteur.transform.position.x + (x * Time.fixedDeltaTime * vitesse * 0.6f), 1, 14), Mathf.Clamp(selecteur.transform.position.y + (y * Time.fixedDeltaTime * vitesse * 0.6f), 0, 19));
            }
                /* CONTROL XBOX 360 ===================================================================================================
                 * 
                 * Les controls ne fonctionnaient pas totalement alors on a decidé de passer les deux joueurs sur le même clavier. Mais vu que j'etais proche du but j'ai décidé de garder ce qui avait été fait pour la manette au cas où je trouve une solution. 
                 * 
                 * 
                 * 
                 * 
                if (construction == false) {
                    if (Input.GetAxis ("Top360Y") > 0.5f || Input.GetAxis ("Cross360Y") < -0.5f || Input.GetKeyDown(KeyCode.JoystickButton0)) {
                        if (p_rb.velocity.y == 0) {
                            p_rb.AddForce (Vector2.up * v_saut);
                        }
                    }

                    if (Input.GetAxis ("Top360X") < 0 || Input.GetAxis ("Cross360X") < 0){
                        p_rb.AddForce (Vector2.left * vitesse);
                    }

                    if (Input.GetAxis ("Top360X") > 0 || Input.GetAxis ("Cross360X") > 0){
                        p_rb.AddForce (Vector2.right * vitesse);
                    }

                    if (Input.GetAxis ("Top360Y") < -0.5f || Input.GetAxis ("Cross360Y") > 0.5f) {
                        p_rb.AddForce (Vector2.up * -v_saut);
                        this.transform.localScale = new Vector3 (1, 1, 1);
                    } else {
                        this.transform.localScale = new Vector3 (1, 2, 1);
                    }


                    if (Input.GetAxis("Gachette360") > 0 || Input.GetKeyDown(KeyCode.JoystickButton2)){
                        tir_ramasser ();
                    }

                    if (Input.GetKeyDown(KeyCode.JoystickButton1)) {
                        ouvrir_construction ();
                    }

                } else {
                    aspect_selecteur = selecteur.GetComponent<SpriteRenderer> ();
                    RaycastHit2D center;
                    RaycastHit2D left;
                    RaycastHit2D right;
                    RaycastHit2D up;
                    RaycastHit2D down;

                    center = Physics2D.Raycast(new Vector2(selecteur.transform.position.x + 0.5f, selecteur.transform.position.y - 0.5f), Vector2.zero, 0.4f);
                    left = Physics2D.Raycast(new Vector2(selecteur.transform.position.x, selecteur.transform.position.y - 0.5f), Vector2.left, 0.4f);
                    right = Physics2D.Raycast(new Vector2(selecteur.transform.position.x + 1f, selecteur.transform.position.y - 0.5f), Vector2.right, 0.4f);
                    up = Physics2D.Raycast(new Vector2(selecteur.transform.position.x + 0.5f, selecteur.transform.position.y), Vector2.up, 0.4f);
                    down = Physics2D.Raycast(new Vector2(selecteur.transform.position.x + 0.5f, selecteur.transform.position.y - 1f), Vector2.down, 0.4f);
                    Debug.Log("up " + up.transform);
                    Debug.Log("left " + left.transform);
                    Debug.Log("right " + right.transform);
                    Debug.Log("down " + down.transform);

                    float x = 0;
                    float y = 0;

                    if (Input.GetAxis ("Top360Y") > 0 || Input.GetAxis ("Cross360Y") > 0.5f || Input.GetKeyDown(KeyCode.JoystickButton0)) {
                        Debug.Log("UP");
                        if (up.transform == null || up.transform.gameObject.layer != BlockingLayer || up.transform.gameObject.tag == "Bloc") {
                            Debug.Log("UP_CLEAR");
                            y = Input.GetAxis ("Top360Y");
                        }
                    }

                    if (Input.GetAxis ("Top360X") < 0 || Input.GetAxis ("Cross360X") < 0){
                        Debug.Log("LEFT");
                        if (left.transform == null || left.transform.gameObject.layer != BlockingLayer || left.transform.gameObject.tag == "Bloc") {
                            Debug.Log("LEFT_CLEAR");
                            x = Input.GetAxis ("Top360X");
                        }
                    }

                    if (Input.GetAxis ("Top360X") > 0 || Input.GetAxis ("Cross360X") > 0){
                        Debug.Log("RIGHT");
                        if (right.transform == null || right.transform.gameObject.layer != BlockingLayer || right.transform.gameObject.tag == "Bloc") {
                            Debug.Log("RIGHT_CLEAR");
                            x =Input.GetAxis ("Top360X");
                        }
                    }

                    if (Input.GetAxis ("Top360Y") < 0 || Input.GetAxis ("Cross360Y") < -0.5f) {
                        Debug.Log("DOWN");
                        if (down.transform == null || down.transform.gameObject.layer != BlockingLayer || down.transform.gameObject.tag == "Bloc") {
                            Debug.Log("DOWN_CLEAR");
                            y =Input.GetAxis ("Top360Y");
                        }
                    }

                    if (center.transform == null) {
                        if (left.transform != null && left.transform.gameObject.layer == BlockingLayer || right.transform != null && right.transform.gameObject.layer == BlockingLayer || up.transform != null && up.transform.gameObject.layer == BlockingLayer || down.transform != null && down.transform.gameObject.layer == BlockingLayer) {
                            aspect_selecteur.color = greenAlpha;
                        } else {
                            aspect_selecteur.color = redAlpha;
                        }
                    } else if (center.transform.gameObject.tag == "Bloc") {
                        aspect_selecteur.color = yellowAlpha;
                    }
                    
                        Construire !


                    if (Input.GetKeyDown(KeyCode.JoystickButton1)) {
                        ouvrir_construction ();
                    }
                    selecteur.transform.position = new Vector2 (selecteur.transform.position.x + (x*Time.fixedDeltaTime*vitesse*0.6f), selecteur.transform.position.y + (y*Time.fixedDeltaTime*vitesse*0.6f));

                }
                */



            }

        


    }


    //Fonction lu juste après update() permettant de corriger les deplacement
	void LateUpdate() {
        //Si le joueur a de la vitesse alors il joue son animation de course
        if (Mathf.Abs(p_rb.velocity.x) > 0.3f)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }

        //Limitation de la vitesse entre un maximum et un minimum
		p_rb.velocity = new Vector2(Mathf.Clamp(p_rb.velocity.x, -vitesse_max, vitesse_max), Mathf.Clamp(p_rb.velocity.y, -100, v_saut_max));

        //Limitation du selecteur pour qu'il est une position composé de nombre entier (afin de respecter la grille)
		if (construction == true){
			if (Player2 == false) {
				if (Input.GetKeyUp (KeyCode.Z) || Input.GetKeyUp (KeyCode.Q) || Input.GetKeyUp (KeyCode.D) || Input.GetKeyUp (KeyCode.S)) {
					selecteur.transform.position = new Vector2 (Mathf.RoundToInt (selecteur.transform.position.x), Mathf.RoundToInt (selecteur.transform.position.y));
				}
			} else {
				if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.DownArrow)) {
					selecteur.transform.position = new Vector2 (Mathf.RoundToInt (selecteur.transform.position.x), Mathf.RoundToInt (selecteur.transform.position.y));
				}
			}
		}

	}


    //Switch du menu construction
	void ouvrir_construction(){
        if (p_rb.isKinematic == false)
        {
            if (construction == false)
            {
                constructionMenu.SetActive(true);
                construction = true;
            }
            else
            {
                constructionMenu.SetActive(false);
                construction = false;
            }
        }

	}

    //Construction et retrait du coùt de construction
	void construire(){
		if (Munitions >= 3) {
			Munitions = Munitions - 3;
			GameObject obj = Instantiate (blocPrefab, selecteur.transform.position, Quaternion.identity) as GameObject;
		}
	}

    //Amélioration et retrait du coùt d'amélioration
    void upgrade(GameObject obj){
		Bloc bloc = obj.GetComponent<Bloc> ();
		if (bloc.bloc == "Neige") {
			if (Munitions >= 5) {
				bloc.Upgrade ();
				Munitions = Munitions -5;
			}
		} else if (bloc.bloc == "Glace"){

		}
	}

    //Detection si dans la zone d'une pile de neige
	public void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "SnowPile") {
			SnowPile = true;
		}
	}

    //Fin de detection de pile de neige
	public void OnTriggerExit2D(Collider2D other){
		if (other.tag == "SnowPile") {
			SnowPile = false;
		}
	}

    //Tir
	void tir_ramasser(float time){
        //Si pile de neige alors ramasse une boule
		if (SnowPile == true) {
			Munitions++;
            audio_src.clip = son_ramasser;
            audio_src.Play();
        } else {
        //Sinon tir une boule 
            if (Munitions > 0 && p_rb.isKinematic == false)
            {
                    //Création de la boule de neige
                    GameObject obj = Instantiate(snowballPrefab, new Vector2(this.transform.position.x, this.transform.position.y), Quaternion.identity) as GameObject;
                    Rigidbody2D objRb = obj.GetComponent<Rigidbody2D>();
                    obj.GetComponent<SnowBall>().player = this.transform;
                    float x;
                    //direction de la boule en fonction de l'orientation
                    if (Flip == true)
                    {
                        x = -1;
                    }
                    else
                    {
                        x = 1;
                    }
                    //Propulsion de la boule de neige
                    objRb.AddForce(new Vector2(x * vitesse * 100, valueYShoot * vitesse * 100));
                    //retrait de stock de boule de neige
                    Munitions--;
                
               
            }
		}
	}

}
