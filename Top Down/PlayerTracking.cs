using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//take this script with the smallest grain of salt.i don't remember if i fixed the violent rotation problem or not.
public class PlayerTracking : MonoBehaviour
{
    public Rigidbody2D rbParent;
    public bool playerSeen = false;
    private int type;
    public bool isChasing = false;
    public float speed = 2f;
    private float currentRotation = 0f;
    public GameObject player;
    public RaycastHit2D hit;
    public bool pathBlocked = false;
    public int direction; //1 = down, 2 = up, 3 = left, 4 = right
    public bool canMove = true;
    private bool rangedStarted = false;
    public GameObject projectile;

    void Awake(){
        playerSeen = false;
        isChasing = false;
        rbParent = GetComponentInParent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        currentRotation = 0f;
    }

    public void setType(int typeFromParent){
        type = typeFromParent;
    }

    void OnTriggerEnter2D(Collider2D other){

        if(other.gameObject.CompareTag("Player") && !playerSeen){
            playerSeen = true;
            isChasing = true;
        }
    }

    void FixedUpdate(){

        LayerMask mask = LayerMask.GetMask("Bounds");
        hit = Physics2D.Raycast(transform.position, Vector2.up, 1.0f, mask); //FOR THE LOVE OF CHEESE REMEMBER TO SET THE LENGTH OF YOUR CAST I SPENT  W E E K S  ON THIS
        if(playerSeen && isChasing){    
            switch(type){
                case 0:
                    //Debug.Log("Debug Type);
                    //chooseDirection();
                    projectile = null;
                    break;
                case 1:
                    chooseDirection();
                    projectile = null;
                    break;
                case 2:
                    if(canMove){
                        chooseDirection();
                    }
                    if(!rangedStarted){
                        rangedStarted = true;
                        StartCoroutine(Ranged());
                    }
                    break;
                default:
                    break;
            }
        }

        if(hit && canMove){
            pathBlocked = true;
            currentRotation += 90f;
            setRotation(currentRotation);
        } else{
            pathBlocked = false;
        }
    }

    void setRotation(float rotation){
        this.currentRotation = rotation;
        rbParent.rotation = currentRotation;
    }

    void chooseDirection(){
            Vector2 playerPos = player.transform.position;
            float xDist = this.transform.position.x - playerPos.x;
            float yDist = this.transform.position.y - playerPos.y;

        if(!hit){
            if(Mathf.Abs(xDist) < Mathf.Abs(yDist)){
                if(yDist < 0){
                    setRotation(0);
                    direction = 1;
                }
                else if(yDist > 0){
                    setRotation(180);
                    direction = 2;
                }
            } else if(Mathf.Abs(xDist) >= Mathf.Abs(yDist)){
                if(xDist < 0){
                    setRotation(-90f);
                    direction = 3;
                }
                else if(xDist > 0){
                    setRotation(90f);
                    direction = 4;
                }
            }
        }
    }

    IEnumerator Ranged(){
        while(gameObject.active){
            Instantiate(projectile, this.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(3f);
        }
    }
}
