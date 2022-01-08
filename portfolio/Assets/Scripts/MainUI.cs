using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    public GameObject Dragon;
    public Animation Borderanim;
    public AnimationClip fade;
    private void Awake()
    {
        DontDestroyOnLoad(Dragon);
    }
    private void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Quit(CanvasGroup canvasGroup_)
    {
        if (canvasGroup_.alpha != 1)
            return;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
     Application.Quit();
#endif
    }
    public void Play(CanvasGroup canvasGroup_)
    {
        if (canvasGroup_.alpha != 1)
            return;

        Borderanim.clip = fade;
        Borderanim.Play();
        StartCoroutine(StartGame());
    }
    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadSceneAsync("Game");
    }
}
