using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{

    public Transform player;
    static Animator anim;

    //used by CombatManager
    private bool inCombat = false;

    //managed by CombatManager
    private bool playerHasWon;
    //private bool initialised;
    private float attackDistance;
    private int viewDistance;
    private int viewAngle;


    void Start()
    {
        anim = GetComponent<Animator>();
        playerHasWon = false;
        //initialised = false;
    }

    // Update is called once per frame
    void Update()
    {


            Vector3 direction = player.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);

            print(Vector3.Distance(player.position, this.transform.position));
            print(angle);

        //Now, with NavMesh it's even easier. Every second just do a raycast to see if you can see the player and then set the target position to the player's position and NavMesh will take care of object avoidance automatically.
            if (Vector3.Distance(player.position, this.transform.position) < viewDistance && angle < viewAngle)
            {

                direction.y = 0;

                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.05f);
            

                anim.SetBool("isIdle", false);
                if (playerHasWon)                                       //player has won, monster runs away
                {
                    //need a translate here to make it change direction
                    anim.SetBool("isRunning", true);
                }
                if (direction.magnitude > attackDistance)               //detected player but too far away to attack, walk towards
                {
                    inCombat = true;
                    this.transform.Translate(0, 0, 0.1f);
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isAttacking", false);

                }
                else
                {                                                        //detected player nearby, attack
                    inCombat = true;
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isAttacking", true);
                }
            }

            else                                                         //not in combat, idle
            {
                inCombat = false;
                //TODO: win state needs to be reset
                anim.SetBool("isIdle", true);
                anim.SetBool("isWalking", false);
                anim.SetBool("isAttacking", false);

            }

    }

    //Getters and Setters for CombatManager

    public bool GetInCombat() {
        return inCombat;
    }

    public void SetPlayerHasWon(bool value)
    {
        playerHasWon = value;
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


}
