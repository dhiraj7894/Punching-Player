using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerCameraController : MonoBehaviour
{
    public static _PlayerCameraController cameraController;
    private Rigidbody rb;
    public GameObject EnemyAIC;
    public Transform Parant;

    public bool enemyIsInRange;
    public bool isPlayerPunchLevel = false;
    public float speed;

    public float AimRange;
    public float AimRedius;
    public float lookSpeed;

    
    public float DistanceFromPlayer;

    public LayerMask EnemyMask;

    private float turnSmoothVelocity;
    private float rotationSmooth = 0.1f;
    private void Start()
    {
        rb=GetComponent<Rigidbody>();
        cameraController = this;
    }
    private void FixedUpdate()
    {
        if (!isPlayerPunchLevel)
        {
            movement();
        }
        lookEnemy();
        raycastSensor();
        
    }
    void raycastSensor()
    {
        RaycastHit hit;
        Ray ray = new Ray(Parant.position, Parant.forward);
        if(Physics.SphereCast(ray,AimRedius,out hit, AimRange, EnemyMask))
        {
                EnemyAIC = hit.transform.GetChild(2).gameObject;
        }

        if (EnemyAIC != null)
        {
            DistanceFromPlayer = Vector3.Distance(Parant.position, EnemyAIC.transform.position);
            if (DistanceFromPlayer >= 4)
            {
                EnemyAIC = null;
            }
        }
    }


    private Coroutine LookCo;

    void lookEnemy()
    {
        if (EnemyAIC != null)
        {
            Vector3 direction = EnemyAIC.transform.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction).normalized;
            Quaternion lookAt = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
            transform.rotation = lookAt;
        }
        if (EnemyAIC == null)
        {
            Vector3 direction = Vector3.zero;
            Quaternion targetRotation = Quaternion.LookRotation(direction).normalized;
            Quaternion lookAt = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookSpeed);
            transform.rotation = lookAt;
        }
    }

    void movement()
    {
        if (transform.position == GameManager.GManager.waypoint[GameManager.GManager.moveTowardPoint].position)
        {
            speed = 0;
        }
        if (transform.position != GameManager.GManager.waypoint[GameManager.GManager.moveTowardPoint].position)
        {
            speed = 2.5f;
            transform.position = Vector3.MoveTowards(transform.position, GameManager.GManager.waypoint[GameManager.GManager.moveTowardPoint].position, speed * Time.deltaTime);
            if (rb.velocity.magnitude > 0)
            {
                float targetAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmooth);
                transform.rotation = Quaternion.Euler(0, angle, 0);
            }
        }
    }

    public void desableAnimation()
    {
        transform.GetComponent<Animator>().Play("None");

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("win"))
        {
            transform.GetChild(0).GetComponent<Animator>().Play("Enjoy");
            GameManager.GManager.Win = true;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Parant.position, AimRedius);
    }
}
