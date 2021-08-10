using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class bag : MonoBehaviour
{
    public ParticleSystem Sparks;
    public GameObject FloatingT;

    public Rigidbody[] planks;
    public float maxHealth;
    public float currentHealth;
    public float Force = 10;
    void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            GameManager.GManager.Win = true;
        }
    }
    public void SpawnText()
    {
        if (currentHealth > -1)
        {
           GameObject FT = Instantiate(FloatingT, transform.position, Quaternion.identity);
            FT.GetComponent<TextMeshPro>().text = currentHealth.ToString();
        }
        if (currentHealth <= 0)
        {
            Destroy(GetComponent<SpringJoint>());
            transform.GetComponent<Rigidbody>().AddForce(Vector3.forward * Force, ForceMode.Impulse);
            transform.GetComponent<Rigidbody>().AddForce(Vector3.right * (Force/4), ForceMode.Impulse);

            foreach(Rigidbody rb in planks)
            {
                rb.isKinematic = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Sparks.Play();
            SpawnText();
            currentHealth--;
        }

    }

}
