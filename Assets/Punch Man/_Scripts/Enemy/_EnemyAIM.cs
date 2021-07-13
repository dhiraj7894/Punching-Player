using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class _EnemyAIM : MonoBehaviour
{
    /*private NavMeshAgent agent;

    private GameObject Player;
    private Animator EnemyAI;
    private Rigidbody rb;
    private bool isDead = false;
    private float turnSmoothVelocity;

    [SerializeField]private float distanceFromPlayer;

    public float maxHealth = 75;
    public float rotationSmooth = 10;
    public float stoppingDistance;


    [HideInInspector]public float CurrentHealth;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Player = GameObject.Find("Hand Holder");
        EnemyAI = transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        distanceFromPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if(distanceFromPlayer>= stoppingDistance)
        {
            this.agent.SetDestination(Player.transform.position);
            EnemyAI.Play("Running");

        }
        if (distanceFromPlayer < stoppingDistance)
        {
            agent.SetDestination(transform.position);
            EnemyAI.Play("Punching");
           // transform.LookAt(Player.transform.position);
        }


        if (agent.velocity.magnitude > 0)
        {
            float targetAngle = Mathf.Atan2(agent.velocity.x, agent.velocity.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmooth);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }*/

    public List<GameObject> coll = new List<GameObject>();
    private NavMeshAgent agent;

    private GameObject Player;
    private Animator demonAnime;
    private Rigidbody rb;
    private bool isDead = false;
    private float turnSmoothVelocity;

    public LayerMask WhatIsPlayer;
    public float CurrentHealth;
    public float maxHealth = 75;
    public float WalkPointRange;
    public float timeBetweenAttack;
    public float sightRange, attackRange;
    public float rotationSmooth = 10;

    [SerializeField] private bool walkPointSet;
    [SerializeField] private bool alreadyAttacked;
    [SerializeField] private bool isPlayerInSightRange, isPlayerInAttackRange;
    [SerializeField] private Vector3 walkPoint;


    void Awake()
    {
        Player = GameObject.Find("Hand Holder");
        agent = this.GetComponent<NavMeshAgent>();
        demonAnime = transform.GetChild(0).GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        CurrentHealth = maxHealth;

        coll[1].SetActive(false);
        coll[0].SetActive(true);
    }

    [SerializeField]float distanceFromPlayer = 0;
    private void Update()
    {
        isPlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);
        isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
        distanceFromPlayer = Vector3.Distance(transform.position, Player.transform.position);
        if (isDead)
        {
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        if (!isDead)
        {
            if (!isPlayerInAttackRange && !isPlayerInSightRange) Petrolling();
            if (isPlayerInSightRange && !isPlayerInAttackRange) ChasePlayer();
            if (isPlayerInAttackRange && isPlayerInSightRange) AttackPlayer();
        }
    }

    private void Petrolling()
    {
        if (!walkPointSet) searchForPoint();

        if (walkPointSet)
        {
            this.agent.SetDestination(walkPoint);
            //demonAnime.SetBool("run", true);
            demonAnime.SetBool("walk",true);
            lookAtWalkPoint();
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    private void searchForPoint()
    {
        float randomZ = Random.Range(-WalkPointRange, WalkPointRange);
        float randomX = Random.Range(-WalkPointRange, WalkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, 0, transform.position.z + randomZ);

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

    public void lookAtWalkPoint()
    {
        if (agent.velocity.magnitude > 0)
        {
            float targetAngle = Mathf.Atan2(agent.velocity.x, agent.velocity.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmooth);
            transform.rotation = Quaternion.Euler(0, angle, 0);
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
        coll[0].SetActive(false);
        coll[1].SetActive(true);
        /*Destroy(this.gameObject, 0.5f);*/
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
            demonAnime.Play("Hit");
            takeDamage(1);
        }
            
    }
}
