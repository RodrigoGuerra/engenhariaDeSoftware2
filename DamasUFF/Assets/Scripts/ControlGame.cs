using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class ControlGame : MonoBehaviour
{
	//Both Players(AI AND/OR PERSON)
	public Player player1;
	public Player player2;

	//Current player
	public Player currentPlayerTurn;

	//Pieces
	public List<Piece> teamWhitePieces;
	public List<Piece> teamBlackPieces;


	//Array for IA
	public int[,] piecesArray;


	//Piece current selected
	public Piece selectedPiece;

	//board parent
	public GameObject board;

	//Board list and array
	public List<House> houses;
	public House[,] housesArray;

	//Movement-related variables
	public float movementSpeed = 250f;
	public Queue<MovementAction> movementsToGo;
	private bool isMoving;
	private House HouseToGo;
	private Piece pieceToMove;


	// Use this for initialization
	void Start ()
	{
		houses = new List<House> ();
		houses = board.GetComponentsInChildren<House> ().ToList ();
		movementsToGo = new Queue<MovementAction> ();
		housesArray = new House[8, 8];

		piecesArray = new int[8, 8];

		teamBlackPieces = new List<Piece> ();
		teamWhitePieces = new List<Piece> ();

		//clear piecesArray
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				piecesArray [i, j] = 0;
			}
		}

		//Populate HousesArray
		int k = 0;
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				housesArray [i, j] = houses.ElementAt (k);
				k++;
			}
		}
			
		StartGame (GameTypeEnum.PlayerVsAI, DifficultyEnum.Normal);
	}
	
	// Update is called once per frame
	void Update ()
	{
		

		if (isMoving) {
			
			float step = (movementSpeed * 0.01f) * Time.deltaTime;

			Vector3 positionToGo = new Vector3 (HouseToGo.transform.position.x, 0.15f, HouseToGo.transform.position.z);
			pieceToMove.transform.position = Vector3.MoveTowards (pieceToMove.transform.position, positionToGo, step);
			if (pieceToMove.transform.position == positionToGo) {

				isMoving = false;		
				Debug.Log ("ismoving =false, chegou na posicao");
			
							
			} else {
				//after the movement animation update line and column of the piece moved
				pieceToMove.column = HouseToGo.line;
				pieceToMove.line = HouseToGo.column;

				if (movementsToGo != null && movementsToGo.Count != 0) {
					
					MovementAction m = movementsToGo.Dequeue ();
					EfectuatePlay (m.piece, m.houseToGo);
				} else if (movementsToGo.Count == 0) {
					//if there is no movements left to do, change turn
					currentPlayerTurn = currentPlayerTurn == player1 ? player2 : player1;

				}
					
			}
		
		}
	}

	//Use this to start game, after user click play game (already set gametype and difficulty)
	private void StartGame (GameTypeEnum gameType, DifficultyEnum difficulty)
	{
		InstantiateBoard ();

		GeneratePiecesArray ();

		switch (gameType) {

		case GameTypeEnum.AIvsAI:
			player1 = new AI ();
			player2 = new Person ();
			break;

		case GameTypeEnum.PlayerVsAI:
			player1 = new AI ();
			player2 = new Person ();
			break;

		case GameTypeEnum.PlayerVsPlayer:
			player1 = new Person ();
			player2 = new Person ();
			break;

		default:
			Debug.Log ("Nenhum gametype selecionado.");
			break;
		}
	
		ChooseTeamsRandomly ();

		currentPlayerTurn = player1.color == Color.white ? player1 : player2;

	}


	private void ChooseTeamsRandomly ()
	{
		int r = Random.Range (0, 1);
		if (r < 0.5) {
			player1.color = Color.white;
			player2.color = Color.black;
		} else {
			player1.color = Color.black;
			player2.color = Color.white;
		}

	}

	public void GeneratePiecesArray ()
	{
		
		foreach (Piece p in teamWhitePieces) {
			if (p.CompareTag ("WhitePieceTag"))
				piecesArray [p.line, p.column] = 1;
			else if (p.CompareTag ("WhiteQueenTag"))
				piecesArray [p.line, p.column] = 3;
		}

		foreach (Piece p in teamBlackPieces) {
			if (p.CompareTag ("BlackPieceTag"))
				piecesArray [p.line, p.column] = 2;
			else if (p.CompareTag ("BlackQueenTag"))
				piecesArray [p.line, p.column] = 4;
		}

	}


	//Do not call this method directly if you need to make one movement, call EfectuateListofPlay and pass a list with one movement
	public void EfectuatePlay (Piece piece, House houseToGo)
	{
		
		if (!isMoving) {		
	
			GameObject p = piece.transform.parent.gameObject;

			HouseToGo = houseToGo;
			pieceToMove = piece;


			//for TESTING
			//HouseToGo = housesArray [piece.line + 1, piece.column + 1];     

			

			isMoving = true;
			Debug.Log ("ismoving =true, iniciou movimento");
		}


	}

	public void EfectuateListOfPlays (Piece p, List<MovementAction> listOfPlays)
	{
		for (int i = 0; i < listOfPlays.Count; i++) {
			movementsToGo.Enqueue (listOfPlays.ElementAt (i));
		}

	}

	public void VerifyStatus ()
	{


	}

	public void LoadGame ()
	{		

					
	}

	public void SaveGame ()
	{
	}

	public void InstantiateBoard ()
	{
								
		//Set column and line of each House
		int count = 0;
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				House t = houses.ElementAt (count);
				t.column = j;
				t.line = i;

				count++;
			}
		}
		
		//Instantiate Objects to organize teams
		GameObject teams = new GameObject ("Teams");
		GameObject teamWhite = new GameObject ("Whites");
		teamWhite.transform.SetParent (teams.transform);
		GameObject teamBlack = new GameObject ("Blacks");
		teamBlack.transform.SetParent (teams.transform);

		//Instantiate each piece for each team
		int k = 0;
		bool alterna = false;
		for (int i = 0; i < 8; i++) {
			if (i < 3) {
				k = alterna ? 1 : 0;
				while (k < 8) {
				
					GameObject p = (GameObject)Instantiate (Resources.Load ("Prefabs/WhiteMenChecker_T"));
					p.transform.SetParent (teamWhite.transform);
					p.transform.position = new Vector3 ((float)housesArray [i, k].transform.position.x, 0.15f, (float)housesArray [i, k].transform.position.z);
					Piece piece = p.GetComponent<Piece> ();
					piece.column = k;
					piece.line = i;
					teamWhitePieces.Add (piece);

					k += 2;
				}
				alterna = alterna ? false : true;
			} else if (i >= 5) {

				k = alterna ? 1 : 0;
				while (k < 8) {

					GameObject p = (GameObject)Instantiate (Resources.Load ("Prefabs/BlackMenChecker_T"));
					p.transform.SetParent (teamBlack.transform);
					p.transform.position = new Vector3 ((float)housesArray [i, k].transform.position.x, 0.15f, (float)housesArray [i, k].transform.position.z);
					Piece piece = p.GetComponent<Piece> ();
					piece.column = k;
					piece.line = i;
					teamBlackPieces.Add (piece);

					k += 2;
				}
				alterna = alterna ? false : true;

			}
		}
	}

	private void InstantiateHousesAndPieces ()
	{
	}

}
