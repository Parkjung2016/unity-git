using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class BossHPManager : MonoBehaviour
{
    NavMeshAgent Nav;
    public int Hp;
    public int MaxHp;
    public static BossHPManager Instance = null;
    Animator anim;
    public bool Death;
    public GameObject Sword;
    BossAttack bossAttack_;
    BossSkillManager bossSkillManager_;
    private void Awake()
    {
        bossSkillManager_ = GetComponent<BossSkillManager>();
        Nav = GetComponent<NavMeshAgent>();
        bossAttack_ = GetComponent<BossAttack>();
        anim = GetComponent<Animator>();
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        Sword.GetComponent<MeshCollider>().enabled = false;
        Hp = 200;
        MaxHp = Hp;
    }
    private void Update()
    {
        Hp = Mathf.Clamp(Hp, 0, MaxHp);
        if (Hp == 0)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                anim.Play("Death");
            if (Death)
                return;
            Sword.transform.parent = null;
            Sword.AddComponent<Rigidbody>();
            Destroy(Nav);
            Destroy(Sword.GetComponent<Collider>());
            Sword.GetComponent<MeshCollider>().enabled = true;
            Death = true;
            Destroy(GetComponent<BossSkillManager>());
        }

    }
    public static void ApplyDamage(int Damage)
    {
        Instance.bossAttack_.ComboReset();
        Instance.Hp -= Damage;

        if (Instance.Hp <= Instance.bossSkillManager_.HealtriggerAmount && !Instance.bossSkillManager_.IsHeal)
        {
            if(Instance.bossSkillManager_ != null)
            Instance.bossSkillManager_.StartCoroutine(Instance.bossSkillManager_.HealCheck());
        }
        Instance.anim.SetTrigger("Hit");
    }
    public static void Heal(int amount)
    {
        Instance.Hp += amount;
    }
    private void OnParticleCollision(GameObject other)
    {
        if(other.CompareTag("SwordParticle"))
        {
            ApplyDamage(15);
        }
    }
}
