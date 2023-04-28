using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Player : MonoBehaviour
{
    public GameManager gm;
    public bool moving;
    Rigidbody rb;
    public float moveSpeed;
    public CubeParent matchedCubeParent;
    public bool isMatched;
    public Animator anim;
    public ParticleSystem hitParticle;

    public float startScale;
    public float currentScale;
    public float maxHealth;
    public float Health;
    public ParticleSystem blood;

     public Transform targetObject;
    [HideInInspector] public Vector3 dir;

    public bool run, idle, isHitting;

    public float nodeControlCheckTimer, nodeControlCheckTime;
    public NodeParent nodeParent;
    public int myNodeCount;

    public ParticleSystem healthBuffParticle;
    public ParticleSystem powerBuffParticle;

    public SkinnedMeshRenderer mesh;
    public Material[] mats;
    public GameObject powerParticle;
    
    private void Start()
    {
        nodeParent = FindObjectOfType<NodeParent>();
        nodeControlCheckTime = Random.Range(0.2f, 1f);
        SelectNode();
        transform.position = targetObject.position;
     
        idle = true;
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        startScale = transform.localScale.x;
      
        gm = FindObjectOfType<GameManager>();
        maxHealth = gm.health;
        Health = maxHealth;
        anim.speed = Random.Range(0.8f, 1.2f);
    }
    private void Update()
    {
        if (isMatched)
        {
            if (matchedCubeParent != null)
            {
                if (matchedCubeParent.isDestroyable)
                {
                    matchedCubeParent.DownCubes();
                    matchedCubeParent.isDestroyable = false;
                    Health -= gm.hitTime/1;
                    DOVirtual.DelayedCall(4f, () => {
                        gm.EarnCoin();

                    });
                    currentScale = Health / maxHealth;
                    if (currentScale < 0.5f)
                    {
                        currentScale = 0.5f;
                    }
                    transform.localScale =Vector3.one*1.25f*currentScale;
                    if (matchedCubeParent.transform.childCount>0)
                    {
                        matchedCubeParent.transform.GetChild(0).gameObject.SetActive(false);
                        if (matchedCubeParent.transform.GetChild(0).GetComponent<Cube>().smallCubes != null)
                        {
                            if (matchedCubeParent.transform.GetChild(0).GetComponent<Cube>().smallCubes.childCount > 0)
                            {
                                for (int i = matchedCubeParent.transform.GetChild(0).GetComponent<Cube>().smallCubes.childCount - 1; i >= 0; i--)
                                {
                                    var mySmallCube = matchedCubeParent.transform.GetChild(0).GetComponent<Cube>().smallCubes.GetChild(i);
                                    mySmallCube.gameObject.SetActive(true);
                                    mySmallCube.transform.DOJump(new Vector3(transform.position.x + Random.Range(-10f, 10f), transform.position.y, transform.position.z + Random.Range(-10f, 10f)), 3, 1, 0.7f).SetEase(Ease.Linear);
                                    mySmallCube.gameObject.AddComponent<Destroy>();
                                    mySmallCube.GetComponent<Destroy>().disactive = true;
                                    mySmallCube.GetComponent<Destroy>().disactiveTime = 0.5f;
                                    mySmallCube.GetComponent<Destroy>().destroyTime = 1.2f;
                                    mySmallCube.transform.parent = GameObject.FindGameObjectWithTag("Trash").transform;
                                }
                            }
                        }
                        if (matchedCubeParent.transform.GetChild(0).GetComponent<Cube>().coinCubes != null)
                        {
                            if (matchedCubeParent.transform.GetChild(0).GetComponent<Cube>().coinCubes.childCount > 0)
                            {
                                for (int i = matchedCubeParent.transform.GetChild(0).GetComponent<Cube>().coinCubes.childCount - 1; i >= 0; i--)
                                {
                                    var mySmallCube = matchedCubeParent.transform.GetChild(0).GetComponent<Cube>().coinCubes.GetChild(i);
                                    mySmallCube.gameObject.SetActive(true);
                                    var mySafe = FindObjectOfType<Safe>();
                                    mySmallCube.transform.DOJump(mySafe.safes[Random.Range(0,gm.safeLevel)].transform.position, Random.Range(13f,18f), 1, Random.Range(2f, 3f)).SetEase(Ease.Linear);
                                    mySmallCube.gameObject.AddComponent<Destroy>();
                                    mySmallCube.GetComponent<Destroy>().disactive = true;
                                    mySmallCube.GetComponent<Destroy>().disactiveTime = 4f;
                                    mySmallCube.GetComponent<Destroy>().destroyTime = 5f;
                                    mySmallCube.transform.parent = GameObject.FindGameObjectWithTag("Trash").transform;
                                }
                              
                            }
                        }
                        if (matchedCubeParent.transform.GetChild(0).GetComponent<Cube>().explosionParticle != null)
                        {
                            var explosion = matchedCubeParent.transform.GetChild(0).GetComponent<Cube>().explosionParticle;
                            explosion.Play();
                            //explosion.gameObject.AddComponent<Destroy>();
                            //explosion.GetComponent<Destroy>().destroyTime = 5f;
                            explosion.transform.parent = GameObject.FindGameObjectWithTag("Trash").transform;

                        }
                    }
                    if (Health <= 0)
                    {
                        blood.Play();
                        blood.transform.parent = GameObject.FindGameObjectWithTag("Trash").transform;
                        targetObject.GetComponent<Node>().isEmpty = true;
                        gm.currentPlayerCount -= 1;
                        gm.playerText.text = gm.currentPlayerCount + "/" + gm.playerCountInStart;
                        Destroy(gameObject);
                    }
                }
            }
            else
            {
                isMatched = false;
                isHitting = false;
            }

        }
        nodeControlCheckTimer += Time.deltaTime;
        if (nodeControlCheckTimer > nodeControlCheckTime)
        {
            nodeControlCheckTimer = 0;
            if (!isHitting)
            {
                for (int i = 0; i < myNodeCount; i++)
                {
                    var node = nodeParent.transform.GetChild(i).GetComponent<Node>();
                    if (node.isEmpty)
                    {
                        targetObject.GetComponent<Node>().isEmpty = true;
                        node.isEmpty = false;
                        targetObject = node.transform;
                        myNodeCount = i;
                        return;
                    }
                }
            }
        }
    }
    private void FixedUpdate()
    {
     
        if (isHitting)
        {
            Hitting();
        }
        else
        {
            if (run)
            {
                Running();
            }
            if (idle)
            {
                Idle();
            }
        }

       
        
    }
    void Hitting()
    {
        dir = (targetObject.position - transform.position).normalized * moveSpeed;
        rb.velocity = dir;
        transform.rotation = Quaternion.LookRotation(new Vector3(rb.velocity.x, 0, rb.velocity.z));
        rb.isKinematic = false;
        anim.SetBool("idle", false);
        anim.SetBool("run", false);
        anim.SetBool("hit", true);
    }
    void Idle()
    {
        rb.isKinematic = true;
        anim.SetBool("idle", true);
        anim.SetBool("run", false);
        anim.SetBool("hit", false);
        idle = true;
        run = false;
        if (Vector3.Distance(transform.position, targetObject.position) >= 0.4f)
        {
            run = true;
            idle = false;
        }
    }
    void Running()
    {
        dir = (targetObject.position - transform.position).normalized * moveSpeed;
        rb.velocity = dir;
        transform.rotation = Quaternion.LookRotation(new Vector3(rb.velocity.x,0, rb.velocity.z));
        rb.isKinematic = false;
        anim.SetBool("idle", false);
        anim.SetBool("run", true);
        anim.SetBool("hit", false);
        idle = false;
        run = true;
        if (Vector3.Distance(transform.position, targetObject.position) < 0.4f)
        {
           
            run = false;
            idle = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Tnt>())
        {
            other.GetComponent<Tnt>().Explode();
        }
        if (other.GetComponent<Power>())
        {
            other.GetComponent<Power>().PowerUpgrade();
        }
        if (other.GetComponent<MorePlayer>())
        {
            other.GetComponent<MorePlayer>().ExtraPlayer();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.GetComponent<CubeParent>())
        {
            if (!isMatched && collision.transform.GetComponent<CubeParent>().isEmpty)
            {
                var myCubeParent = collision.transform.GetComponent<CubeParent>();
                matchedCubeParent = myCubeParent;
                isMatched = true;
                myCubeParent.isEmpty = false;
                myCubeParent.matchedPlayer = this;
                isHitting = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.GetComponent<CubeParent>())
        {
            if (isMatched && !collision.transform.GetComponent<CubeParent>().isEmpty)
            {
                var myCubeParent = collision.transform.GetComponent<CubeParent>();
                matchedCubeParent = null;
                isMatched = false;
                myCubeParent.isEmpty = true;
                myCubeParent.matchedPlayer = null;
                isHitting = false;
            }
        }
    }
    public void HitParticle()
    {
        hitParticle.Play();
    }

    public void SelectNode()
    {
        var nodeParent = FindObjectOfType<NodeParent>();
        for (int i = 0; i < nodeParent.transform.childCount; i++)
        {
            var node = nodeParent.transform.GetChild(i).GetComponent<Node>();
            if (node.isEmpty)
            {
                node.isEmpty = false;
                targetObject = node.transform;
                myNodeCount = i;
                return;
            }
        }
    }
}
