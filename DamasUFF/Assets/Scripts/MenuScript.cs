using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
	public Dropdown drop;

	public DifficultyEnum difficulty;
	public GameTypeEnum gameType;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void StartGame (Slider type)
	{
		DontDestroyOnLoad (GameObject.Find ("Parameters"));

		int temp = (int)type.value;
		switch (temp) {
		case 0:
			gameType = GameTypeEnum.PlayerVsAI;
			break;
		case 1:
			gameType = GameTypeEnum.PlayerVsPlayer;
			break;
		case 2:
			gameType = GameTypeEnum.AIvsAI;
			break;
		default:
			Debug.Log ("Erro de load");
			break;
		}

		int dif = drop.value;
		switch (dif) {
		case 0:
			difficulty = DifficultyEnum.Easy;
			break;
		case 1:
			difficulty = DifficultyEnum.Normal;
			break;
		case 2:
			difficulty = DifficultyEnum.Hard;
			break;
		default:
			Debug.Log ("Erro de Dificuldade");
			break;
		}


		Application.LoadLevel ("MainGame");
	}

}

