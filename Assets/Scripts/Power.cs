using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Power : MonoBehaviour
{
    public GameManager gm;
    public float initialPower;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void PowerUpgrade()
    {
        initialPower = gm.hitTime;
        gm.hitTime = 0.1f;
        for (int i = 0; i < FindObjectsOfType<Player>().Length; i++)
        {
            FindObjectsOfType<Player>()[i].mesh.material = FindObjectsOfType<Player>()[i].mats[1];
            FindObjectsOfType<Player>()[i].powerParticle.SetActive(true);
        }
   
        DOVirtual.DelayedCall(10f, () => {
            gm.hitTime = initialPower;
            for (int i = 0; i < FindObjectsOfType<Player>().Length; i++)
            {
                FindObjectsOfType<Player>()[i].mesh.material = FindObjectsOfType<Player>()[i].mats[0];
                FindObjectsOfType<Player>()[i].powerParticle.SetActive(false);
            }
            Destroy(gameObject);
        });
        gameObject.SetActive(false);

    }
}
