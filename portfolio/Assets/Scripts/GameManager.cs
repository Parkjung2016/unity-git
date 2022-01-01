using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instace = null;
    public Volume m_Volume;
    ColorAdjustments m_colorAdjustments;
    bool setcoloradj;
    public PlayableDirector Timeline;
    public GameObject VCam;
    public GameObject VCam2;
    public GameObject PlayerCam;
    public float setcoloradjSpeed;
    public PlayableAsset[] Playable;
    public PlayerMove player;
    public GameObject PortalEffect;
    public Vector3 TeleportPos;
    public GameObject Boss;
    public Vector3 spawnbossPos;
    public GameObject gate;
    public GameObject PlayerInBoat;
    bool fade;
    UI UI_;
    float fadevalue;
    public SkipUI skipUI_;
    public CinemachineImpulseSource impulse;
    public float ShakePower;
    private void Awake()
    {
        UI_ = GetComponent<UI>();
        if (instace == null)
        {
            instace = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        impulse = GetComponent<CinemachineImpulseSource>();
        PortalEffect.SetActive(false);
        TeleportPos = GameObject.FindGameObjectWithTag("TeleportPos").transform.position;
        Timeline = GetComponent<PlayableDirector>();
        Timeline.playableAsset = Playable[0];
        m_Volume.profile.TryGet<ColorAdjustments>(out m_colorAdjustments);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Timeline.Play();
        UI_.UICanavas.SetActive(false);
        UI_.BossHPObj.SetActive(false);
        player.gameObject.SetActive(false);
        player.movetrue = false;
    }
    private void Update()
    {
        if(setcoloradj)
        {
           
            if(m_colorAdjustments.postExposure.value <= -9)
            {
                StartCoroutine(ChangeCamera());
                Destroy(PlayerInBoat);

            }
        }
        if(fade)
        {
            if (m_colorAdjustments.postExposure.value >= 1 && !setcoloradj)
            {
                fade = false;

            }
            m_colorAdjustments.postExposure.value = Mathf.Lerp(m_colorAdjustments.postExposure.value, fadevalue, setcoloradjSpeed * Time.deltaTime);
        }
    }
    public IEnumerator ChangeCamera()
    {
        setcoloradj = false;
        yield return new WaitForSeconds(1);
        m_colorAdjustments.postExposure.value = 1.08f;
        Timeline.Stop();
        UI_.FadeQuestAnim();
        if (skipUI_ != null)
            Destroy(skipUI_.gameObject);
        player.movetrue = true;
        UI_.UICanavas.SetActive(true);
        player.gameObject.SetActive(true);
        yield return null;
    }
    public void TimeLineSignal()
    {
        Destroy(skipUI_.gameObject);
        setcoloradj = true;
        fadevalue = -10;
        fade = true;
    }
    public static IEnumerator CameraShake(Animator PlayerAnim, Animator EnemyAnim)
    {
        instace.impulse.GenerateImpulse(instace.ShakePower);
        PlayerAnim.speed = 0;
        EnemyAnim.speed = 0;
        yield return new WaitForSeconds(0.2f);
        PlayerAnim.speed = 1;
        EnemyAnim.speed = 1;
        yield return null;
    }
    IEnumerator SpawnBossSignal()
    {
        instace.Timeline.playableAsset = instace.Playable[1];
        GameObject Boss =  Instantiate(instace.Boss, instace.spawnbossPos, Quaternion.Euler(0, 90, 0));
        Boss.GetComponent<BossMove>().FindPlayer();
        Destroy(gate);
        instace.Timeline.Play();
        instace.UI_.UICanavas.SetActive(false);
        instace.m_colorAdjustments.postExposure.value = -10;
        instace.player.movetrue = false;
        UI_.QuestAnim.gameObject.SetActive(false);
        instace.player.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        instace.fade = true;
        instace.fadevalue = 1.08f;
        yield return null;
    }
    static public void SpawnBoss()
    {
        instace.StartCoroutine(instace.SpawnBossSignal());
    }
    public void FinishBossTimeline()
    {
        player.movetrue = true;
        player.gameObject.SetActive(true);
        Timeline.Stop();
        UI_.BossHPObj.SetActive(true);
        UI_.UICanavas.SetActive(true);
    }
}
