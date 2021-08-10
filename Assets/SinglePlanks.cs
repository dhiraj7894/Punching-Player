using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlanks : MonoBehaviour
{
    public ParticleSystem cloud;

    private void OnCollisionEnter(Collision collision)
    {
        cloud.Play();
    }
}
