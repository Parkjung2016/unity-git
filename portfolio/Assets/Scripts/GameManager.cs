using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Playables;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GameManager : MonoBehaviour
{
    public Animation Fade;
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
    public GameObject PortalEffect2;
    public Vector3 TeleportPos;
    public Vector3 TeleportPos2;
    public GameObject Boss;
    public Vector3 spawnbossPos;
    public GameObject gate;
    public GameObject PlayerInBoat;
    bool fade;
    public UI UI_;
    private float fadevalue;
    public SkipUI skipUI_;
    public CinemachineImpulseSource impulse;
    public float ShakePower;
    public int SpawnPoint = 0;
    public Vector3 PlayerSpawnPos;
    public Animation DeathBgAnim;
    public GameObject DestroyEffect;
    private GameObject bossSpawnCol;
    public GameObject portal2;
    public Transform TimeLinePlayer;
    public GameObject InGameUI;
    public int HpPotionAmount;
    public AudioSource boatsfx;
    bool pauseTrue;
    private void Awake()
    {
        portal2 = GameObject.FindGameObjectWithTag("portal2");
        bossSpawnCol = GameObject.FindGameObjectWithTag("BossSpawnCol");
        UI_ = GetComponent<UI>();
        if (instace == null)
        {
            instace = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pauseTrue = false;
        UI_.SetHPPotionAmountText();
        InGameUI.SetActive(true);
        Fade.gameObject.SetActive(false);
        VCam2.SetActive(false);
        bossSpawnCol.SetActive(true);
        PlayerSpawnPos = player.transform.position;
        impulse = GetComponent<CinemachineImpulseSource>();
        PortalEffect.SetActive(false);
        PortalEffect2.SetActive(false);
        TeleportPos = GameObject.FindGameObjectWithTag("TeleportPos").transform.position;
        TeleportPos2 = GameObject.FindGameObjectWithTag("TeleportPos2").transform.position;
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
    public void AddHPPotion(int amount, bool isincrease)
    {
        HpPotionAmount= isincrease ? HpPotionAmount + amount : HpPotionAmount - amount;
        UI_.SetHPPotionAmountText();
    }
    private void Update()
    {
        if (setcoloradj)
        {

            if (m_colorAdjustments.postExposure.value <= -9)
            {
                StartCoroutine(ChangeCamera());
                Destroy(PlayerInBoat);

            }
        }
        if (fade)
        {
            if (m_colorAdjustments.postExposure.value >= 1 && !setcoloradj)
            {
                fade = false;
                pauseTrue = true;

            }
            m_colorAdjustments.postExposure.value = Mathf.Lerp(m_colorAdjustments.postExposure.value, fadevalue, setcoloradjSpeed * Time.deltaTime);
        }

        if (PlayerHPManager.Instance.death)
        {
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            if (boss != null)
            {
                Instantiate(DestroyEffect, boss.transform.position, Quaternion.identity);
                Destroy(boss);
            }
        }
        if (pauseTrue)
            if (Input.GetKeyDown(KeyCode.Escape) && !PlayerHPManager.Instance.death || Input.GetKeyDown(KeyCode.Escape) && BossHPManager.Instance != null && !BossHPManager.Instance.Death)
            {
                UI_.Pause.SetActive(!UI_.Pause.activeSelf);
                Time.timeScale = UI_.Pause.activeSelf ? 0 : 1;
                Cursor.visible = UI_.Pause.activeSelf ? true : false;
                Cursor.lockState = UI_.Pause.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
            }
    }
    public void ContinueBtnClick()
    {
        UI_.Pause.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
    }
    public void SpawnTimeLinePlayerDestroyEffect()
    {
        Instantiate(DestroyEffect, TimeLinePlayer.position, Quaternion.identity);
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
        boatsfx.Stop();
        fade = true;
    }
    public void FadetoMainMenu()
    {
        StartCoroutine(Fade_main());
    }
    public IEnumerator HpInteractionKeyanim()
    {
        UI_.HpInteractionKey.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        UI_.HpInteractionKey.color = Color.white;
    }
    private IEnumerator Fade_main()
    {
        pauseTrue = false;
        Fade.gameObject.SetActive(true);
        Fade.Play();
        yield return new WaitForSeconds(4);
        LoadMainMenu();
    }
    public static IEnumerator CameraShake(Animator PlayerAnim, Animator EnemyAnim, float Power)
    {
        if (PlayerAnim == null)
            yield return null;
        instace.impulse.GenerateImpulse(Power);
        PlayerAnim.speed = 0;
        EnemyAnim.speed = 0;
        yield return new WaitForSeconds(0.2f);
        if (PlayerAnim == null)
            yield return null;
        PlayerAnim.speed = 1;
        EnemyAnim.speed = 1;
        yield return null;
    }
    IEnumerator SpawnBossSignal()
    {
        if (Timeline.playableAsset == Playable[2])
            yield return null;
        instace.Timeline.playableAsset = instace.Playable[1];
        GameObject Boss =  Instantiate(instace.Boss, instace.spawnbossPos, Quaternion.Euler(0, 90, 0));
        Boss.GetComponent<BossMove>().FindPlayer();
        gate.SetActive(false);
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
        instace.pauseTrue = false;
        instace.StartCoroutine(instace.SpawnBossSignal());
        instace.portal2.SetActive(false);
    }
    public void FinishBossTimeline()
    {
        player.movetrue = true;
        player.gameObject.SetActive(true);
        Timeline.Stop();
        UI_.BossHPObj.SetActive(true);
        UI_.UICanavas.SetActive(true);
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync("Main");
    }
    public void PlayBossKillTimeLine()
    {
        pauseTrue = false;
        Destroy(player.gameObject);
        Fade.gameObject.SetActive(false);
        Timeline.playableAsset = Playable[2];
        Timeline.Play();
    }
    public void FinishFade()
    {
        InGameUI.SetActive(false);
        PlayBossKillTimeLine();
        Destroy(GameObject.FindGameObjectWithTag("Boss"));
    }
    public void RestartGame()
    {
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.rotation = Quaternion.Euler(0, -90, 0);
        switch (SpawnPoint)
        {
            case 0:
                player.transform.position = PlayerSpawnPos;
                break;
            case 1:
                player.transform.position = TeleportPos;
                break;
        }
        gate.SetActive(true);
        PlayerHPManager.Instance.HP = PlayerHPManager.Instance.maxHP;
        PlayerHPManager.Instance.death = false;
        UI_.BossHPObj.SetActive(false);
        Camera.main.GetComponent<CinemachineBrain>().enabled = true;
        PlayerHPManager.Instance.Sword.GetComponent<MeshCollider>().enabled = false;
        PlayerHPManager.Instance.Rifle.GetComponent<MeshCollider>().enabled = false;
        PlayerHPManager.Instance.Shield.GetComponent<MeshCollider>().enabled = false;
        Collider col = PlayerHPManager.Instance.Sword.AddComponent<BoxCollider>();
        col.isTrigger = true;
        PlayerHPManager.Instance.Sword.GetComponent<PlayerWeapon>().Col = col;
        portal2.SetActive(true);
        player.GetComponent<PlayerRifleAttack>().RifleAttackTrue = false;
        player.movetrue = true;
        player.GetComponent<PlayerRifleEquip>().Rifle_is_equipped = false;
        player.GetComponent<PlayerEquip>().sword_is_equipped = false;
        player.GetComponent<PlayerShieldEquip>().Shield_is_equipped = true;
        player.GetComponent<PlayerAttack>().ComboReset();
        Destroy(PlayerHPManager.Instance.Sword.GetComponent<Rigidbody>());
        Destroy(PlayerHPManager.Instance.Rifle.GetComponent<Rigidbody>());
        Destroy(PlayerHPManager.Instance.Shield.GetComponent<Rigidbody>());
        PlayerRifleAttack script = player.GetComponent<PlayerRifleAttack>();
        PortalEffect2.SetActive(true);
        script.currentBullets = script.maxBullet;
        UI_.SetBulletText(script.currentBullets, script.maxBullet);
        Cursor.visible = false;
        bossSpawnCol.SetActive(true);
        player.velocity = Vector3.zero;
        player.MoveDir = Vector3.zero;
        PlayerHPManager.Instance.anim.applyRootMotion = false;
        Animator anim = player.GetComponent<Animator>();
        anim.Play("Idle");
        anim.SetBool("Shield", true);
        anim.SetBool("Sword", false);
        anim.SetBool("RifleAimUp", false);
         anim.Play("None",1);
         anim.Play("None",2);
         anim.Play("None",3);
         anim.Play("None",4 );
        DeathBgAnim.GetComponent<CanvasGroup>().alpha = 0;
        Destroy(GameObject.FindGameObjectWithTag("bossskill"));
        Cursor.lockState = CursorLockMode.Locked;
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerRifleEquip>().RigBuilderTrue = false;
    }
}
