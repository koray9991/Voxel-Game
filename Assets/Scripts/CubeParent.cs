using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CubeParent : MonoBehaviour
{

    public bool isDestroyable = true;
    public Player matchedPlayer;
    public bool isEmpty = true;
    public float smashCubeTimer;
    public bool isInAir;
    public GameObject connectedBody;
    public GameManager gm;
    private void Start()
    {
        CheckCubes();
        gm=FindObjectOfType<GameManager>();
    }

    private void Update()
    {

        ChildCountCheck();
        MatchedPlayerCheck();

        if (isInAir)
        {
            if (connectedBody == null)
            {
                isInAir = false;
                transform.DOMoveY(0, 0.5f).SetEase(Ease.OutBounce);
                DOVirtual.DelayedCall(0.6f, () => {
                    GetComponent<BoxCollider>().enabled = true;
                    CheckCubes();

                });
            }
            else
            {
                if (!connectedBody.gameObject.activeInHierarchy)
                {
                    isInAir = false;
                    transform.DOMoveY(0, 0.5f).SetEase(Ease.OutBounce);
                    DOVirtual.DelayedCall(0.6f, () => {
                        GetComponent<BoxCollider>().enabled = true;
                        CheckCubes();

                    });
                }
            }
           
        }
      
    }
    public void ChildCountCheck()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
    public void MatchedPlayerCheck()
    {
        if (matchedPlayer != null)
        {
            smashCubeTimer += Time.deltaTime;
            if (smashCubeTimer > 2)
            {
                matchedPlayer.matchedCubeParent = null;
                matchedPlayer.isMatched = false;
                matchedPlayer.isHitting = false;
                isEmpty = true;
                matchedPlayer = null;
                smashCubeTimer = 0;
            }
        }
        else
        {
            isEmpty = true;
        }
    }
    public void CheckCubes()
    {
        for (int i = transform.childCount - 1; i > 0; i--)
        {
            if (transform.GetChild(i - 1) != null)
            {
                if (transform.GetChild(i).GetComponent<Cube>())
                {
                    transform.GetChild(i).GetComponent<Cube>().downCubePosY = transform.GetChild(i - 1).transform.position.y;
                }
               
            }
        }
    }
    public void DownCubes()
    {
         StartCoroutine(DownCubesCoroutine());

        #region
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    transform.GetChild(i).DOMoveY(transform.GetChild(i).GetComponent<Cube>().downCubePosY, 0.5f).SetEase(Ease.OutBounce);
        //    if (i == transform.childCount - 1)
        //    {
        //        DOVirtual.DelayedCall(0.6f, () => {
        //            Destroy(transform.GetChild(0).gameObject);

        //            isDestroyable = true;
        //           
        //            CheckCubes();

        //        });

        //    }

        //}
        #endregion

    }
    IEnumerator DownCubesCoroutine()
    {
     
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).DOMoveY(transform.GetChild(i).GetComponent<Cube>().downCubePosY, gm.hitTime).SetEase(Ease.OutBounce);
           
            yield return new WaitForSeconds(0.01f);
          
            if (i == transform.childCount - 1)
            {
                DOVirtual.DelayedCall(gm.hitTime-gm.hitTime/5, () => {
                Destroy(transform.GetChild(0).gameObject);
                    smashCubeTimer = 0;
                    isDestroyable = true;
                    isInAir = false;
                    CheckCubes();
                  
                });

            }

        }
    }
}
