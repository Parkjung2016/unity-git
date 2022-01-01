using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class BossMove : MonoBehaviour
{
    Animator anim;
    public GameObject Player;
    public float moveDis;
    public float attackDis;
    bool Move;
    bool AttackTrue;
    NavMeshAgent Nav;
    public float turnspeed;
    BossAttack bossAttack_;
    private void Awake()
    {
        bossAttack_ = GetComponent<BossAttack>();
        anim = GetComponent<Animator>();
        Nav = GetComponent<NavMeshAgent>();
    }
    public void FindPlayer()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    public float Distance()
    {
        return Vector3.Distance(transform.position, Player.transform.position);

    }
    // Update is called once per frame
    void Update()
    {
        if(Player == null|| BossHPManager.Instance.Death) 
        {
            return;
        }
        Move =  Distance() <= moveDis && Distance() > attackDis;
        anim.SetBool("Move", Move);
        if (Move)
        {
            Nav.isStopped = false;
            Nav.updatePosition = true;
            Nav.updateRotation = true;
            Nav.destination = Player.transform.position;

        }
        else if (Distance() <= attackDis)
        {
            if (!bossAttack_.attacking)
            {
                bossAttack_.Attack();
            }
        }
        else
        {
            if (!Nav.isStopped)
            {
                Nav.isStopped = true;
                Nav.updatePosition = false;
                Nav.updateRotation = false;
                Nav.velocity = Vector3.zero;

            }

        }
        if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
        {
            Nav.isStopped = true;
            Nav.updatePosition = false;
            Nav.updateRotation = false;
            Nav.velocity = Vector3.zero;
        }
    }
}
