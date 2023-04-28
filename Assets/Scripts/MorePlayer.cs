using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorePlayer : MonoBehaviour
{
    public int playerCount;
    GameManager gm;
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }
    public void ExtraPlayer()
    {
        for (int i = 0; i < playerCount; i++)
        {
            gm.ExtraPlayer();
        }
        Destroy(gameObject);
    }
}
