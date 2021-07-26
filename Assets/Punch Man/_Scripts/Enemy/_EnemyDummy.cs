using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _EnemyDummy : MonoBehaviour
{
    public List<GameObject> coll = new List<GameObject>();
    public Slider healthMeter;
    public ParticleSystem bloodPos;


    private bool isDead = false;
    [HideInInspector] public float CurrentHealth;
    public float maxHealth = 75;

    private void Start()
    {
        CurrentHealth = maxHealth;
        healthMeter.maxValue = maxHealth;
        coll[1].SetActive(false);
        coll[0].SetActive(true);
    }
    private void Update()
    {
        healthMeter.value = CurrentHealth;
    }
    public void takeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            isDead = true;
            Die();
        }
    }

    public void Die()
    {
        coll[0].SetActive(false);
        coll[1].SetActive(true);
        GetComponent<Collider>().enabled = false;
        Destroy(coll[0].gameObject, 0.2f);
        Destroy(coll[2].gameObject);
        healthMeter.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isDead)
        {
            bloodPos.Play();
            takeDamage(1);
        }
    }
}
