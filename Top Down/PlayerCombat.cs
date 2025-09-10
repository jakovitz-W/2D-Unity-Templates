using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private PlayerControls playerControls;
    private int weapon = 0;
    //melee is = 0; ranged is = 1;
    public Transform attackPoint;
    public GameObject crossHair;
    public float attackRange = 1f;
    public LayerMask enemyLayers;

    private bool swordActive = false;
    private bool bowActive = false;
    private bool shieldActive = false;
    public float shieldCooldownTime = .5f;
    public GameObject shield;

    private void Awake(){
        playerControls = new PlayerControls();
        shield.SetActive(false);
        swordActive = true;
        weapon = 0;
    }
    private void OnEnable(){
        playerControls.Combat.Attack.performed += ProcessAttack;
        playerControls.Combat.Switch.performed += Switch;
        playerControls.Enable();
    }

    private void OnDisable(){
        playerControls.Combat.Attack.performed -= ProcessAttack;
        playerControls.Combat.Switch.performed -= Switch;
        playerControls.Disable();
    }

    void ProcessAttack(InputAction.CallbackContext ctx){
        if(ctx.performed){
            if(weapon == 0){
                
            //melee
            //play animation
                Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
                foreach(Collider2D enemy in hitEnemies){
                    Debug.Log("Hit Enemy");
                    if(!enemy.gameObject.CompareTag("Projectile")){
                        float damage = enemy.GetComponent<EnemyBehavior>().damageFromMelee;
                        //Debug.Log(damage);
                        enemy.GetComponent<EnemyBehavior>().TakeDamage(damage);
                    }
                }
                
            }else if(weapon == 1){

                //I did not finish ranged combat either. Consider it a coding challenge, Your can Do it I believe at you
                //ranged
                //Instantiate player projectile from crosshair
                    //(create seperate prefab + script)
                    //move on fixed path until wall or enemy hit
                        //moveTowards might not work here
                    //modify melee hit statement
            }
        }
    }

    void Switch(InputAction.CallbackContext ctx){
        if(ctx.performed){
            if(weapon != 0){ //use ints for weapon types, or enums for readability
                weapon = 0;
                swordActive = true;
                attackPoint.gameObject.SetActive(true);

                bowActive = false;
                //set bow gameobject active state to false
            } else if(weapon != 1){
                weapon = 1;
                swordActive = false;
                attackPoint.gameObject.SetActive(false);

                bowActive = true;
                //set bow gameobject active state to true
            }
        }
    }

    void Update(){
        if((playerControls.Combat.Shield.ReadValue<float>() > 0) && !shieldActive && swordActive){
            Debug.Log("Shield activated");
            shieldActive = true;
            attackPoint.gameObject.SetActive(false);
            shield.SetActive(true);
        } 
        else if(shieldActive && (playerControls.Combat.Shield.ReadValue<float>() == 0)){
            StartCoroutine(ShieldCooldown(shieldCooldownTime));
        }

        if(bowActive){
            /*--Bow Combat--
                - point crosshair in the direction of the mouse
                
            */
        }
    }

    public IEnumerator ShieldCooldown(float time){

        shield.SetActive(false);
        attackPoint.gameObject.SetActive(true);

        yield return new WaitForSeconds(time);
        shieldActive = false; 
    }

    void OnDrawGizmosSelected(){
        if(attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}