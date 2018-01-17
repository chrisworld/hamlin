using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonController : MonoBehaviour
{

    public Transform player;
    static Animator anim;
    NavMeshAgent nav;

    //used by CombatManager
    private bool inCombat;
    private bool playerHealthDamaged;

    //managed by CombatManager
    private bool playerHasWon;
    //private bool initialised;
    private float attackDistance;
    private int viewDistance;
    private int viewAngle;
    private bool gameOver;


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

                if (playerHasWon)                                       //player has won, monster runs away
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
                    nav.destination = player.position;
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isAttacking", false);
                    anim.SetBool("isIdle", false);
                    inCombat = false;

                }
                else
                {                                                        //detected player nearby, attack, player takes damage
                    inCombat = true;
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isAttacking", true);
                    anim.SetBool("isIdle", false);
                    if (!nav.isStopped)
                    {
                      nav.ResetPath();
                      nav.isStopped = true;
                    }
                    //playerHealthDamaged = true;
                }
            }

            else                                                         //not in combat, idle
            {
                print("else ran, idling");
                inCombat = false;
                anim.SetBool("isRunning", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", false);
                anim.SetBool("isIdle", true);

    }

    }

    //Getters and Setters for CombatManager

    public bool GetInCombat() {
        return inCombat;
    }

    public bool GetPlayerHealthDamaged()
    {
        return playerHealthDamaged;
    }

    public void ResetPlayerHealthDamaged()
    {
        playerHealthDamaged = false;
    }

    public void SetPlayerHasWon(bool value)
    {
        playerHasWon = value;
    }

    public void SetGameOver(bool value)
    {
        gameOver = value;
    }

    public void SetAttackDistance(float value)
    {
        attackDistance = value;
    }

    public void SetViewDistance(int value)
    {
        viewDistance = value;
    }

    public void SetViewAngle(int value)
    {
        viewAngle = value;
    }

    public void DamagePlayer()
    {
      playerHealthDamaged = true;
      print("damage");
    }

}
