using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Tnt : MonoBehaviour
{
    public List<GameObject> destroyParents;
    public ParticleSystem explosion;
    public GameManager gm;
    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }
    public void Explode()
    {
        for (int i = 0; i < destroyParents.Count; i++)
        {
            if (destroyParents[i] != null)
            {
                var myDestroyParent = destroyParents[i].GetComponent<CubeParent>();

                for (int x = myDestroyParent.transform.childCount - 1; x >= 0; x--)
                {
                    var myCube = myDestroyParent.transform.GetChild(x).GetComponent<Cube>();

                    DOVirtual.DelayedCall(4f, () => {
                        gm.EarnCoin();

                    });
                    if (myCube.coinCubes.childCount > 0)
                    {
                        for (int y = myCube.coinCubes.childCount - 1; y >= 0; y--)
                        {
                            var mySmallCube = myCube.coinCubes.GetChild(y);
                            mySmallCube.gameObject.SetActive(true);
                            mySmallCube.transform.DOJump(FindObjectOfType<Safe>().transform.position, Random.Range(6f, 10f), 1, Random.Range(2f, 3f)).SetEase(Ease.Linear);
                            mySmallCube.gameObject.AddComponent<Destroy>();
                            mySmallCube.GetComponent<Destroy>().disactive = true;
                            mySmallCube.GetComponent<Destroy>().disactiveTime = 4f;
                            mySmallCube.GetComponent<Destroy>().destroyTime = 5f;
                            mySmallCube.transform.parent = GameObject.FindGameObjectWithTag("Trash").transform;
                        }

                    }
                }



                Destroy(destroyParents[i].gameObject);

            }
        }
        explosion.Play();
        explosion.transform.parent = GameObject.FindGameObjectWithTag("Trash").transform;
        FindObjectOfType<Cam>().Shake();
        Destroy(gameObject);
    }
}
