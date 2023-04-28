using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
public class Safe : MonoBehaviour
{
    public GameObject coin;
    public Transform[] coinSpawnPoint;
    public float x, z;
    public GameManager gm;
    public GameObject[] safes;
    public LeanGameObjectPool coinPool;
    
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }
    public void CoinSpawn()
    {
        for (int i = 0; i < gm.safeLevel; i++)
        {

           
           GameObject myCoin = coinPool.Spawn(coinSpawnPoint[Random.Range(0, gm.safeLevel)].position,Quaternion.identity,GameObject.FindGameObjectWithTag("Trash").transform);
           
   
              
            
           
            myCoin.GetComponent<Coin>().x = x;
            myCoin.GetComponent<Coin>().z = z;
           
            coinPool.Despawn(myCoin, 15);

            gm.EarnCoinValue(1);
        }
       
    }
}
