using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : MonoBehaviour
{

    public Transform player;

    [HideInInspector]
    public static Animator anim;
    [HideInInspector]
    public NavMeshAgent nav;

    //used by CombatManager
    [HideInInspector]
    public bool inCombat;
    [HideInInspector]
    public bool playerHealthDamaged;

    //managed by CombatManager
    [HideInInspector]
    public bool playerHasWon;
    [HideInInspector]
    public float attackDistance;
    [HideInInspector]
    public int viewDistance;
    [HideInInspector]
    public int viewAngle;
    [HideInInspector]
    public bool gameOver;
    [HideInInspector]
    public string status;


    void Start()
    {
        anim = GetComponent<Animator>();
        playerHasWon = false;
        nav = GetComponent<NavMeshAgent>();
        inCombat = false;
        playerHealthDamaged = false;
        gameOver = true;    //start with skeleton disabled until player presses a key
    }

    // Update is called once per frame
    void Update()
    {


            Vector3 direction = player.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);

            if (!gameOver && Vector3.Distance(player.position, this.transform.position) < viewDistance && angle < viewAngle)
            {
                   //direction.y = 0;
                   //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.05f);

                if (status == "run away")                                       //player has won
                {
                    transform.LookAt(new Vector3(player.transform.position.x, 0, player.transform.position.z));
                    nav.destination = -direction;
                    anim.SetBool("isRunning", true);
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isIdle", false);
                    inCombat = false;
                    gameOver = true;
                }
                else if (direction.magnitude > attackDistance)               //detected player but too far away to attack, walk towards
                {
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isIdle", false);
                    nav.destination = player.position;
                    inCombat = false;

                }
                else
                {                                                        //detected player nearby, activate combat
                    inCombat = true;
                    //TODO: TURN TO FACE PLAYER. transform.LookAt?
                    transform.LookAt(player);
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isWalking", false);
                    if (!nav.isStopped)
                    {
                      nav.ResetPath();
                      nav.isStopped = true;
                    }
                    bool isAttacking = (status == "attack") ? true : false;
                    anim.SetBool("isAttacking", isAttacking);
                    anim.SetBool("isIdle", !isAttacking);
                    if(status == "damaged"){
                      //TODO: player successfully attacked with note. Knockback animation?                          
                    }
                }
            }

            else                                                         //not in combat, idle
            {
                inCombat = false;
                anim.SetBool("isRunning", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", false);
                anim.SetBool("isIdle", true);
            }

    }

    //Used by animation event to time health damage
    public void DamagePlayer()
    {
      playerHealthDamaged = true;
      status = "idle";     //only attack once per wrong note
    }

}
