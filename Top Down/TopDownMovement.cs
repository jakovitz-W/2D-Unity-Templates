using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class TopDownMovement : MonoBehaviour
{
    private PlayerControls playerControls;
    private GameObject initialSpawn;
    public GameObject[] respawnPoints;
    private GameObject respawnPoint;
    private int roomIndex = 0;
    private int totalRooms;

    public GameObject[] teleportSpots;
    private GameObject nextTeleportSpot;

    public float runSpeed = 20f;
    public float sprintModifier = 2f;
    private bool isSprinting = false;
    private Rigidbody2D rb;
    private Vector2 movement;
    public int enemiesKilled = 0;
    private int deathCount = 0;
    public TextMeshProUGUI deathCountText;
    public GameObject escapePrompt;
    private bool requirementReached = false;
    public GameObject finishMenu;

    private void Awake(){
        playerControls = new PlayerControls();
    }
    private void OnEnable(){
        playerControls.Enable();
    }

    private void OnDisable(){
        playerControls.Disable();
    }
    private void Start(){

        rb = GetComponent<Rigidbody2D>();
        
        initialSpawn = respawnPoints[0];
        this.transform.position = initialSpawn.transform.position;
        escapePrompt.SetActive(false);

        //makes sure all teleport spots are inactive
        for(int i = 0; i < teleportSpots.Length; i++){
            teleportSpots[i].SetActive(false);
        }
        totalRooms = teleportSpots.Length;
        finishMenu.SetActive(false);

        //remember to set all room spawn/teleport points in order in inspector
        respawnPoint = respawnPoints[0];        
        nextTeleportSpot = teleportSpots[0];
        enemiesKilled = 0;
        deathCount = 0;
    }
    void Update(){
        movement = playerControls.Land.Move.ReadValue<Vector2>();
        Flip(movement.x, movement.y);

        //replace later
        if(playerControls.Land.Sprint.ReadValue<float>() > 0){
            isSprinting = true;
        } else{
            isSprinting = false;
        }
        
        //only allow player to move on when certain # of enemies are killed
        //can expand conditions to finding keys/defeating bosses
        if(enemiesKilled >= nextTeleportSpot.GetComponent<TeleportPlayer>().requirement && !requirementReached){
            requirementReached = true;
            if(requirementReached){
                nextTeleportSpot.SetActive(true);
                escapePrompt.SetActive(true);
            }
        }
    }

    void FixedUpdate(){
        if(isSprinting){
            rb.linearVelocity = movement * runSpeed * sprintModifier;
        } else{
            rb.linearVelocity = movement * runSpeed;
        }
        
    }
    
    //this function is a nightmare
    void Flip(float x, float y){

        if(x > 0){
            
            if(y > 0){
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            }else if(y < 0){
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else{
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
        }else if(x < 0){
            
            if(y > 0){
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

            }else if(y < 0){
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else{
                this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
        if(y > 0 && x == 0){
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        }else if(y < 0 && x == 0){
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
    }   

    /* void OnCollisionEnter2D(Collision2D other){

        if(other.gameObject.CompareTag("Enemy")){
            //OnDeath();
        }
    } */

    public void OnRoomChange(){
        
        roomIndex++;
        if(roomIndex < totalRooms){
            respawnPoint = respawnPoints[roomIndex];
            nextTeleportSpot = teleportSpots[roomIndex];
            enemiesKilled = 0;
            requirementReached = false;
            escapePrompt.SetActive(false);
        } else{
            //end
            StartCoroutine(EndGame());
        }
    }

    IEnumerator EndGame(){
        finishMenu.SetActive(true);
        yield return new WaitForSeconds(3);
        GameObject.Find("SceneManager").GetComponent<SceneManagement>().OnMain();
    }

    public void OnDeath(){
        this.transform.position = respawnPoint.transform.position;
        deathCount++;
        deathCountText.text = "Deaths: " + deathCount;
    }
}
