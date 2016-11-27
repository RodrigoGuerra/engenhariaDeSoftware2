using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinMax : MonoBehaviour {
	[ReadOnly]private int LEVEL = 0;
	[ReadOnly]private int MAX_LEVEL;

	[ReadOnly]private int[,] TAB;

	//Type é verdadeiro para 1 ou 3 e falso para 2 ou 4
	[ReadOnly]private bool TYPE;

	// Use this for initialization
	void Start () {
	
	}

	public MinMax(int[,] tab, bool type, int maxLevel) {
		this.TAB = tab;
		this.TYPE = type;
		this.MAX_LEVEL = maxLevel;
	}

	public Play Search() {
		Play play = new Play(TAB, TYPE);
		return MaxValue(play, LEVEL, int.MinValue, int.MaxValue);
	}

	private Play MaxValue(Play state, int level, int alpha, int beta) {

		//Altura máxima da árvore        
		if (level < MAX_LEVEL) {

			Play v = new Play(int.MinValue, TYPE);

			//Percorre o tabuleiro inteiro
			for (int line = 0; line < state.getTAB().Length; line++) {
				for (int column = 0; column < state.getTAB().GetLength(1); column++) {

					//Se a peça for do tipo atual
					if ((((state.getTAB()[line,column] == (int)PieceTypeEnum.White) && TYPE) || ((state.getTAB()[line,column] == (int)PieceTypeEnum.Black) && !TYPE))
						|| (((state.getTAB()[line,column] == (int)PieceTypeEnum.KingWhite) && TYPE) || ((state.getTAB()[line,column] == (int)PieceTypeEnum.KingBlack) && !TYPE))) {

						//Gera a lista de jogadas para tal posição
						int pieceType = state.getTAB()[line,column];
						List<List<int[]>> list = Verifier.VerifyPlayByPiece(line, column, state.getTAB());
						state.getTAB()[line,column] = pieceType;

						//Posição atual
						int[] pos = {line, column};

						//Qtd máxima de peças que podem ser comidas
						int maxCount = Verifier.MaxCount(list, pos);                        

						for (int i = 0; i < list.Count; i++) {

							//Se a jogada for a máxima
							if (Verifier.Count(list[i], pos) == maxCount) {

								//Atualiza o tabuleiro
								int[,] child = Verifier.UpdateTab(list[i], state.getTAB(), pos, state.getTAB()[line,column]);
								Play play = new Play(child, TYPE);
								if(level == 0){
									play.setNextTab(child);
									play.setPlay(list[i]);
									play.setPos(pos);
								}else{
									play.setNextTab(state.getNextTab());
									play.setPlay(state.getPlay());
									play.setPos(state.getPos());
								}

								v = Utils.max(v, min_value(play, level + 1, alpha, beta));

								if(v.getCount() > beta){
									return v;
								}

								alpha = Mathf.Max(v.getCount(), alpha);
							}
						}                        
					}
				}                
			}
			return v;
		}
		Play p = new Play(state.getTAB(), state.isTYPE());
		p.setNextTab(state.getNextTab());
		p.setPlay(state.getPlay());
		p.setPos(state.getPos());
		return p;
	}

	private Play min_value(Play state, int level, int alpha, int beta) {

		//Altura máxima da árvore        
		if (level < MAX_LEVEL) {

			Play v = new Play(int.MaxValue, !TYPE);

			//Percorre o tabuleiro inteiro
			for (int line = 0; line < state.getTAB().Length; line++) {
				for (int column = 0; column < state.getTAB().GetLength(1); column++) {

					//Se a peça for do tipo atual
					if ((((state.getTAB()[line,column] == (int)PieceTypeEnum.White) && !TYPE) || ((state.getTAB()[line,column] == (int)PieceTypeEnum.Black) && TYPE))
						|| (((state.getTAB()[line,column] == (int)PieceTypeEnum.KingWhite) && !TYPE) || ((state.getTAB()[line,column] == (int)PieceTypeEnum.KingBlack) && TYPE))) {

						//Gera a lista de jogadas para tal posição
						int pieceType = state.getTAB()[line,column];
						List<List<int[]>> list = Verifier.VerifyPlayByPiece(line, column, state.getTAB());
						state.getTAB()[line,column] = pieceType;

						//Posição atual
						int[] pos = {line, column};

						//Qtd máxima de peças que podem ser comidas
						int maxCount = Verifier.MaxCount(list, pos);                        

						for (int i = 0; i < list.Count; i++) {

							//Se a jogada for a máxima
							if (Verifier.Count(list[i], pos) == maxCount) {

								//Atualiza o tabuleiro
								int[,] child = Verifier.UpdateTab(list[i], state.getTAB(), pos, state.getTAB()[line,column]);
								Play play = new Play(child, TYPE);                                
								play.setNextTab(state.getNextTab());
								play.setPlay(state.getPlay());
								play.setPos(state.getPos());

								v = Utils.min(v, MaxValue(play, level + 1, alpha, beta));

								if(v.getCount() <= alpha){
									return v;
								}

								beta = Mathf.Min(v.getCount(), beta);
							}
						}                        
					}
				}                
			}
			return v;
		}
		Play p = new Play(state.getTAB(), state.isTYPE());
		p.setNextTab(state.getNextTab());
		p.setPlay(state.getPlay());
		p.setPos(state.getPos());
		return p;
	}    

}
