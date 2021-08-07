using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _EnemyAnimation : MonoBehaviour
{
    public GameObject parantObj;

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

    public void enableS(float t)
    {
        parantObj.GetComponent<_EnemyAIM>().agent.enabled = true;        
        parantObj.GetComponent<_EnemyAIM>().enabled = true;
        parantObj.GetComponent<_EnemyThrower>().rb.isKinematic = false;
        parantObj.GetComponent<_EnemyAIM>().Thrower = false;
        parantObj.GetComponent<_EnemyThrower>().enabled = false;        
    }
}
