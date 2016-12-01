using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class House : MonoBehaviour
{
	
	public ControlGame controlGame;

	[ReadOnly]public int line;
	[ReadOnly]public int column;

	public Material highlightedMaterial;

	public bool isHighlighted = false;


	// Use this for initialization
	void Start ()
	{
		controlGame = GameObject.Find ("GameController").GetComponent<ControlGame> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		
		MouseClick ();
	
	}

	public void TurnOnLEDHouse ()
	{
		this.isHighlighted = true;

		MeshRenderer rend = GetComponent<MeshRenderer> ();        
		rend.material = highlightedMaterial;
		//set LED color
	}

	public void TurnOffLEDHouse ()
	{
		this.isHighlighted = false;

		//set LED off
	}

	public void SetPosition (int line, int column)
	{
		this.line = line;
		this.column = column;

	}

	private void MouseClick ()
	{
		//Uses raycast to define which piece the mouse is clicking
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				
				if (hit.collider.gameObject.GetComponent<House> () != null
				   && hit.collider.gameObject.GetComponent<House> () == this) {
					//controlGame gets the reference of last piece clicked by mouse
					if (this.isHighlighted) {
						MovementAction m = new MovementAction ();
						m.houseToGo = this;
						m.piece = controlGame.selectedPiece;

						Debug.Log (m.piece);

						List<MovementAction> list = new List<MovementAction> ();
						list.Add (m);
						controlGame.EfectuateListOfPlays (list);

						TurnOffLEDHouse ();
					}


					//TESTING MOVEMENT
					//	controlGame.EfectuatePlay (controlGame.selectedPiece,new House() );      
				}
			}
		}
	}


	///======================

}
