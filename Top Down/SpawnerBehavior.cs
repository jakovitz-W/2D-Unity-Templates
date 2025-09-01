using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerBehavior : MonoBehaviour
{
    public GameObject enemyType;
    public int enemiesPerWave;
    public int enemyLimit;
    private int enemiesSpawned_total = 0;
    private int enemiesSpawned_wave = 0;
    public float cooldown; //amount of time between waves
    private bool isActive = false;
    private bool isFinished = false;
    private float randX;
    private float randY;

    void Start(){
        enemiesSpawned_total = 0;
        enemiesSpawned_wave = 0;
        isActive = false;
        isFinished = false;
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("Player")){
            if(!isFinished && !isActive){
                isActive = true;
                StartCoroutine(Cooldown(cooldown));
            }
        }
    }

    IEnumerator Cooldown(float time){
        
        while(enemiesSpawned_total < enemyLimit){
                
            if(enemiesSpawned_wave < enemiesPerWave){
                //Debug.Log(enemiesSpawned_wave);

                randX = Random.Range(-2.5f, 2.5f);
                randY = Random.Range(-2.5f, 2.5f);
                Vector2 coords = new Vector2(randX + this.transform.position.x, randY + this.transform.position.y);
                
                LayerMask mask = LayerMask.GetMask("Bounds"); 
                Collider2D conflict = Physics2D.OverlapBox(new Vector3(coords.x, coords.y, 0), transform.localScale / 2, 0, mask);

                //maskes sure that enemies can't spawn inside walls
                if(conflict == null){
                    Instantiate(enemyType, new Vector3(coords.x, coords.y, 0), Quaternion.identity);
                    enemiesSpawned_total++;
                    enemiesSpawned_wave++;
                }
                else{
                    Debug.Log("spawn failed");
                }
                yield return new WaitForSeconds(1f);

            } else{
                yield return new WaitForSeconds(cooldown);
                enemiesSpawned_wave = 0;
                Debug.Log("Next Wave");
            }
        }

        isActive = false;
        isFinished = true;
    }
}
