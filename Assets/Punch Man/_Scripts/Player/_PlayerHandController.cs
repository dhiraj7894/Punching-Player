using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerHandController : MonoBehaviour
{
    private Animator handAnimation;
    [SerializeField] private bool HandChecker;
    [HideInInspector] public bool longPress = false;
    [SerializeField] private float downPressTime;
    [SerializeField] private float longPressTime;
                                  private float force = 0;

    public Collider LeftGlove;
    public Collider RightGlove;
    public ParticleSystem superEffect;
    void Start()
    {
        handAnimation = GetComponent<Animator>();
        desableCollider();
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
                force = 0;
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
                force = 30;
                downPressTime = 0;
                handAnimation.Play("PowerHit");
                superEffect.Stop();
            }
        }


    }

    public void enableCollider()
    {
        LeftGlove.enabled = true;
        RightGlove.enabled = true;
    }
    public void desableCollider()
    {
        LeftGlove.enabled = false;
        RightGlove.enabled = false;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (longPress)
        {
            other.transform.GetComponent<_EnemyAIM>().CurrentHealth -= 2;
        }
        other.transform.GetChild(0).GetComponent<Animator>().Play("Hit");
        //other.transform.GetComponent<Rigidbody>().AddForce(-other.transform.forward * force, ForceMode.Impulse);
    }
}
