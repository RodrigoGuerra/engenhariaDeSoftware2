/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package damas;

import java.util.ArrayList;
import static damas.Damas.*;

/**
 *
 * @author Elias
 */
public class Play {
    private final int[][] TAB;            
    private int[][] nextTab; 
    //Type é verdadeiro para 1 ou 3 e falso para 2 ou 4
    private final boolean TYPE;
    private final int COUNT;
    private ArrayList<int[]> play = new ArrayList<>();

    public Play(int COUNT, boolean TYPE) {
        this.TAB = null;
        this.TYPE = true;
        this.COUNT = COUNT;
    }
    
    public Play(int[][] TAB, boolean TYPE) {
        this.TAB = TAB;
        this.TYPE = TYPE;
        
        int number = 0;
        for (int i = 0; i < TAB.length; i++) {
            for (int j = 0; j < TAB[0].length; j++) {
                //Se a peça for do tipo atual
                if (((TAB[i][j] == 1) && TYPE) || ((TAB[i][j] == 2) && !TYPE)) {
                    number++;
                } else if (((TAB[i][j] == 3) && TYPE) || ((TAB[i][j] == 4) && !TYPE)) {
                    number+=3;
                } else if (((TAB[i][j] == 1) && !TYPE) || ((TAB[i][j] == 2) && TYPE)) {
                    number--;
                } else if (((TAB[i][j] == 3) && !TYPE) || ((TAB[i][j] == 4) && TYPE)) {
                    number-=3;
                }
            }
        }
        //System.out.println("\n\n");
        //imprime(TAB);
        //System.out.println("COUNT: " + number + " - TYPE: " + TYPE);
        this.COUNT = number; 
    }

    public void setPlay(ArrayList<int[]> play) {
        this.play = play;
    }

    public void setNextTab(int[][] nextTab) {
        this.nextTab = nextTab;
    }

    public int[][] getNextTab() {
        return nextTab;
    }

    public boolean isTYPE() {
        return TYPE;
    }

    public int[][] getTAB() {
        return TAB;
    }

    public int getCount() {        
        return COUNT;        
    }        
}
