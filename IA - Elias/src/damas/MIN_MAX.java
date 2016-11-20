/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package damas;

import java.util.ArrayList;
import static damas.Utils.*;

/**
 *
 * @author Elias
 */
public class MIN_MAX {

    private final int LEVEL = 0;
    private final int MAX_LEVEL;

    private final int[][] TAB;

    //Type é verdadeiro para 1 ou 3 e falso para 2 ou 4
    private final boolean TYPE;

    public MIN_MAX(int[][] tab, boolean type, int maxLevel) {
        this.TAB = tab;
        this.TYPE = type;
        this.MAX_LEVEL = maxLevel;
    }

    public Play search() {
        Play play = new Play(TAB, TYPE);
        return max_value(play, LEVEL, Integer.MIN_VALUE, Integer.MAX_VALUE);
    }

    private Play max_value(Play state, int level, int alpha, int beta) {
        
        //Altura máxima da árvore        
        if (level < MAX_LEVEL) {
            
            Play v = new Play(Integer.MIN_VALUE, TYPE);
            
            //Percorre o tabuleiro inteiro
            for (int line = 0; line < state.getTAB().length; line++) {
                for (int column = 0; column < state.getTAB()[0].length; column++) {

                    //Se a peça for do tipo atual
                    if ((((state.getTAB()[line][column] == 1) && TYPE) || ((state.getTAB()[line][column] == 2) && !TYPE))
                            || (((state.getTAB()[line][column] == 3) && TYPE) || ((state.getTAB()[line][column] == 4) && !TYPE))) {

                        //Gera a lista de jogadas para tal posição
                        int pieceType = state.getTAB()[line][column];
                        ArrayList<ArrayList<int[]>> lista = Damas.verificaJogada(line, column, state.getTAB());
                        state.getTAB()[line][column] = pieceType;

                        //Posição atual
                        int[] pos = {line, column};

                        //Qtd máxima de peças que podem ser comidas
                        int maxCount = Damas.maxCount(lista, pos);                        
                        
                        for (int i = 0; i < lista.size(); i++) {

                            //Se a jogada for a máxima
                            if (Damas.count(lista.get(i), pos) == maxCount) {

                                //Atualiza o tabuleiro
                                int[][] child = Damas.updateTab(lista.get(i), state.getTAB(), pos, state.getTAB()[line][column]);
                                Play play = new Play(child, TYPE);
                                if(level == 0){
                                    play.setNextTab(child);
                                }else{
                                    play.setNextTab(state.getNextTab());
                                }
                                
                                v = max(v, min_value(play, level + 1, alpha, beta));
                                
                                if(v.getCount() > beta){
                                    return v;
                                }
                                
                                alpha = Math.max(v.getCount(), alpha);
                            }
                        }                        
                    }
                }                
            }
            return v;
        }
        Play p = new Play(state.getTAB(), state.isTYPE());
        p.setNextTab(state.getNextTab());
        return p;
    }
    
    private Play min_value(Play state, int level, int alpha, int beta) {
        
        //Altura máxima da árvore        
        if (level < MAX_LEVEL) {
            
            Play v = new Play(Integer.MAX_VALUE, !TYPE);
            
            //Percorre o tabuleiro inteiro
            for (int line = 0; line < state.getTAB().length; line++) {
                for (int column = 0; column < state.getTAB()[0].length; column++) {

                    //Se a peça for do tipo atual
                    if ((((state.getTAB()[line][column] == 1) && !TYPE) || ((state.getTAB()[line][column] == 2) && TYPE))
                            || (((state.getTAB()[line][column] == 3) && !TYPE) || ((state.getTAB()[line][column] == 4) && TYPE))) {

                        //Gera a lista de jogadas para tal posição
                        int pieceType = state.getTAB()[line][column];
                        ArrayList<ArrayList<int[]>> lista = Damas.verificaJogada(line, column, state.getTAB());
                        state.getTAB()[line][column] = pieceType;

                        //Posição atual
                        int[] pos = {line, column};

                        //Qtd máxima de peças que podem ser comidas
                        int maxCount = Damas.maxCount(lista, pos);                        
                        
                        for (int i = 0; i < lista.size(); i++) {

                            //Se a jogada for a máxima
                            if (Damas.count(lista.get(i), pos) == maxCount) {

                                //Atualiza o tabuleiro
                                int[][] child = Damas.updateTab(lista.get(i), state.getTAB(), pos, state.getTAB()[line][column]);
                                Play play = new Play(child, TYPE);                                
                                play.setNextTab(state.getNextTab());
                                
                                v = min(v, max_value(play, level + 1, alpha, beta));
                                
                                if(v.getCount() <= alpha){
                                    return v;
                                }
                                
                                beta = Math.min(v.getCount(), beta);
                            }
                        }                        
                    }
                }                
            }
            return v;
        }
        Play p = new Play(state.getTAB(), state.isTYPE());
        p.setNextTab(state.getNextTab());
        return p;
    }    
}
