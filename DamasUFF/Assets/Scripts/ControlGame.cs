using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System;

public class ControlGame : MonoBehaviour
{
	private bool gameStillValid = true;

	//UI
	public Text info;
	public Text contador;
	public Text turn;
	public Text piecesInfo;

	private int difficultyLevel = 0;


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
	public Piece pieceToMove;
	public bool alreadyMoved = false;

	//Verifier
	public int qntOfMovementsLeft;
	public bool multipleMovements = false;
	public Piece multipleMovementsPiece;
	public List<Piece> maxPlayForPlayer = new List<Piece> ();


	private bool _AIHasPlayed;
	public List<int[]> listOfMovements;


	public int countPlays = 0;
	private bool drawScenario = false;
	// Use this for initialization
	void Start ()
	{
		MenuScript menu = GameObject.Find ("Parameters").GetComponent<MenuScript> ();

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
			
		StartGame (menu.gameType, menu.difficulty);
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (currentPlayerTurn is Person) {
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
								housesArray [v [0], v [1]].SetIsEnabledToMove (false);
							}

							for (int i = p + 1; i < listOfMovements.Count (); i++) {
								int[] v = listOfMovements [i];
								housesArray [v [0], v [1]].SetIsEnabledToMove (true);
							}

						} else {
							if (qntOfMovementsLeft > 1) {

								for (int i = 0; i < listOfMovements.Count (); i++) {
									int[] v = listOfMovements [i];
									housesArray [v [0], v [1]].SetIsEnabledToMove (false);
								}

								int[] vetor = listOfMovements [p + 1];
								housesArray [vetor [0], vetor [1]].SetIsEnabledToMove (true);
							}
						}								

						//if player has more than 1 movement left, decrease 1 unit
						//	if (multipleMovements)
						//		qntOfMovementsLeft--;

						CheckMultipleMovements ();

						pieceToMove.CheckQueen ();
						//check if play has multiple captures (if does, cant change turn until that amount of captures has been done)


					
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

		} else {

			if (!AIHasPlayed ()) {
		
				_AIHasPlayed = true;
				
				bool white = currentPlayerTurn.color == Color.white ? true : false;

				MinMax mm = new MinMax (piecesArray, white, difficultyLevel);

				Play p = mm.Search ();
				int[] pos = p.getPos ();

				//pieceToMove = piecesArray [pos [0], pos [1]];
				if (pos != null) {
			
					pieceToMove = GetPieceAtPosition (pos [0], pos [1]);

					listOfMovements = p.getPlay ();


					List<MovementAction> ma = new List<MovementAction> ();
					while (listOfMovements.Count () > 0) {

						MovementAction m = new MovementAction ();

						int[] hToGoVector = listOfMovements.ElementAt (0);
						listOfMovements.RemoveAt (0);

						m.houseToGo = housesArray [hToGoVector [0], hToGoVector [1]];
						m.piece = pieceToMove;
						ma.Add (m);
					}
					if (ma.Count () > 1) {
						Debug.Log ("break");
					}
					EfectuateListOfPlays (ma);

				} else {
					Debug.Log ("Acabaram jogadas");
					VerifyStatus ();
				}

			} else {

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
							


							//check if play has multiple captures (if does, cant change turn until that amount of captures has been done)
							CheckMultipleMovements ();
							pieceToMove.CheckQueen ();

							//if there is no movements left to do, change turn
							ChangePlayersTurn ();
						}
					}
				} else if (movementsToGo != null && movementsToGo.Count != 0) {

					MovementAction m = movementsToGo.Dequeue ();
					EfectuatePlay (m.piece, m.houseToGo);
				} 

			}
		}
	}

	private Piece GetPieceAtPosition (int line, int column)
	{

		foreach (Piece p in teamWhitePieces) {

			if (p.line == line && p.column == column)
				return p;

		}

		foreach (Piece p in teamBlackPieces) {

			if (p.line == line && p.column == column)
				return p;
			

		}

		return null;
	}

	private bool AIHasPlayed ()
	{

		return _AIHasPlayed;
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
		if (!alreadyMoved) {
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
			player2 = new AI ();
			player1.id = 1;
			player2.id = 2;
			break;

		case GameTypeEnum.PlayerVsAI:
			player1 = new Person ();
			player2 = new AI ();
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

		switch (difficulty) {

		case DifficultyEnum.Easy:
			difficultyLevel = 1;
			break;

		case DifficultyEnum.Normal:
			difficultyLevel = 4;
			break;

		case DifficultyEnum.Hard:
			difficultyLevel = 7;
			break;

		default:
			Debug.Log ("Nenhum gametype selecionado.");
			break;
		}
	
		ChooseTeamsRandomly ();

		Player first = player1.color == Color.white ? player1 : player2;

		SetText (contador, "");
		SetFirstPlayerTurn (first);
		SetPiecesInfo ();
	}

	private void SetFirstPlayerTurn (Player first)
	{
		
		currentPlayerTurn = first;
		SetText (turn, "Turn: Whites");

		GetMaxPlay (currentPlayerTurn.color);
	}


	public void ChangePlayersTurn ()
	{
		if (gameStillValid) {
			Status gameStatus = VerifyStatus ();

			switch (gameStatus) {
			case Status.Continue:
				break;
			case Status.Draw:
				Draw ();
				return;

			case Status.WinBlack:
				Win (Color.black);
				return;
			
			case Status.WinWhite:
				Win (Color.white);
				return;
			

			}
	

			if (!multipleMovements) {
				Debug.Log ("Change turn.");


				if (currentPlayerTurn.id == player1.id) {
					currentPlayerTurn = player2;
					if (player1 is AI && player2 is AI) {
						SetText (info, "AI vs AI mode");

					} else {
						if (player2 is Person) {
							SetText (info, "Player 2, make a move.");
						} else {
							SetText (info, "AI is thinking...");
						}
					}

				} else {
					currentPlayerTurn = player1;
					if (player1 is AI && player2 is AI) {
						SetText (info, "AI vs AI mode");

					} else {
						if (player2 is AI) {
							SetText (info, "Your turn.");
						} else {
							SetText (info, "Player 1, make a move.");
						}
					}
				}

				if (currentPlayerTurn.color == Color.black)
					SetText (turn, "Turn: Blacks");
				else
					SetText (turn, "Turn: Whites");
			
				_AIHasPlayed = false;
				alreadyMoved = false;

				if (drawScenario) {
					countPlays++;

					SetText (contador, "Moves left: " + Math.Abs (countPlays - 5).ToString ());
				}

				foreach (Piece pp in maxPlayForPlayer) {
					pp.ShutHighlightedPiece ();
				}
				maxPlayForPlayer.Clear ();

				if (currentPlayerTurn is Person) {
				
					//max =  GetMaxPlay ();
					GetMaxPlay (currentPlayerTurn.color);
				}
			}



			/*	if (currentPlayerTurn.color == Color.black)
			VerifyForEachPiece (teamBlackPieces);
		else
			VerifyForEachPiece(teamWhitePieces);
*/
		}
	}

	private void GetMaxPlay (Color c)
	{

		int max = 0;

		if (c == Color.black) {		

			foreach (Piece p in teamBlackPieces) {
				int[] v = new int[2];
				v [0] = p.line;
				v [1] = p.column;

				int cont = Verifier.MaxCount (Verifier.VerifyPlayByPiece (p.line, p.column, piecesArray, null), v);
				if (cont >= max) {
					
					if (cont > max) {
						max = cont;
				
						maxPlayForPlayer.Clear ();
					}

					maxPlayForPlayer.Add (p);

				}
			}

			if (maxPlayForPlayer.Count < teamBlackPieces.Count) {
				foreach (Piece pp in maxPlayForPlayer) {
					pp.SetHighlightedPiece ();
				}
			}

		} else {
			foreach (Piece p in teamWhitePieces) {
				int[] v = new int[2];
				v [0] = p.line;
				v [1] = p.column;

				int cont = Verifier.MaxCount (Verifier.VerifyPlayByPiece (p.line, p.column, piecesArray, null), v);
				if (cont >= max) {

					if (cont > max) {
						max = cont;


					
						maxPlayForPlayer.Clear ();
					}

					maxPlayForPlayer.Add (p);

				}
			}

			if (maxPlayForPlayer.Count < teamWhitePieces.Count) {
				foreach (Piece pp in maxPlayForPlayer) {
					pp.SetHighlightedPiece ();
				}
			}
		}


	}

	private void VerifyForEachPiece (List<Piece> team)
	{
		foreach (Piece p in team)
			Verifier.VerifyPlayByPiece (p.line, p.column, piecesArray, this);
	}

	private void Draw ()
	{
		gameStillValid = false;
		SetText (info, "It was a draw!");
		//display draw hud
	}

	private void Win (Color winColor)
	{
		//Stop and show scores
		gameStillValid = false;

		if (winColor == Color.white) {
			SetWinTextWhite ();
		} else {
			SetWinTextBlack ();
		}
	}

	private void ChooseTeamsRandomly ()
	{
		float r = UnityEngine.Random.Range (0, 1f);
		if (r < 0.5) {
			player1.color = Color.white;
			player2.color = Color.black;

			if (player1 is Person && player2 is AI) {
				SetText (info, "You got the whites!");
			} else if (player2 is Person) {
				SetText (info, "Player 1 got whites!");
			}
		} else {
			player1.color = Color.black;
			player2.color = Color.white;
			if (player1 is Person && player2 is AI) {
				SetText (info, "You got the blacks!");
			} else if (player2 is Person) {
				SetText (info, "Player 2 got whites!");
			}
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

			/*		if (((houseToGo.line + piece.line) % 2) == 0 && ((houseToGo.column + piece.column) % 2) == 0) {
				int l = (houseToGo.line + piece.line) / 2;
				int c = (houseToGo.column + piece.column) / 2;
				//	piecesArray [l, c] = 0;


				SearchToDestroy (teamBlackPieces, l, c);
				SearchToDestroy (teamWhitePieces, l, c);
			}
*/
			CheckIfDestroy (piece.line, piece.column, houseToGo.line, houseToGo.column);

			HouseToGo = houseToGo;
			pieceToMove = piece;


			isMoving = true;
			//	Debug.Log ("ismoving =true, iniciou movimento");
		}


	}

	private void CheckIfDestroy (int linhaI, int colunaI, int linhaF, int colunaF)
	{
		
		//posicaoInicial como (xi,yi)
		//posicaoFinal como (xf,yf)

		//horizontal = null;
		//vertical = null;
		int qtdCasasAndadas = 0;
		bool direita;
		bool cima;
		qtdCasasAndadas = Mathf.Abs (linhaI - linhaF);

		if ((linhaI - linhaF) > 0) {
			direita = true;
		} else {
			direita = false;
		}

		if ((colunaI - colunaF) > 0) {
			cima = false;
		} else {
			cima = true;
		}
 
		if (direita && cima) { 
			for (int i = 0; i < qtdCasasAndadas; i++) {
				if (piecesArray [linhaF + i, colunaF - i] != 0) {
					SearchToDestroy (teamBlackPieces, linhaF + i, colunaF - i);
					SearchToDestroy (teamWhitePieces, linhaF + i, colunaF - i);
				}
			}

			//	i = 1 ate qtdCasasAndadas-1 faça
			//		se temPeça(xf+i,yf-i)
			//		DestroiPeça 
		}
		if (direita && !cima) { 

			for (int i = 0; i < qtdCasasAndadas; i++) {
				if (piecesArray [linhaF + i, colunaF + i] != 0) {
					SearchToDestroy (teamBlackPieces, linhaF + i, colunaF + i);
					SearchToDestroy (teamWhitePieces, linhaF + i, colunaF + i);
				}
			}

			//	para i = 1 ate qtdCasasAndadas-1 faça
			//		se temPeça(xf+i,yf+i)
			//		DestroiPeça
		}
		if (!direita && cima) {

			for (int i = 0; i < qtdCasasAndadas; i++) {
				if (piecesArray [linhaF - i, colunaF - i] != 0) {
					SearchToDestroy (teamBlackPieces, linhaF - i, colunaF - i);
					SearchToDestroy (teamWhitePieces, linhaF - i, colunaF - i);
				}
			}

		}

		//então 
		///	para i = 1 ate qtdCasasAndadas-1 faça
		//	se temPeça(xf-i,yf-i)
		//		DestroiPeça

		if (!direita && !cima) { 
			
			for (int i = 0; i < qtdCasasAndadas; i++) {
				if (piecesArray [linhaF - i, colunaF + i] != 0) {
					SearchToDestroy (teamBlackPieces, linhaF - i, colunaF + i);
					SearchToDestroy (teamWhitePieces, linhaF - i, colunaF + i);
				}
			}
		}
		//	para i = 1 ate qtdCasasAndadas-1 faça
		//	se temPeça(xf-i,yf+i)
		//			DestroiPeça
		
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
			piecesArray [pieceToRemove.line, pieceToRemove.column] = 0;
				
			Destroy (pieceToRemove.gameObject);
			if (qntOfMovementsLeft > 0)
				qntOfMovementsLeft--;
		}

		GeneratePiecesArray ();
	
	}

	public void EfectuateListOfPlays (List<MovementAction> listOfPlays)
	{
		
			
		for (int i = 0; i < listOfPlays.Count; i++) {
			movementsToGo.Enqueue (listOfPlays.ElementAt (i));
		}
		
		alreadyMoved = true;

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



	public Status VerifyStatus ()
	{
		SetPiecesInfo ();
		Status status = Status.Continue;
		if (teamBlackPieces.Count () > 0 && teamWhitePieces.Count () > 0
		    && teamBlackPieces.Count () <= 2 && teamWhitePieces.Count () <= 2) {

			if (!drawScenario) {
				drawScenario = true;
			}

			int countBlackQueen = 0;
			int countWhiteQueen = 0;
			int i;
			for (i = 0; i < teamBlackPieces.Count (); i++) {
				if (teamBlackPieces.ElementAt (i).tag.Equals ("BlackQueenTag"))
					countBlackQueen++;    
			}
			for (i = 0; i < teamWhitePieces.Count (); i++) {
				if (teamWhitePieces.ElementAt (i).tag.Equals ("WhiteQueenTag"))
					countWhiteQueen++;    
			}
			if (countBlackQueen >= 1 && countWhiteQueen >= 1 && countPlays >= 4) {   
				status = Status.Draw;
			}
		} else {
			if (teamBlackPieces.Count () == 0 && teamWhitePieces.Count () > 0) {
				status = Status.WinWhite;
			}
			if (teamWhitePieces.Count () == 0 && teamBlackPieces.Count () > 0) {
				status = Status.WinBlack;
			}
		}

		return status;

	}

	private void SetText (Text t, string message)
	{
		
		t.text = message;
	}

	private void SetWinTextWhite ()
	{

		info.text = "White pieces win!";
	
	}

	private void SetWinTextBlack ()
	{

		info.text = "Black pieces win!";
	}

	private void SetPiecesInfo ()
	{

		piecesInfo.text = "Whites: " + teamWhitePieces.Count ();
		piecesInfo.text += "\n\nBlacks: " + teamBlackPieces.Count ();
	}

}
