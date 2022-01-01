using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillManager : MonoBehaviour
{
    Animator anim;
    public bool IsHeal;
    public int HealtriggerAmount;
    public int HealAmount;
    public bool knockback;
    public float knockbackDis;
    GameObject Player;
    BossAttack bossAttack_;
    private void Awake()
    {
        bossAttack_ = GetComponent<BossAttack>();
        Player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        IsHeal = false;
    }
    
    private void Update()
    {
        if (BossHPManager.Instance.Death)
            return;
        if(anim.GetCurrentAnimatorStateInfo(0).IsTag("Heal") || BossHPManager.Instance.Death)
        {
            StopCoroutine(HealCheck());
        }
        if(knockback)
        {
            if(Vector3.Distance(Player.transform.position,transform.position) <=knockbackDis)
            {
                Player.GetComponent<PlayerMove>().addImpact(10,500);

                knockback = false;
            }
        }
        if (Vector3.Distance(Player.transform.position, transform.position) > knockbackDis && IsHeal)
        {
            knockback = true;
        }
        if(!IsHeal)
        {
            knockback = false;
        }
    }
   
    IEnumerator ResetHealCount()
    {
        yield return new WaitForSeconds(5);
        IsHeal = false;
        knockback = false;
        bossAttack_.ComboReset();
        yield return null;
    }
    public IEnumerator HealCheck()
    {

        yield return new WaitForSeconds(3);
        if (BossHPManager.Instance.Death)
            yield return null;
        if (Random.Range(1,100) <= HealtriggerAmount)
        {
            IsHeal = true;
            anim.Play("Heal");
            knockback = true;
            StartCoroutine(ResetHealCount());
            yield return null;
        }
    }
    public void Heal()
    {
        BossHPManager.Heal(BossHPManager.Instance.Hp*(1+HealAmount/100));
    }
}
