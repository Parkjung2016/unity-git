using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShieldEquip : MonoBehaviour
{
	public Transform Shield, Shield_ueq, Shield_eq;
	public bool Shield_is_equipped;
	PlayerAttack playerAttack_;
	Animator anim;

	void Awake()
	{
		playerAttack_ = GetComponent<PlayerAttack>();
		anim = GetComponent<Animator>();
	}

    private void Start()
    {
		Shield_is_equipped = true;
		anim.SetBool("Shield", true);
    }
    void Update()
	{
		if (PlayerHPManager.Instance.death)
		{
			return;
		}
		if (Shield_is_equipped)
		{
			Shield.position = Shield_eq.position;
			Shield.rotation = Shield_eq.rotation;
		}
		else
		{
			Shield.position = Shield_ueq.position;
			Shield.rotation = Shield_ueq.rotation;
		}
	}
	public void Shield_Equip()
	{
		Shield_is_equipped = true;
		anim.SetBool("Shield", true);
		if (anim.GetBool("Sword"))
		{
			anim.SetTrigger("sword_i");
		}
		anim.ResetTrigger("Shield_i");
	}
	public void Shield_Unequiped()
	{
		Shield_is_equipped = false;
		anim.SetBool("Shield", false);
		anim.ResetTrigger("Shield_i");
		anim.SetBool("Block", false);

	}
}
