using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPManager : MonoBehaviour
{
    public int HP;
    Animator anim;
    public int maxHP;
    public bool death;
    public GameObject Sword;
    PlayerAnim playerAnim_;
    public static  PlayerHPManager Instance = null;
    private void Awake()
    {
        playerAnim_ = GetComponent<PlayerAnim>();
        anim = GetComponent<Animator>();
        if(Instance == null)
        {
            Instance = this;
        }
    }
    
    private void Update()
    {
        HP = Mathf.Clamp(HP,0, maxHP);

        if(HP == 0)
        {
            if (death)
                return;
            Sword.transform.parent = null;
            Destroy(Sword.GetComponent<Collider>());
            Sword.GetComponent<MeshCollider>().enabled = true;
            Sword.AddComponent<Rigidbody>();
            anim.Play("Death");
            death = true;
            anim.applyRootMotion = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Sword.GetComponent<MeshCollider>().enabled = false;
        HP = 100;
        maxHP = HP;
    }

    public static void ApplyDamage(int Damage)
    {
        if (Instance.anim.GetCurrentAnimatorStateInfo(0).IsTag("Roll") || Instance.anim.GetCurrentAnimatorStateInfo(0).IsTag("Knock"))
            return;
        Instance.anim.applyRootMotion = true;
        if(Instance.playerAnim_.Block && PlayerBlockManager.Instance.Block != 0)
        {
           Instance.anim.Play("Block");
            PlayerBlockManager.ApplyBlockDamage(Damage);
        }
        else
        {
                Instance.anim.Play("Hit");
        Instance.HP -= Damage;
        }
    }

}
