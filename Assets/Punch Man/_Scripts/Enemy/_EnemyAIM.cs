using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class _EnemyAIM : MonoBehaviour
{
    public List<GameObject> coll = new List<GameObject>();
    [HideInInspector]public NavMeshAgent agent;

    private GameObject Player;
    private Animator demonAnime;
    [HideInInspector]public Rigidbody rb;
    
    public GameObject AnotherEnemy;
    public Slider healthMeter;
    public ParticleSystem bloodPos;


    private bool isDead = false;
    private float turnSmoothVelocity;

    public LayerMask WhatIsPlayer;
    [HideInInspector]public  float CurrentHealth;
    public float maxHealth = 75;
    public float timeBetweenAttack;
    public float sightRange, attackRange;
    public float rotationSmooth = 10;
    public float distFWal;


    public bool Boss = false, Thrower = false, dummy = false;
    private bool alreadyAttacked;
    private bool isPlayerInSightRange, isPlayerInAttackRange;


    void Awake()
    {
        Player = GameObject.Find("Hand Holder");
        agent = this.GetComponent<NavMeshAgent>();
        demonAnime = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        CurrentHealth = maxHealth;
        healthMeter.maxValue = maxHealth;
        coll[1].SetActive(false);
        coll[0].SetActive(true);
    }

    [SerializeField]float distanceFromPlayer = 0;
    private void Update()
    {
        //ColliderRemover();
        healthMeter.value = CurrentHealth;
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);
        isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
        distanceFromPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if (isDead)
        {
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        if (!isDead && !Thrower && !dummy)
        {
            if (AnotherEnemy != null)
            {
                if (isPlayerInSightRange && !isPlayerInAttackRange && AnotherEnemy.GetComponent<_EnemyAIM>().isDead) ChasePlayer();
            }
            if (AnotherEnemy == null)
            {
                if (isPlayerInSightRange && !isPlayerInAttackRange) ChasePlayer();
            }

            if (isPlayerInAttackRange && isPlayerInSightRange) AttackPlayer();
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(Player.transform.position);
        lookAtPlayer();
        //demonAnime.SetBool("run", true);
        demonAnime.SetBool("walk",true);
    }

    void lookAtPlayer()
    {
        if (agent.velocity.magnitude > 0)
        {
            float targetAngle = Mathf.Atan2(agent.velocity.x, agent.velocity.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmooth);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
    private void AttackPlayer()
    {
        //demonAnime.SetBool("run", false);
        demonAnime.SetBool("walk",false);
        lookAtPlayer();
        //agent.SetDestination(transform.localPosition);

        if (!alreadyAttacked)
        {
            //Attack code here
            //demonAnime.Play("Zombie Attack");
            demonAnime.SetTrigger("attack");
            alreadyAttacked = true;
            Invoke(nameof(resetAttack), timeBetweenAttack);
        }
    }

    private void resetAttack()
    {
        alreadyAttacked = false;
    }
    public void takeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            agent.isStopped = true;
            isDead = true;
            Die();
            //demonAnime.SetBool("Dead",true);
        }
    }
    public void Die()
    {
        _PlayerCameraController.cameraController.EnemyAIC = null;
        GameManager.GManager.EnemyKilled++;
        coll[0].SetActive(false);
        coll[1].SetActive(true);
        agent.enabled = false;
        GetComponent<Collider>().enabled = false;
        Destroy(coll[0].gameObject,0.2f);
        Destroy(coll[2].gameObject);
        healthMeter.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(!isDead)
                demonAnime.Play("Hit");

            bloodPos.Play();

            if (Boss && CurrentHealth<=2)
            {
                rb.mass = 1;
                rb.drag = 0;
                rb.angularDrag = 0.05f;
                rb.AddForce(-transform.forward * 10, ForceMode.Impulse);
            }



            if (collision.transform.parent.gameObject.transform.parent.GetComponent<_PlayerHandController>().longPress)
            {
                takeDamage(2);
            }
            else
            {
                takeDamage(1);
            }
        }
    }

}
