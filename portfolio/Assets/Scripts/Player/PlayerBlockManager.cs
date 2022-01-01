using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockManager : MonoBehaviour
{
    public int Block;
    Animator anim;
    public int MaxBlock;
    PlayerAnim playerAnim_;
    public static PlayerBlockManager Instance = null;
    private void Awake()
    {
        playerAnim_ = GetComponent<PlayerAnim>();
        anim = GetComponent<Animator>();
        if (Instance == null)
        {
            Instance = this;
        }
    }
    private void Start()
    {
        Block = 100;
        MaxBlock = Block;
    }
    // Update is called once 3eper frame
    void Update()
    {
        Block = Mathf.Clamp(Block, 0, MaxBlock);
    }
    public static void ApplyBlockDamage(int Damage)
    {
        Instance.anim.applyRootMotion = true;
        Instance.Block -= (int)(Damage * 0.8f);
    }
}
