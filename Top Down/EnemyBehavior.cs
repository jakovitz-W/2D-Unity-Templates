using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private Rigidbody2D rb;
    [Range(5, 100)] public float maxHealth;
    public float health;
    [SerializeField] FloatingHealthBar healthbar;

    public float damageFromMelee;
    public float damageFromRanged;
    public enum DamageType{
        Debug, 
        Melee, 
        Ranged
    }
    public DamageType damageType;
    public GameObject player;
    public float idleTime = 2f;
    public PlayerTracking tracker;
    public float speed = 5f;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthbar = GetComponentInChildren<FloatingHealthBar>();
        player = GameObject.FindGameObjectWithTag("Player");
        tracker = GetComponentInChildren<PlayerTracking>();

        switch((int)damageType){
            case 0:
                tracker.setType(0);
                Debug.Log("Debug type");
                break;
            case 1:
                tracker.setType(1);
                break;
            case 2:
                tracker.setType(2);
                if(speed == 0){
                    tracker.canMove = false;
                }
                break;
            default:
                //do nothing
                break;
        }
    }

    void FixedUpdate(){
        if(tracker.isChasing){
            switch(tracker.direction){
                case 1:
                    rb.linearVelocity = new Vector2(0f, speed);
                    break;
                case 2:
                    rb.linearVelocity = new Vector2(0f, -speed);
                    break;
                case 3:
                    rb.linearVelocity = new Vector2(speed, 0f);
                    break;
                case 4:
                    rb.linearVelocity = new Vector2(-speed, 0f);
                    break;
                default:
                    break;
            }
        }
    }
    public void TakeDamage(float damageAmount){
        health -= damageAmount;
        healthbar.UpdateHealthBar(health, maxHealth);
        if(health <= 0){
            player.GetComponent<TopDownMovement>().enemiesKilled++;
            Destroy(gameObject);
        }
    }
}
