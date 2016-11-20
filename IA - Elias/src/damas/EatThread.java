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
public class EatThread extends Thread {

    private final ArrayList<ArrayList<int[]>> JOGADAS;
    private final ArrayList<int[]> JOGADA;
    private static int MAX;
    private final int COR_ADVERSARIO;
    private final int I;
    private final int J;
    private final int D;
    private final int SIGNAL_LINE;
    private final int SIGNAL_COLUMN;

    private final String NAME;
    private int[][] M;

    public EatThread(ArrayList<ArrayList<int[]>> jogadas, ArrayList<int[]> jogada, int max, int[][] m, int corAdversario, int i, int j, int d, int signal_line, int signal_column, String name) {
        this.JOGADAS = jogadas;
        this.JOGADA = jogada;
        this.MAX = max;
        this.COR_ADVERSARIO = corAdversario;
        this.I = i;
        this.J = j;
        this.D = d;
        this.SIGNAL_LINE = signal_line; // +1 OU -1
        this.SIGNAL_COLUMN = signal_column; // +1 OU -1
        this.NAME = name;
        this.M = m;
    }

    @Override
    public void run() {
        //Jogada para trás
        ArrayList<int[]> jogada1 = new ArrayList<>();
        ArrayList<ArrayList<int[]>> jogadasTemp = new ArrayList<>();
        int[][] tab = igualaTabuleiro(M);
        boolean comeu1 = false;
        for (int k = 1; k < MAX; k++) {
            System.out.println(NAME);
            if (compara(I + SIGNAL_LINE * k * D, J + SIGNAL_COLUMN * k, tab, COR_ADVERSARIO) || compara(I + k * D, J + k, tab, COR_ADVERSARIO + 2)) {
                if (compara(I + SIGNAL_LINE * 2 * k * D, J + SIGNAL_COLUMN * 2 * k, tab, 0)) {
                    tab[I + SIGNAL_LINE * k * D][J + SIGNAL_COLUMN * k] = 0;

                    int[] posicao = {I + SIGNAL_LINE * 2 * k * D, J + SIGNAL_COLUMN * 2 * k};

                    jogada1.add(posicao);
                    adicionaNoFinal(jogadasTemp, concatenaListaJogadas(jogada1, eatPiece(I + SIGNAL_LINE * 2 * k * D, J + SIGNAL_COLUMN * 2 * k, MAX, tab, D, jogada1, false, NAME)));
                    //System.out.println("");
                    //imprime(tab);
                    comeu1 = true;
                    k++;
                } else {
                    k = MAX;
                }
            } else if (compara(I + SIGNAL_LINE * k * D, J + SIGNAL_COLUMN * k, tab, 0)) {
                tab[I + SIGNAL_LINE * k * D][J + SIGNAL_COLUMN * k] = 3;

                int[] posicao = {I + SIGNAL_LINE * k * D, J + SIGNAL_COLUMN * k};

                jogada1.add(posicao);

                if (comeu1) {
                    adicionaNoFinal(jogadasTemp, concatenaListaJogadas(JOGADA, eatPiece(I + SIGNAL_LINE * k * D, J + SIGNAL_COLUMN * k, MAX, tab, D, jogada1, false, NAME)));
                }
                //System.out.println("");
                //imprime(tab);
            } else {
                k = MAX;
            }
        }
        if (comeu1) {
            adicionaJogadas(JOGADAS, jogada1, 0);
            adicionaNoFinal(JOGADAS, jogadasTemp);
        }
        this.interrupt();
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
