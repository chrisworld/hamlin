using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{

    public Transform player;
    static Animator anim;

    //used by CombatManager
    public bool inCombat = false;

    //managed by CombatManager        
    public bool playerHasWon = false;
    public int attackDistance;
    public int viewDistance;
    public int viewAngle;



    void Start()
    {
        anim = GetComponent<Animator>();

        //"not initialised" values, initialised by CombatManager
        attackDistance = -1;
        viewDistance = -1;
        viewAngle = -1;
    }

    // Update is called once per frame
    void Update()
    {

        if(attackDistance != -1 && viewDistance != -1 && viewAngle != -1)
        {

            Vector3 direction = player.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);

            print(Vector3.Distance(player.position, this.transform.position));
            print(angle);

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

    }

}
