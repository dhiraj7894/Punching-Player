using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _EnemyThrower : MonoBehaviour
{
    public GameObject Target;
    [HideInInspector]public Rigidbody rb;

    private bool isPlayerInSightRange;

    public _EnemyAIM enemyAi;
    public float sightRange;
    public Animator anime;
    private void Start()
    {
        enemyAi.enabled = false;
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
    }

    private void Update()
    {
        isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, FindObjectOfType<_EnemyAIM>().WhatIsPlayer);

        if (isPlayerInSightRange)
        {
            transform.LookAt(Target.transform.position);
            StartCoroutine(throwD(0.2f));
        }
    }



    IEnumerator throwD(float t){

        yield return new WaitForSeconds(t);
        anime.SetTrigger("throw");        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


}
