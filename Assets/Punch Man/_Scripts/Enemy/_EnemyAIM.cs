using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class _EnemyAIM : MonoBehaviour
{
    private NavMeshAgent agent;

    private GameObject Player;
    private Animator EnemyAI;
    private bool isDead = false;
    private float turnSmoothVelocity;

    public float maxHealth = 75;
    public float rotationSmooth = 10;


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
        this.agent.SetDestination(Player.transform.position);
        EnemyAI.Play("Running");
        if (agent.velocity.magnitude > 0)
        {
            float targetAngle = Mathf.Atan2(agent.velocity.x, agent.velocity.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmooth);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}
