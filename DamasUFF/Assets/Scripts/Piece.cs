using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour
{
	public ControlGame controlGame;


	public int id;
	public bool isQueen = false;

	public Color color;

	public int line;
	public int column;


	public Material highlightedMaterial;

	public Material normalMaterial;


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

	public void SetHighlightedPiece(){


		MeshRenderer rend = GetComponent<MeshRenderer> ();        
		rend.material = highlightedMaterial;

		//set LED color
	}

	public void ShutHighlightedPiece(){


		MeshRenderer rend = GetComponent<MeshRenderer> ();        
		rend.material = normalMaterial;

		//set LED color
	}

	public void SetPosition (int l, int c)
	{
		line = l;
		column = c;
		CheckQueen ();
	}

	public void CheckQueen ()
	{

		switch (this.tag) {

		case"WhitePieceTag":
			if (this.line == 7 && controlGame && !controlGame.multipleMovements) {
				controlGame.GrantQueenPiece (this, true);
			}
				
			break;

		case "BlackPieceTag":
			if (this.line == 0 && controlGame && !controlGame.multipleMovements) {
				controlGame.GrantQueenPiece (this, false);
			}
			break;

		}

	}

	private void MouseClick ()
	{
		//Uses raycast to define which piece the mouse is clicking
		if (Input.GetMouseButtonDown (0)) {


			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (controlGame.currentPlayerTurn is Person) {

				if (Physics.Raycast (ray, out hit)) {
					if (hit.collider.gameObject.GetComponent<Piece> () != null
					    && hit.collider.gameObject.GetComponent<Piece> () == this) {


						if (controlGame.maxPlayForPlayer.Contains (this)) {

						
							//controlGame gets the reference of last piece clicked by mouse
							if (!controlGame.alreadyMoved) {
								if ((controlGame.currentPlayerTurn.color == Color.white && (this.CompareTag ("WhitePieceTag") || this.CompareTag ("WhiteQueenTag")))
								   || (controlGame.currentPlayerTurn.color == Color.black && (this.CompareTag ("BlackPieceTag") || this.CompareTag ("BlackQueenTag")))) {


									controlGame.selectedPiece = this;
									controlGame.qntOfMovementsLeft = 0;
									controlGame.TurnOffAllHouses ();
									Verifier.VerifyPlayByPiece (this.line, this.column, controlGame.piecesArray, controlGame);

									//	Debug.Log ("Piece line: "+this.line+"  Piece Column: " + this.column);

								}
					

								//TESTING MOVEMENT
								//	controlGame.EfectuatePlay (controlGame.selectedPiece,new House() );      
							} else {
								//controlGame.selectedPiece = null;
				
							}
						}
					}
				}


			}
		}
	}
}
