using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Play {
	[ReadOnly]private int[,] TAB;            
	private int[,] nextTab; 
	//Type é verdadeiro para 1 ou 3 e falso para 2 ou 4
	[ReadOnly]private bool TYPE;
	[ReadOnly]private int COUNT;

	private List<int[]> play = new List<int[]>();
	private int[] pos;

	// Use this for initialization
	void Start () {
	
	}

	public Play(int COUNT, bool TYPE) {
		this.TAB = null;
		this.TYPE = true;
		this.COUNT = COUNT;
	}

	public Play(int[,] TAB, bool TYPE) {
		this.TAB = TAB;
		this.TYPE = TYPE;

		int number = 0;
		for (int i = 0; i < TAB.GetLength(0); i++) {
			for (int j = 0; j < TAB.GetLength(1); j++) {
				//Se a peça for do tipo atual
				if (((TAB[i,j] == (int)PieceTypeEnum.White) && TYPE) || ((TAB[i,j] == (int)PieceTypeEnum.Black) && !TYPE)) {
					number++;
				} else if (((TAB[i,j] == (int)PieceTypeEnum.KingWhite) && TYPE) || ((TAB[i,j] == (int)PieceTypeEnum.KingBlack) && !TYPE)) {
					number+=3;
				} else if (((TAB[i,j] == (int)PieceTypeEnum.White) && !TYPE) || ((TAB[i,j] == (int)PieceTypeEnum.Black) && TYPE)) {
					number--;
				} else if (((TAB[i,j] == (int)PieceTypeEnum.KingWhite) && !TYPE) || ((TAB[i,j] == (int)PieceTypeEnum.KingBlack) && TYPE)) {
					number-=3;
				}
			}
		}
		//System.out.println("\n\n");
		//imprime(TAB);
		//System.out.println("COUNT: " + number + " - TYPE: " + TYPE);
		this.COUNT = number; 
	}

	public void setPlay(List<int[]> play) {
		this.play = play;
	}

	public void setNextTab(int[,] nextTab) {
		this.nextTab = nextTab;
	}

	public int[,] getNextTab() {
		return nextTab;
	}

	public bool isTYPE() {
		return TYPE;
	}

	public void setPos(int[] pos) {
		this.pos = pos;
	}

	public int[,] getTAB() {
		return TAB;
	}

	public int getCount() {        
		return COUNT;        
	}       

	public List<int[]> getPlay() {
		return play;
	}

	public int[] getPos() {
		return pos;
	} 
}
