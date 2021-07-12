using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerHandController : MonoBehaviour
{
    private Animator handAnimation;
    [SerializeField] private bool HandChecker;
    [SerializeField] private bool longPress = false;
    [SerializeField] private float downPressTime;
    [SerializeField] private float longPressTime;
    private float force = 0;

    public ParticleSystem superEffect;
    void Start()
    {
        handAnimation = GetComponent<Animator>();
    }
    void Update()
    {
        handA();
    }
    void handA()
    {
        if (Input.GetMouseButton(0))
        {
            downPressTime += Time.deltaTime;
            if (downPressTime > longPressTime)
            {
                longPress = true;
                handAnimation.Play("Long");
                superEffect.Play();
            }
            else
            {
                longPress = false;
            }

        }

        if (Input.GetMouseButtonUp(0))
        {
            if (!longPress)
            {
                force = 100;
                downPressTime = 0;
                if (handAnimation.GetCurrentAnimatorStateInfo(0).IsName("Left") && handAnimation.GetCurrentAnimatorStateInfo(0).IsName("Right"))
                    return;

                if (!HandChecker)
                {
                    HandChecker = true;
                }
                else if (HandChecker)
                {
                    HandChecker = false;
                }

                if (HandChecker)
                    handAnimation.Play("Right");

                if (!HandChecker)
                    handAnimation.Play("Left");
            }
            if (longPress)
            {
                force = 10000;
                downPressTime = 0;
                handAnimation.Play("PowerHit");
                superEffect.Stop();
            }
        }


    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.collider.name+" : Hit");
        other.transform.GetComponent<Rigidbody>().AddForce(-other.transform.forward * force);
    }

}
