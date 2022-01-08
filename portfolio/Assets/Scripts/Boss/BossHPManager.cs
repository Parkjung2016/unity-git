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
    bool fadeplay;
    PlayerAttack playerAttack;
    private void Awake()
    {
        playerAttack = GameObject.FindObjectOfType<PlayerAttack>();
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
            if (!fadeplay)
            {
                fadeplay = true;
                GameManager.instace.Fade.gameObject.SetActive(true);
                GameManager.instace.Fade.Play();
                GameManager.instace.VCam2.SetActive(true);
                GameManager.instace.PlayerCam.SetActive(false);
                StartCoroutine( callfinishfade());
            }
        }

    }
    IEnumerator callfinishfade()
    {
        yield return new WaitForSeconds(4);
        GameManager.instace.FinishFade();
    }
    public static void ApplyDamage(int Damage)
    {
        if (Instance.Death)
            return;
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
        if(other.CompareTag("SwordParticle") && playerAttack.FinalAttackCol)
        {
            ApplyDamage(15);
            GameManager.instace.StartCoroutine(GameManager.CameraShake(GetComponentInParent<Animator>(), anim, 1f));
            playerAttack.FinalAttackCol = false;
        }
    }
}
