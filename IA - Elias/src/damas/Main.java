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
public class Main {

    private static final int[][] TABULEIRO
            = {{0, 1, 0, 1, 0, 1, 0, 1},
            {1, 0, 1, 0, 0, 0, 0, 0},
            {0, 1, 0, 1, 0, 0, 0, 1},
            {0, 0, 1, 0, 2, 0, 0, 0},
            {0, 0, 0, 3, 0, 0, 0, 0},
            {2, 0, 0, 0, 2, 0, 2, 0},
            {0, 2, 0, 2, 0, 0, 0, 2},
            {2, 0, 2, 0, 2, 0, 2, 0}};

    private static final int[][] tabuleiro1
            = {{0, 1, 0, 0, 0, 0, 0, 1},
            {1, 0, 1, 0, 1, 0, 1, 0},
            {0, 1, 0, 1, 0, 0, 0, 1},
            {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 1, 0, 2, 0, 0, 0, 0},
            {2, 0, 2, 0, 2, 0, 2, 0},
            {0, 2, 0, 0, 0, 0, 0, 2},
            {2, 0, 2, 0, 2, 0, 2, 0}};

//    private static final int[][] TABULEIRO
//            = {{0, 1, 0, 0},
//               {1, 0, 1, 0},
//               {0, 2, 0, 2},
//               {0, 0, 0, 0}};
    private static final int MAX = 8;

    //private ArrayList<ArrayList<int[]>> jogadas;
    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        System.out.println("MAIN");
//        MIN_MAX_THREAD mm = new MIN_MAX_THREAD(TABULEIRO, true, 1);
//        Play p = mm.search();
//        imprime(p.getTAB());
//        System.out.println("\n\n");
//        imprime(p.getNextTab());
        imprimeLista(verify(4, 3, MAX, TABULEIRO, -1, null, true));
        System.out.println("TESTE");
    }

    public static ArrayList<ArrayList<int[]>> verificaJogada(int i, int j, int[][] m) {
        ArrayList<ArrayList<int[]>> jogadas = new ArrayList<>();

        switch (m[i][j]) {
            case 1:
                return verify(i, j, 2, m, -1, jogadas, false);
            case 2:
                return verify(i, j, 2, m, 1, jogadas, false);
            case 3:
                return verify(i, j, MAX, m, -1, jogadas, true);
            case 4:
                return verify(i, j, MAX, m, 1, jogadas, true);
        }
        return jogadas;
    }

    public static ArrayList<ArrayList<int[]>> verify(int i, int j, int max, int[][] m, int d, ArrayList<ArrayList<int[]>> jogadas, boolean isKing) {
        int corAdversario = 2;
        if (d == 1) {
            corAdversario = 1;
        }

        m[i][j] = 0;

        //Jogada para trás
        //Direita
        PlayThread pt_BR = new PlayThread(jogadas, m, max, corAdversario, i, j, d, isKing, isKing, 1, 1, "pt_BR");
        //Esquerda
        PlayThread pt_BL = new PlayThread(jogadas, m, max, corAdversario, i, j, d, isKing, isKing, 1, -1, "pt_BL");

        //Jogada para frente
        //Direita
        PlayThread pt_FR = new PlayThread(jogadas, m, max, corAdversario, i, j, d, isKing, isKing, -1, 1, "pt_FR");
        //Esquerda
        PlayThread pt_FL = new PlayThread(jogadas, m, max, corAdversario, i, j, d, isKing, isKing, -1, -1, "pt_FL");

        pt_BR.start();
        pt_BL.start();
        pt_FR.start();
        pt_FL.start();

        return jogadas;
    }

    public static ArrayList<ArrayList<int[]>> eatPiece(int i, int j, int max, int[][] m, int d, ArrayList<int[]> jogada, boolean comeu, String name) {
        //System.out.println("\nCOMER PECA\n");
        int corAdversario = 2;
        if (d == 1) {
            corAdversario = 1;
        }

        //imprime(m);
        //System.out.println("");
        //System.out.println("i: " + i);
        //System.out.println("j: " + j);
        ArrayList<ArrayList<int[]>> jogadas = new ArrayList<>();
        
        //Jogada para trás
        //Direita
        EatThread et_BR = new EatThread(jogadas, jogada, max, m, corAdversario, i, j, d, 1, 1, name);
        //Esquerda
        EatThread et_BL = new EatThread(jogadas, jogada, max, m, corAdversario, i, j, d, 1, -1, name);

        //Jogada para frente
        //Direita
        EatThread et_FR = new EatThread(jogadas, jogada, max, m, corAdversario, i, j, d, -1, 1, name);
        //Esquerda
        EatThread et_FL = new EatThread(jogadas, jogada, max, m, corAdversario, i, j, d, -1, -1, name);

        et_BR.start();
        et_BL.start();
        et_FR.start();
        et_FL.start();

        return jogadas;
    }

    public static ArrayList<ArrayList<int[]>> insere(int i, int j, int[][] m, ArrayList<ArrayList<int[]>> jogadas) {
        int[] posicao = {i, j};
        if (m[i][j] != 0) {
            ArrayList<int[]> jogada = new ArrayList<>();
            jogada.add(posicao);
            jogadas.add(jogada);
        }
        return jogadas;
    }

    public static boolean compara(int i, int j, int[][] m, int num) {
        return (i < MAX && i >= 0 && j < MAX && j >= 0 && m[i][j] == num);
    }

    public static void imprime(int[][] m) {
        int l = m.length;
        int c = m[0].length;
        for (int i = 0; i < l; i++) {
            for (int j = 0; j < c; j++) {
                System.out.print(m[i][j] + "  ");
            }
            System.out.println("");
        }
    }

    public static void imprimeLista(ArrayList<ArrayList<int[]>> jogadas) {
        jogadas.stream().map((jogada) -> {
            jogada.stream().forEach((vetor) -> {
                //System.out.print("{" + vetor[0] + ", " + vetor[1] + "} ");
            });
            return jogada;
        }).forEach((_item) -> {
            //System.out.println("");
        });
    }

    public static int[][] igualaTabuleiro(int[][] m) {
        int[][] resp = new int[m.length][m[0].length];

        for (int i = 0; i < resp.length; i++) {
            for (int j = 0; j < resp[0].length; j++) {
                resp[i][j] = m[i][j];
            }
        }
        return resp;
    }

    public static void adicionaJogadas(ArrayList<ArrayList<int[]>> jogadas, ArrayList<int[]> jogada, int inicio) {
        //System.out.println(jogada.size());
        for (int i = inicio; i < jogada.size(); i++) {
            ArrayList<int[]> jogadaTemp = new ArrayList<>();
            for (int j = 0; j <= i; j++) {
                jogadaTemp.add(jogada.get(j));
            }
            jogadas.add(jogadaTemp);
        }
    }

    public static ArrayList<int[]> concatenaJogadas(ArrayList<int[]> jogadaA, ArrayList<int[]> jogadaB) {
        jogadaB.stream().forEach((pos) -> {
            jogadaA.add(pos);
        });

        return jogadaA;
    }

    public static ArrayList<int[]> concatenaJogadas(ArrayList<int[]> jogadaA, ArrayList<int[]> jogadaB, int max) {
        ArrayList<int[]> jogadaTemp = new ArrayList<>();
        for (int i = 0; i < jogadaA.size(); i++) {
            jogadaTemp.add(jogadaA.get(i));
        }
        for (int i = 0; i <= max; i++) {
            jogadaTemp.add(jogadaB.get(i));
        }
        return jogadaTemp;
    }

    public static ArrayList<ArrayList<int[]>> concatenaListaJogadas(ArrayList<int[]> ultimaJogada, ArrayList<ArrayList<int[]>> jogadasNovas) {

        if (jogadasNovas == null) {
            return null;
        }

        ArrayList<ArrayList<int[]>> jogadasTemp = new ArrayList<>();
        for (int k = 0; k < jogadasNovas.size(); k++) {
            for (int i = 0; i < jogadasNovas.get(k).size(); i++) {
                jogadasTemp.add(concatenaJogadas(ultimaJogada, jogadasNovas.get(k), i));
            }
        }

        return jogadasTemp;
    }

    public static void adicionaNoFinal(ArrayList<ArrayList<int[]>> jogadas1, ArrayList<ArrayList<int[]>> jogadas2) {
        if (jogadas2 == null) {
            return;
        }

        for (int i = 0; i < jogadas2.size(); i++) {
            jogadas1.add(jogadas2.get(i));
        }
    }

    public static int maxCount(ArrayList<ArrayList<int[]>> jogadas, int[] pos) {
        int maxCount = 0, actualCount = 0;
        if (jogadas == null) {
            return maxCount;
        }

        for (int i = 0; i < jogadas.size(); i++) {
            actualCount = count(jogadas.get(i), pos);
            if (actualCount > maxCount) {
                maxCount = actualCount;
            }
        }
        return maxCount;
    }

    public static int count(ArrayList<int[]> jogadas, int[] pos) {
        int count = 0;
        if (jogadas == null) {
            return count;
        }

        for (int j = 0; j < jogadas.size(); j++) {
            if (j == 0) {
                if (!isAdjacent(pos, jogadas.get(j))) {
                    count++;
                }
            } else if (!isAdjacent(jogadas.get(j - 1), jogadas.get(j))) {
                count++;
            }
        }

        return count;
    }

    public static boolean isAdjacent(int[] pos1, int[] pos2) {
        if (Math.abs(pos1[0] - pos2[0]) == 1) {
            return true;
        }

        if (Math.abs(pos1[1] - pos2[1]) == 1) {
            return true;
        }

        return false;
    }

    public static int[][] updateTab(ArrayList<int[]> jogadas, int[][] m, int[] pos, int type) {
        int[][] resp = igualaTabuleiro(m);
        int l = 0;
        int c = 0;
        if (jogadas == null) {
            return resp;
        }

        resp[pos[0]][pos[1]] = 0;
        for (int j = 0; j < jogadas.size(); j++) {
            l = 0;
            c = 0;
            if (j == 0) {
                if (!isAdjacent(pos, jogadas.get(j))) {
                    l = centerPieces(pos[0], jogadas.get(j)[0]);
                    c = centerPieces(pos[1], jogadas.get(j)[1]);

                    resp[l][c] = 0;
                }
            } else if (!isAdjacent(jogadas.get(j - 1), jogadas.get(j))) {
                l = centerPieces(jogadas.get(j - 1)[0], jogadas.get(j)[0]);
                c = centerPieces(jogadas.get(j - 1)[1], jogadas.get(j)[1]);

                resp[l][c] = 0;
            }
        }
        for (int i = 0; i < MAX; i++) {
            if (resp[MAX - 1][i] == 1) {
                resp[MAX - 1][i] = 3;
            }
            if (resp[0][i] == 2) {
                resp[0][i] = 4;
            }
        }

        if (jogadas.size() > 0) {
            l = jogadas.get(jogadas.size() - 1)[0];
            c = jogadas.get(jogadas.size() - 1)[1];
            if ((l > 7) || (c > 7)) {
                System.out.println("");
            }
            resp[l][c] = type;
        }

        return resp;
    }

    public static int centerPieces(int pos1, int pos2) {
        if (pos1 > pos2) {
            return pos1 - 1;
        } else if (pos1 == pos2) {
            return pos1;
        }

        return pos1 + 1;
    }

    public static boolean typeFromPosition(int[] pos, int[][] tab) {
        return ((tab[pos[0]][pos[1]] == 1)
                || (tab[pos[0]][pos[1]] == 3));
    }
}
