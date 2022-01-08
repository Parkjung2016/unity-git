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
    public float Skill2Time;
    public float Skill2TimeFirst;
    public GameObject Skill2;
    private void Awake()
    {
        bossAttack_ = GetComponent<BossAttack>();
        Player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        Skill2Time = 10;
        Skill2TimeFirst = Skill2Time;
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
        if (Player != null)
        {

            if (knockback)
            {
                if (Vector3.Distance(Player.transform.position, transform.position) <= knockbackDis)
                {
                    Player.GetComponent<PlayerMove>().addImpact(10, 500);

                    knockback = false;
                }
            }
            if (Vector3.Distance(Player.transform.position, transform.position) > knockbackDis && IsHeal)
            {
                knockback = true;
            }
            if (!IsHeal)
            {
                knockback = false;
            }
        }
        Skill2Time -= 0.01f;
        if(Skill2Time <= 0)
        {
            Skill2Time = Skill2TimeFirst;
            GameObject obj = Instantiate(Skill2, Player.transform.position, Quaternion.identity);
            StartCoroutine(SkillDestroy(obj));
        }
    }
    IEnumerator SkillDestroy(GameObject obj)
    {
        yield return new WaitForSeconds(7);
        Destroy(obj);
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
            BossHPManager.Instance.Sword.GetComponent<BossWeapon>().Damage = 25;
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
