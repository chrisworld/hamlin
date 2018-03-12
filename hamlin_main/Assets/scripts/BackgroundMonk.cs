﻿using UnityEngine;
using System.Collections;
using UnityEngine.AI;
 
//super basic logic to make monks wander around randomly

public class BackgroundMonk : MonoBehaviour
{

  public float wanderRadius;
  public float wanderTimer;

  Animator anim;

  private Transform target;
  private NavMeshAgent agent;
  private float timer;

  // Use this for initialization
  void OnEnable()
  {
    agent = GetComponent<NavMeshAgent>();
    timer = wanderTimer;
    anim = GetComponentInChildren<Animator>();
  }

  // Update is called once per frame
  void Update()
  {
    timer += Time.deltaTime;

    if (timer >= wanderTimer)
    {
      Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
      agent.SetDestination(newPos);
      anim.SetBool("isWalking", true);
      timer = 0;
    }

    //if(agent.velocity.magnitude == 0){
      //anim.SetBool("isWalking", false);
    //}

  }

  public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
  {
    Vector3 randDirection = Random.insideUnitSphere * dist;

    randDirection += origin;

    NavMeshHit navHit;

    NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

    return navHit.position;
  }
}
