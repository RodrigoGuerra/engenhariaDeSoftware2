using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class ControlGame : MonoBehaviour
{
	private bool testmode;

	//Verifier class


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
	private bool alreadyMoved;

	//Verifier
	public int qntOfMovementsLeft;
	public bool multipleMovements = false;
	public Piece multipleMovementsPiece;


	public List<int[]> listOfMovements;
	// Use this for initialization
	void Start ()
	{
		

		houses = new List<House> ();
		houses = board.GetComponentsInChildren<House> ().ToList ();
		movementsToGo = new Queue<MovementAction> ();
		listOfMovements = new List<int[]> ();
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
		//Checks if game is updating position of a piece
		if (isMoving) {
			
			float step = (movementSpeed * 0.01f) * Time.deltaTime;

			Vector3 positionToGo = new Vector3 (HouseToGo.transform.position.x, 0.15f, HouseToGo.transform.position.z);
			pieceToMove.transform.position = Vector3.MoveTowards (pieceToMove.transform.position, positionToGo, step);

			//Piece arrived at desired position
			if (pieceToMove.transform.position == positionToGo) {

				isMoving = false;		
				//Debug.Log ("ismoving =false, chegou na posicao");
				//	piecesArray [pieceToMove.line, pieceToMove.column] = 0;

				//	pieceToMove.column = HouseToGo.column;
				//	pieceToMove.line = HouseToGo.line;

				pieceToMove.SetPosition (HouseToGo.line, HouseToGo.column);

				//Update Array to AI and Verifier
				GeneratePiecesArray ();

				//Check if there is no movement left on movement queue
				if (movementsToGo.Count == 0) {


					int p = PositionHouseOnList (pieceToMove.line, pieceToMove.column);
					if (qntOfMovementsLeft == 1) {
						
						for (int i = 0; i < listOfMovements.Count (); i++) {
							int[] v = listOfMovements [i];
							housesArray [v [0], v [1]].isEnabledToMove = false;
						}

						for (int i = p + 1; i < listOfMovements.Count (); i++) {
							int[] v = listOfMovements [i];
							housesArray [v [0], v [1]].isEnabledToMove = true;
						}
					} else {
						if (qntOfMovementsLeft > 1) {

							for (int i = 0; i < listOfMovements.Count (); i++) {
								int[] v = listOfMovements [i];
								housesArray [v [0], v [1]].isEnabledToMove = false;
							}

							int[] vetor = listOfMovements [p + 1];
							housesArray [vetor [0], vetor [1]].isEnabledToMove = true;
						}
					}								

					//if player has more than 1 movement left, decrease 1 unit
					//	if (multipleMovements)
					//		qntOfMovementsLeft--;

					CheckMultipleMovements ();


					//check if play has multiple captures (if does, cant change turn until that amount of captures has been done)


					Debug.Log ("Change turn.");

					//if there is no movements left to do, change turn
					ChangePlayersTurn ();

					TurnOffAllHouses ();
				} 

				PrintPiecesArray ();

			}
		
		} else {//if not, then check if movements queue has any movements to do


			if (movementsToGo != null && movementsToGo.Count != 0) {

				MovementAction m = movementsToGo.Dequeue ();
				EfectuatePlay (m.piece, m.houseToGo);
			}  
		}
	}

	private int PositionHouseOnList (int line, int column)
	{
		int positionOnlist = 0;
		foreach (int[] v in listOfMovements) {

			if (v [0] == line && v [1] == column) {
				return positionOnlist;
			}

			positionOnlist++;
		}
		return -1;

	}

	public void CheckMultipleMovements ()
	{
	
		if (multipleMovements) {
			
	
			if (qntOfMovementsLeft <= 0) {

				multipleMovements = false;
			} else {
				if (pieceToMove.CompareTag ("WhitePieceTag") || pieceToMove.CompareTag ("BlackPieceTag"))
					Verifier.VerifyPlayByPiece (pieceToMove.line, pieceToMove.column, piecesArray, this);
			}
		}
	}

	public void TurnOffAllHouses ()
	{
		if (!multipleMovements) {
			foreach (House h in houses) {
				h.TurnOffLEDHouse ();
			}
		}
	}

	public void PrintPiecesArray ()
	{

		string t = "";

		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				t += piecesArray [i, j] + " ";
			}
			t += " \n";
		}


		Debug.Log (t);
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
			player1.id = 1;
			player2.id = 2;
			break;

		case GameTypeEnum.PlayerVsAI:
			player1 = new AI ();
			player2 = new Person ();
			player1.id = 1;
			player2.id = 2;
			break;

		case GameTypeEnum.PlayerVsPlayer:
			player1 = new Person ();
			player2 = new Person ();
			player1.id = 1;
			player2.id = 2;
			break;

		default:
			Debug.Log ("Nenhum gametype selecionado.");
			break;
		}
	
		ChooseTeamsRandomly ();

		Player first = player1.color == Color.white ? player1 : player2;

		SetFirstPlayerTurn (first);
	}

	private void SetFirstPlayerTurn (Player first)
	{
		
		currentPlayerTurn = first;
	}


	public void ChangePlayersTurn ()
	{
		bool endgame = VerifyStatus ();

		if (endgame)
			EndGame ();

		if (!multipleMovements) {
			if (currentPlayerTurn.id == player1.id)
				currentPlayerTurn = player2;
			else
				currentPlayerTurn = player1;
		}

		alreadyMoved = false;

		/*	if (currentPlayerTurn.color == Color.black)
			VerifyForEachPiece (teamBlackPieces);
		else
			VerifyForEachPiece(teamWhitePieces);
*/
	}

	private void VerifyForEachPiece (List<Piece> team)
	{
		foreach (Piece p in team)
			Verifier.VerifyPlayByPiece (p.line, p.column, piecesArray, this);
	}

	private void EndGame ()
	{
		//Stop and show scores
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
		//clear piecesArray
		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				piecesArray [i, j] = 0;
			}
		}
		
		foreach (Piece p in teamWhitePieces) {
			if (p.CompareTag ("WhitePieceTag"))
				piecesArray [p.line, p.column] = (int)PieceTypeEnum.White;
			else if (p.CompareTag ("WhiteQueenTag"))
				piecesArray [p.line, p.column] = (int)PieceTypeEnum.KingWhite;
		}

		foreach (Piece p in teamBlackPieces) {
			if (p.CompareTag ("BlackPieceTag"))
				piecesArray [p.line, p.column] = (int)PieceTypeEnum.Black;
			else if (p.CompareTag ("BlackQueenTag"))
				piecesArray [p.line, p.column] = (int)PieceTypeEnum.KingBlack;
		}


	}


	//Do not call this method directly if you need to make one movement, call EfectuateListofPlay and pass a list with one movement
	private void EfectuatePlay (Piece piece, House houseToGo)
	{
		if (!isMoving) {		

			if (((houseToGo.line + piece.line) % 2) == 0 && ((houseToGo.column + piece.column) % 2) == 0) {
				int l = (houseToGo.line + piece.line) / 2;
				int c = (houseToGo.column + piece.column) / 2;
				//	piecesArray [l, c] = 0;


				SearchToDestroy (teamBlackPieces, l, c);
				SearchToDestroy (teamWhitePieces, l, c);
			}

			HouseToGo = houseToGo;
			pieceToMove = piece;


			isMoving = true;
			//	Debug.Log ("ismoving =true, iniciou movimento");
		}


	}

	private void SearchToDestroy (List<Piece> team, int line, int column)
	{

		Piece pieceToRemove = null;

		foreach (Piece p in team) {
			if (p.line == line && p.column == column) {

				pieceToRemove = p;
			}

		}

		if (pieceToRemove != null) {
			team.Remove (pieceToRemove);
			Destroy (pieceToRemove.gameObject);
			if (qntOfMovementsLeft > 0)
				qntOfMovementsLeft--;
		}

		GeneratePiecesArray ();
	
	}

	public void EfectuateListOfPlays (List<MovementAction> listOfPlays)
	{
		if (!alreadyMoved) {
			
			for (int i = 0; i < listOfPlays.Count; i++) {
				movementsToGo.Enqueue (listOfPlays.ElementAt (i));
			}
		
			alreadyMoved = true;
		}
	}

	//return true if game ended
	public bool VerifyStatus ()
	{
		if (teamBlackPieces.Count () > 0 || teamWhitePieces.Count () > 0)
			return false;

		return true;

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
				
					GameObject p = (GameObject)Instantiate (Resources.Load ("Prefabs/WhiteKingChecker_T"));
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

	public void GrantQueenPiece (Piece p, bool white)
	{
		if (white) {
			GameObject whites = GameObject.Find ("Whites");

			GameObject q = (GameObject)Instantiate (Resources.Load ("Prefabs/WhiteKingChecker_T"));
			q.transform.SetParent (whites.transform);
			q.transform.position = new Vector3 (p.gameObject.transform.position.x, p.gameObject.transform.position.y, p.gameObject.transform.position.z);
			Piece piece = q.GetComponent<Piece> ();
			piece.column = p.column;
			piece.line = p.line;
			piece.isQueen = true;
			SearchToDestroy (teamWhitePieces, p.line, p.column);

			teamWhitePieces.Add (piece);



		} else {

			GameObject blacks = GameObject.Find ("Blacks");

			GameObject q = (GameObject)Instantiate (Resources.Load ("Prefabs/BlackKingChecker_T"));
			q.transform.SetParent (blacks.transform);
			q.transform.position = new Vector3 (p.gameObject.transform.position.x, p.gameObject.transform.position.y, p.gameObject.transform.position.z);
			Piece piece = q.GetComponent<Piece> ();
			piece.column = p.column;
			piece.line = p.line;
			piece.isQueen = true;
			SearchToDestroy (teamBlackPieces, p.line, p.column);

			teamBlackPieces.Add (piece);

		}

		GeneratePiecesArray ();

	}

}
