/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package damas;

/**
 *
 * @author Elias
 */
public class Utils {
    public static Play max(Play play1, Play play2){
        if(play1 == null){
            return play2;
        }
        
        if(play1.getCount() > play2.getCount()){
            return play1;
        }else{
            return play2;
        }
    }
    
    public static Play min(Play play1, Play play2){
        if(play1 == null){
            return play2;
        }
        
        if(play1.getCount() < play2.getCount()){
            return play1;
        }else{
            return play2;
        }
    }
}
