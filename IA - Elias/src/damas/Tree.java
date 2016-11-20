/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package damas;

import java.util.ArrayList;

/**
 *
 * @author Elias
 */
public class Tree {

    private final int LEVEL = 0;
    private int bestLevel;
    private final int MAX_LEVEL;

    private final int[][] TAB;

    private int[][] bestTab;
    private int[][] lastTab;
    
    private int bestCountOponents = Integer.MAX_VALUE;
    private int bestCountPieces = 0;
    //Type é verdadeiro para 1 ou 3 e falso para 2 ou 4
    private final boolean TYPE;
    //private ArrayList<Tree> children = new ArrayList<>();
    private ArrayList<ArrayList<int[]>> plays = new ArrayList<>();
    private ArrayList<int[]> bestPlay = new ArrayList<>();
    
    private ArrayList<int[][]> nextTabs = new ArrayList<>();

    public Tree(int[][] tab, boolean type, int maxLevel) {
        this.TAB = tab;
        this.bestTab = tab;
        this.TYPE = type;
        this.MAX_LEVEL = maxLevel;
        this.bestLevel = MAX_LEVEL;
    }

    public void generateTree() {
        generateTree(TAB, TYPE, LEVEL);
    }

    private void generateTree(int[][] tab, boolean type, int level) {
        
        //Altura máxima da árvore        
        if (level < MAX_LEVEL) {

            //Percorre o tabuleiro inteiro
            for (int line = 0; line < tab.length; line++) {
                for (int column = 0; column < tab[0].length; column++) {

                    //Se a peça for do tipo atual
                    if ((((tab[line][column] == 1) && TYPE) || ((tab[line][column] == 2) && !TYPE))
                            || (((tab[line][column] == 3) && TYPE) || ((tab[line][column] == 4) && !TYPE))) {

                        //Gera a lista de jogadas para tal posição
                        int pieceType = tab[line][column];
                        ArrayList<ArrayList<int[]>> lista = Damas.verificaJogada(line, column, tab);
                        tab[line][column] = pieceType;

                        //Posição atual
                        int[] pos = {line, column};

                        //Qtd máxima de peças que podem ser comidas
                        int maxCount = Damas.maxCount(lista, pos);

                        for (int i = 0; i < lista.size(); i++) {

                            //Se a jogada for a máxima
                            if (Damas.count(lista.get(i), pos) == maxCount) {

                                //Atualiza o tabuleiro
                                int[][] child = Damas.updateTab(lista.get(i), tab, pos, tab[line][column]);

                                //System.out.println("\n\nNível: " + LEVEL + "\n");
                                //Damas.imprime(child);
                                //System.out.println("\n\n" + lista.get(i).get(lista.get(i).size() - 1)[0] + ", " + lista.get(i).get(lista.get(i).size() - 1)[1]);
                                //System.out.println("\n\n");

                                //Gera o filho de tipo diferente
                                //Tree tree = new Tree(child, !TYPE, LEVEL);
                                //tree.generateTree();
                                if (level == 0) {
                                    nextTabs.add(child);
                                    plays.add(lista.get(i));
                                }
                                generateTree(child, !type, level + 1);

                                //Adiciona à lista de filhos
                                //children.add(tree);
                            }
                        }

                    }
                }
            }
        }
        int countOfOponents = numberOfPieces(tab, !TYPE);
        int countOfPieces = numberOfPieces(tab, TYPE);        

        if (countOfOponents < bestCountOponents) {
            bestCountOponents = countOfOponents;
            bestTab = nextTabs.get(nextTabs.size() - 1);
            bestLevel = level;
            bestCountPieces = countOfPieces;
            bestPlay = plays.get(plays.size()-1);
            lastTab = tab;
        } else if (countOfOponents == bestCountOponents) {
            if (countOfPieces > bestCountPieces) {
                bestCountOponents = countOfOponents;
                bestTab = nextTabs.get(nextTabs.size() - 1);
                bestLevel = level;
                bestCountPieces = countOfPieces;
                bestPlay = plays.get(plays.size()-1);
                lastTab = tab;
            } else if (countOfPieces == bestCountPieces) {
                if (level < bestLevel) {
                    bestCountOponents = countOfOponents;
                    bestTab = nextTabs.get(nextTabs.size() - 1);
                    bestLevel = level;
                    bestCountPieces = countOfPieces;
                    bestPlay = plays.get(plays.size()-1);
                    lastTab = tab;
                }
            }
        }
    }

    private static int numberOfPieces(int[][] tab, boolean type) {
        int number = 0;
        for (int i = 0; i < tab.length; i++) {
            for (int j = 0; j < tab[0].length; j++) {
                //Se a peça for do tipo atual
                if ((((tab[i][j] == 1) && type) || ((tab[i][j] == 2) && !type))
                        || (((tab[i][j] == 3) && type) || ((tab[i][j] == 4) && !type))) {
                    number++;
                }
            }
        }
        return number;
    }

    public int[][] getBestTab() {
        return bestTab;
    }

    public ArrayList<int[]> getBestPlay() {
        return bestPlay;
    }

    public ArrayList<int[][]> getNextTabs() {
        return nextTabs;
    }

    public int[][] getLastTab() {
        return lastTab;
    }
    
}
