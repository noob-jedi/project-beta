using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour {

    [System.Serializable]
    // Loot or item
    public class Loot
    {
        public string name;
        public GameObject item;
        public float dropChance;
    }

    public List<Loot> lootTable = new List<Loot>();
    public AI ai;
    
    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DropLoot(GameObject enemy)
    {
        int roll = Random.Range(0, 100);
        float cascadedChance = 0;
        foreach (Loot l in lootTable)
        {
            cascadedChance += l.dropChance;
            if(roll < cascadedChance)
            {
                //return l;
                Debug.Log("roll = " + roll + ", cascadedChance = " + cascadedChance);
                Instantiate(l.item, enemy.transform.position, enemy.transform.rotation);
                break;
            }
        }
        //return new Loot();
    }
}
