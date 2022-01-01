using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordEffect : MonoBehaviour
{
    public GameObject[] SwordEffects;
    public Transform[] SwordEffectsTrans;

    public void SpawnEffect1()
    {
        GameObject Obj = Instantiate(SwordEffects[0], SwordEffectsTrans[0].position, SwordEffectsTrans[0].rotation);
        StartCoroutine(ParticleDestroy(Obj.GetComponentInChildren<ParticleSystem>()));
    }
    public void SpawnEffect2()
    {
        GameObject Obj = Instantiate(SwordEffects[1], SwordEffectsTrans[1].position, SwordEffectsTrans[1].rotation);
        StartCoroutine(ParticleDestroy(Obj.GetComponentInChildren<ParticleSystem>()));
    }
    public void SpawnEffect3()
    {
        GameObject Obj = Instantiate(SwordEffects[2], SwordEffectsTrans[2].position, SwordEffectsTrans[2].rotation);
        StartCoroutine(ParticleDestroy(Obj.GetComponentInChildren<ParticleSystem>()));

    }
    public void SpawnEffect4()
    {
        GameObject Obj = Instantiate(SwordEffects[3], SwordEffectsTrans[3].position, SwordEffectsTrans[3].rotation);
        StartCoroutine(ParticleDestroy(Obj.GetComponentInChildren<ParticleSystem>()));
    }
    public void SpawnEffect5()
    {
        GameObject Obj = Instantiate(SwordEffects[4], SwordEffectsTrans[4].position, SwordEffectsTrans[4].rotation);
        StartCoroutine(ParticleDestroy(Obj.GetComponentInChildren<ParticleSystem>()));


    }
    IEnumerator ParticleDestroy(ParticleSystem ps)
    {
        yield return new WaitForSeconds(2f);
        Destroy(ps.transform.parent.gameObject);
        yield return null;
    }
}
