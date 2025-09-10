using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private Boss boss;
    //store stages in GameObject[] and set active/unactive to reduce lag
    public Level[] levels; //array of Level class (found at end of script)
    private GameObject player;
    [HideInInspector] public int LIndex; //level index, increment/decrement upon changing rooms
    public GameObject[] levelParents;  //objects contained in a room get placed under a single parent in the hierarchy
    [SerializeField] private GameObject[] doors; //exit point for each level
    [SerializeField] private Animator transition; //fade transition object, not required. Setup explained in doc.

    //rename to the name of your Movement/Interactions scripts, or remove if not using
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerInteractions interactions;
    
    public IEnumerator OnRoomChange(int room){ //this method allows the player to go to the next room
        
        //key spam protection
        movement.enabled = false;
        interactions.enabled = false;

        //starts fade transition
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);

        //activates next room & teleports the player
        levelParents[room].SetActive(true);
        player.transform.position = levels[room].checkpoint.transform.position;
        
        //deactivates all other rooms
        for(int i = 0; i < levels.Length; i++){
            if(i != room){
                levelParents[i].SetActive(false);
            }
        }

        LIndex = room;

        //end fade transition
        yield return new WaitForSeconds(1);
        transition.SetTrigger("End");
        yield return new WaitForSeconds(1);
        movement.enabled = true;
        interactions.enabled = true;
    }

    public IEnumerator Backtrack(){ //this method allows the player to go to the previous room
        
        //key spam protection
        movement.enabled = false;
        interactions.enabled = false;
        
        //starts fade transition
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        LIndex--;

        //sets the previous room active & teleports the player to the last room's exit point
        levelParents[LIndex].SetActive(true);
        player.transform.position = doors[LIndex].transform.position;
        interactions.TeleportHumans(LIndex);

        //deactivates all other rooms
        for(int i = 0; i < levels.Length; i++){
            if(i != LIndex){
                levelParents[i].SetActive(false);
            }
        }

        //end fade transition
        yield return new WaitForSeconds(1);
        transition.SetTrigger("End");
        yield return new WaitForSeconds(1);

        interactions.enabled = true;
        movement.enabled = true;
    }

    void Start(){
        Time.timeScale = 1f;
        player = GameObject.FindWithTag("Player");

        //if using save system set to last index player was in
        LIndex = 0;
        
        for(int i = LIndex + 1; i < levels.Length; i++){
            levelParents[i].SetActive(false);
            levels[i].hasKey = false;//load from save if using
        }

        //starts player in first room
        //for save/load system store the index of the last room the player was in & start there
        player.transform.position = levels[0].checkpoint.transform.position;
    }
}


[System.Serializable]
public class Level{ //used to store information about individual rooms in the scene
    public GameObject checkpoint; //entry/backtrack point
    public bool hasKey; //allows access to the room's exit point
    public bool isBossRoom = false;
} 

