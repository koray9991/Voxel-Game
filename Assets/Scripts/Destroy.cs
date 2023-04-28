using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    public float destroyTime=1;
    public bool disactive;
    public float disactiveTimer;
    public float disactiveTime;
    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
    private void Update()
    {
        if (disactive)
        {
            disactiveTimer += Time.deltaTime;
            if (disactiveTimer > disactiveTime)
            {
                gameObject.SetActive(false);
                disactive = false;
            }
        }
     
    }
}
