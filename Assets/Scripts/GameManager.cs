using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] LevelStartInfo StartInfo;

    internal static Action GoalReachedEvent;
    internal static Action ResetEvent;
    [SerializeField] Transform LevelHolder;
    [SerializeField] TMP_Text Title;
    [SerializeField] RectTransform WinText;
    [SerializeField] RectTransform TitlePanel;
    [SerializeField] Image TransitionImage;

    GameObject currentLevel;
    bool levelEnded;
    private void Awake()
    {
        GoalReachedEvent += OnGoalReached;
    }

    private void OnDestroy()
    {
        GoalReachedEvent -= OnGoalReached;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (StartInfo.Level == null) StartInfo.LoadNextLevel();
        ReloadLevel();
    }

    private void Update()
    {
        var keyb = Keyboard.current;
        var pad = Gamepad.current;

        if (keyb != null && keyb.f5Key.wasPressedThisFrame || pad != null && pad.selectButton.wasPressedThisFrame)
        {
            ReloadLevel();
        }

        if(keyb != null && keyb.escapeKey.wasPressedThisFrame)
        {
            TransitionImage.DOKill();
            TransitionImage.DOFade(1, .5f).onComplete = () => {
                SceneManager.LoadScene("MainMenu");
            };
        }
    }

    private void LevelStart()
    {
        // Animate Stuff
        DOTween.Sequence()
            .Append(TransitionImage.DOFade(0, .5f))
            .Append(TitlePanel.DOAnchorPos(Vector2.zero, .25f))
            .AppendInterval(2)
            .Append(TitlePanel.DOAnchorPos(new Vector2(0, -800), .25f))
            .AppendCallback(() =>
            {
                levelEnded = false;
            });
    }

    private void LevelEnd()
    {
        SFXPlayer.Play("GoodEnd");
        DOTween.Sequence()
            .Append(WinText.DOAnchorPos(Vector2.zero, .25f))
            .AppendInterval(2)
            .Append(TransitionImage.DOFade(1, .5f))
            .AppendCallback(() =>
            {
                StartInfo.LoadNextLevel();
                ReloadLevel();
            });
    }

    private void OnGoalReached()
    {
        if (levelEnded) return;
        levelEnded = true;
        LevelEnd();
    }

    private void ReloadLevel()
    {
        if (currentLevel != null) Destroy(currentLevel);
        GlobalMaskManager.ResetManager();

        TitlePanel.anchoredPosition = new Vector2(0, 800);
        WinText.anchoredPosition = new Vector2(0, 800);

        if (StartInfo.Level != null)
        {
            // Initialize Stuff
            Title.text = "Niveau " + StartInfo.Level.LevelIndex + "<br>" + StartInfo.Level.LevelName;

            // Load Prefab
            if (StartInfo.Level.LevelPrefab != null) currentLevel = Instantiate(StartInfo.Level.LevelPrefab, LevelHolder);

            GlobalMaskManager.SetAvailableMasks(StartInfo.Level.AvailableMasks);
        }
        else
        {
            Title.text = "OMG C'EST FINI ???";
        }
        LevelStart();
    }
}
