using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _EnemyAnimation : MonoBehaviour
{
    public GameObject dummy;
    private Rigidbody dummyRb;

    public float force;

    private void Start()
    {
        dummyRb = dummy.transform.GetComponent<Rigidbody>();
    }
    public void throwDummy()
    {
        dummyRb.AddForce(transform.forward * force, ForceMode.Impulse);
        StartCoroutine(enableD(0.1f));
    }

    IEnumerator enableD(float t)
    {
        yield return new WaitForSeconds(t);
        dummy.GetComponent<Collider>().enabled = true;
        dummyRb.constraints = RigidbodyConstraints.None;
    }

    public IEnumerator enableS(float t)
    {
        yield return new WaitForSeconds(t);
        FindObjectOfType<_EnemyThrower>().enemyAi.enabled = true;
        FindObjectOfType<_EnemyThrower>().rb.isKinematic = false;
        FindObjectOfType<_EnemyThrower>().enemyAi.Thrower = false;
        FindObjectOfType<_EnemyThrower>().enemyAi.agent.enabled = true;        
        FindObjectOfType<_EnemyThrower>().enabled = false;        
    }
}
