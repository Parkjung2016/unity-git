using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    Animator anim;
    bool combopossible;
   BossMove BossMove_;
    public bool attacking;
    public int Damage;
    BossWeapon bossWeapon;
    private void Awake()
    {
        bossWeapon = GetComponentInChildren<BossWeapon>();
        BossMove_ = GetComponent<BossMove>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (BossHPManager.Instance.Death)
            return;
        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Hit"))
            ComboReset();
    }
    public void Attack()
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsTag("Hit") || BossHPManager.Instance.Death || anim.GetCurrentAnimatorStateInfo(0).IsTag("Heal"))
        {
            ComboReset();
            return;
        }
        anim.applyRootMotion = true;
        if (anim.GetInteger("AttackCombo") == 0)
        {
            anim.SetInteger("AttackCombo", 1);
            bossWeapon.col.enabled = true;
            anim.Play("Attack");
            
        }
        anim.SetBool("Attacking", true);

    }
    public void ComboPossible()
    {
        if (BossHPManager.Instance.Death)
            return;
        if (BossMove_.Distance() > BossMove_.attackDis)
            return;
        combopossible = true;
    }
    public void Combo()
    {
        if (BossHPManager.Instance.Death)
            return;
        if (anim.GetInteger("AttackCombo") != 0)
        {
            if (combopossible)
            {
                combopossible = false;
                anim.SetInteger("AttackCombo", anim.GetInteger("AttackCombo") + 1);
                bossWeapon.col.enabled = true;
            }
        }
    }
    public void ComboReset()
    {
        if (BossHPManager.Instance.Death)
            return;
        combopossible = false;
        anim.SetInteger("AttackCombo", 0);
        attacking = false;
        anim.SetBool("Attacking", false);
        bossWeapon.col.enabled = false;
    }
}
