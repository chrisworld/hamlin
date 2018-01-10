using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonController : MonoBehaviour
{

    public int attackDistance = 10;  //walk towards player if player detected and further away from this distance, else attack player

    //these define the space in which the monster can detect the player
    public int viewDistance = 20;
    public int viewAngle = 30;

    public Transform player;
    static Animator anim;
    private ScaleListener scaleListener;
    private bool inCombat = false;          //used in ScaleListener

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        scaleListener = GetComponent<ScaleListener>();
    }

    // Update is called once per frame
    void Update()
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
            if (scaleListener.GetWinState())                                 //player has won, monster runs away
            {

                print("win state detected");
                inCombat = false;
                scaleListener.ResetWinState();
            }
            if (direction.magnitude > attackDistance)                                   //detected player but too far away to attack, walk towards
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

        else                                                                //not in combat, idle
        {
            scaleListener.ResetWinState();
            inCombat = false;
            anim.SetBool("isIdle", true);
            anim.SetBool("isWalking", false);
            anim.SetBool("isAttacking", false);

        }

    }

    //Used in ScaleListener
    public bool GetCombatStatus()
    {
        return inCombat;
    }

}
