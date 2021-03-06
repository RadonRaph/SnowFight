﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloc : MonoBehaviour {
	public Sprite SpriteNeige;
	public Sprite SpriteGlace;

	public SpriteRenderer aspect;
	public GameController GameController;


	public int vie;

	public string bloc;

	RaycastHit2D left;
	RaycastHit2D right;
	RaycastHit2D up;
	RaycastHit2D down;

	public int BlockingLayer;

	public int contact;
	public int contactDirect;
	public int contactIndirect;
	public Bloc joint;

	public Rigidbody2D rb;
	// Use this for initialization
	void Start () {
		GameController = GameObject.Find ("GameController").GetComponent<GameController> ();
	 	GameController.AddBloc (this.gameObject);

		aspect = this.GetComponent<SpriteRenderer> ();

        //Valeurs initials
		vie = 50;
		bloc = "Neige";
		aspect.sprite = SpriteNeige;

		rb = this.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
        //Detection des blocs voisins
		left = Physics2D.Raycast (new Vector2(this.transform.position.x - 0.1f, this.transform.position.y - 0.5f), Vector2.left, 0.4f);
		right = Physics2D.Raycast (new Vector2(this.transform.position.x + 1.1f, this.transform.position.y - 0.5f), Vector2.right, 0.4f);
		up = Physics2D.Raycast (new Vector2(this.transform.position.x + 0.5f, this.transform.position.y + 0.1f), Vector2.up, 0.4f);
		down = Physics2D.Raycast (new Vector2(this.transform.position.x + 0.5f, this.transform.position.y - 1.1f), Vector2.down, 0.4f);

		Debug.DrawRay (new Vector2 (this.transform.position.x, this.transform.position.y - 0.5f), Vector2.left, Color.red);
		Debug.DrawRay (new Vector2(this.transform.position.x + 1, this.transform.position.y - 0.5f), Vector2.right, Color.blue);
		Debug.DrawRay (new Vector2(this.transform.position.x + 0.5f, this.transform.position.y), Vector2.up, Color.green);
		Debug.DrawRay (new Vector2(this.transform.position.x + 0.5f, this.transform.position.y - 1.1f), Vector2.down, Color.yellow);

		contact = 0;
		contactIndirect = 0;
		contactDirect = 0;
		GameObject obj;

        /*Le système compte le nombre de voisin et pour un voisin vérifie si celui ci a lui aussi des voisins jusqu'a remonter au bloc en contact avec le sol,
        Le bloc en contact avec le sol est le bloc porteur, le premier voisin du bloc porteur est appelée "joint" et est transmis a tout les autre bloc de la structure.
        Le joueur detecte en permanence le bloc porteur et si le bloc porteur disparait alors la structure subit la gravité*/

		if (down.transform != null) {
			obj = down.transform.gameObject;
			if (obj.layer == BlockingLayer && obj.gameObject != this.gameObject) {
				if (obj.tag != "Bloc") {
					contactDirect++;
				} else {
					Bloc bloc = obj.GetComponent<Bloc> ();
					if (bloc.contactDirect > 0) {
						contactIndirect = bloc.contactDirect;
						if (joint == null)
						joint = this;
					}
					if (bloc.contactIndirect > 0) {
						contactIndirect = bloc.joint.contactIndirect;
						if (joint == null)
						joint = bloc;

					}
				}
			}
		}
		if (up.transform != null){
			obj = up.transform.gameObject;
			if (obj.layer == BlockingLayer && obj.gameObject != this.gameObject) {
				if (obj.tag != "Bloc") {
					contactDirect++;
				} else {
					Bloc bloc = obj.GetComponent<Bloc> ();
					if (bloc.contactDirect > 0) {
						contactIndirect = bloc.contactDirect;
						if (joint == null)
						joint = this;
					}
					if (bloc.contactIndirect > 0) {
						contactIndirect = bloc.joint.contactIndirect;
						if (joint == null)
						joint = bloc;
					}
				}
			}
		}

		if (left.transform != null){
			obj = left.transform.gameObject;
			if (obj.layer == BlockingLayer && obj.gameObject != this.gameObject) {
				if (obj.tag != "Bloc") {
					contactDirect++;
				} else {
					Bloc bloc = obj.GetComponent<Bloc> ();
					if (bloc.contactDirect > 0) {
						contactIndirect = bloc.contactDirect;
						if (joint == null)
						joint = this;
					}
					if (bloc.contactIndirect > 0) {
						contactIndirect = bloc.joint.contactIndirect;
						if (joint == null)
						joint = bloc;
					}
				}
			}
		}
		if (right.transform != null) {
			obj = right.transform.gameObject;
			if (obj.layer == BlockingLayer && obj.gameObject != this.gameObject) {
				if (obj.tag != "Bloc") {
					contactDirect++;
				} else {
					Bloc bloc = obj.GetComponent<Bloc> ();
					if (bloc.contactDirect > 0) {
						contactIndirect = bloc.contactDirect;
						if (joint == null)
						joint = this;
					}
					if (bloc.contactIndirect > 0) {
						contactIndirect = bloc.joint.contactIndirect;
						if (joint == null)
						joint = bloc;
					}
				}
			}
		}
		contact = contactDirect + contactIndirect;
		if (contact == 0) {
			this.transform.position = new Vector2 (this.transform.position.x, this.transform.position.y - 1 * Time.deltaTime * 2f);
		} else {
			this.transform.position = new Vector2 (Mathf.RoundToInt (this.transform.position.x), Mathf.RoundToInt (this.transform.position.y));
		}

        //Si la vie est inférieure ou égale à 0 le bloc se détruit
		if (vie <= 0) {
			GameController.removeBloc (this.gameObject);
			Destroy (this.gameObject);
		}
	}

    //FOnction pour passer de la Niege à la glace
	public void Upgrade(){
		if (bloc == "Neige") {
			vie = vie + 100;
			bloc = "Glace";
			aspect.sprite = SpriteGlace;
		}

	}

    //Fonction qui detect une collision avec une boule de neige et retire de la vie
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "SnowBall") {
			vie = vie - 10;
		}
	}
}
