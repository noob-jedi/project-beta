using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour {

    float playerDamage = 25f;
    public LootTable lootTable;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("c"))
        {
            // Execute Attack
            Attack();
        }
    }
    
    // Simulate AOE attack at first
    public void Attack()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == ("AITag"))
        {
            AI ai = col.gameObject.GetComponent<AI>();
            if(ai.enemyHealth > 20)
            {
                ai.enemyHealth -= 20;
            }
            else
            {
                Destroy(col.gameObject);
                lootTable.DropLoot(col.gameObject);
            }

        }
    }

}
