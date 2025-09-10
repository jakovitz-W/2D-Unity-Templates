using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public GameObject hintText, diContainer, diText, cutsceneText;
    public Image containerImg; //profile photo of the character that's speaking
    public List<Dialogue> diList; //add to list in inspector to create dialogue instances
    public List<Dialogue> hintList;
    private bool hint; //flips based on current text being processed

    void Awake(){
        hintText.SetActive(false);
        diContainer.SetActive(false);
    }

    //find the index of dialogue instance with specified key, if key does not exist it returns -1
    public int FindIndexByKey(string searchKey){
        
        if(!hint){
            
            Dialogue current = diList.Find(x => x.key == searchKey);
            return diList.IndexOf(current);
        } else{
            Dialogue current = hintList.Find(x => x.key == searchKey);
            return hintList.IndexOf(current);
        }

        return -1; //error
    }

    //isHint to specify if the text should be hint text or a popup dialogue box
    //random will pick a random option
    public void SetText(string key, bool isHint, bool random, int specific){
        hint = isHint;
        int index = FindIndexByKey(key);
        if(hint){
            hintText.SetActive(true);
            hintText.GetComponent<TypewriterEffect>().SetString(hintList[index].GetDialogue(specific), true);
        } else{
            
            diContainer.SetActive(true);

            containerImg.sprite = diList[index].characterImage;
            if(!random){
                diText.GetComponent<TypewriterEffect>().SetString(diList[index].GetDialogue(specific), false);
            } else{
                diText.GetComponent<TypewriterEffect>().SetString(diList[index].GetRandomOption(), false);
            }
        }
    }

    public void SetCutsceneText(string key){
        int index = FindIndexByKey(key);
        cutsceneText.GetComponent<TypewriterEffect>().SetString(diList[index].GetDialogue(-1), false);
    }
}

[System.Serializable]
public class Dialogue{ //set fields in inspector
    public string key;
    [SerializeField] private List<string> di;
    private int iteration;
    public Sprite characterImage;
    public bool hasNext = true;

    void Awake(){
        iteration = 0;
    }

    //checks if next option exists & picks from list accordingly || for ordered dialogue
    public string GetDialogue(int specific){
        string option;

        if(specific != -1){
            option = di[specific];
            return option;
        }
        option = di[iteration];

        if(iteration < (di.Count - 1)){ //.Count starts counting at 1, index for lists start at 0
            iteration++;
        } else{
            hasNext = false;
        }

        return option;
    }

    //for unordered dialogue
    public string GetRandomOption(){

        int rand = Random.Range(0, (di.Count));
        string option = di[rand];

        return option;
    }
}