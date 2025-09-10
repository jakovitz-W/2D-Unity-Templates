using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//projectiles shot from enemies
public class Projectile : MonoBehaviour
{
    private GameObject player;
    private float speed = 3f;
    private float targetX, targetY;

    void Awake(){
        player = GameObject.FindGameObjectWithTag("Player");
        targetX = player.transform.position.x;
        targetY = player.transform.position.y;
    }

    void Update(){
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetX, targetY), speed * Time.deltaTime);
        
        if(transform.position.x == targetX && transform.position.y == targetY){
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.CompareTag("Bounds")){
            Destroy(gameObject);
        } else if(col.gameObject.CompareTag("Player")){
            player.GetComponent<TopDownMovement>().OnDeath();
            Destroy(gameObject);
        } else if(col.gameObject.CompareTag("Shield")){
            StartCoroutine(player.GetComponent<PlayerCombat>().ShieldCooldown(1f));
            Destroy(gameObject);
        }
    }
}
