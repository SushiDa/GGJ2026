using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private LevelButton ButtonPrefab;
    [SerializeField] private Transform ButtonHolder;
    [SerializeField] private Image TransitionImage;
    [SerializeField] private AudioSource MusicSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var levels = Resources.LoadAll<LevelDefinition>("Levels");
        var lvList = new List<LevelDefinition>();
        lvList.AddRange(levels);
        lvList.Sort((a, b) => a.LevelIndex.CompareTo(b.LevelIndex));

        foreach (var level in lvList)
        {
            var btn = Instantiate(ButtonPrefab,ButtonHolder);
            btn.AssignLevel(level);
        }
        TransitionImage.DOKill();
        TransitionImage.DOFade(0, .5f);
    }

    public void GoTolevelScene()
    {
        TransitionImage.DOKill();
        MusicSource.DOFade(0, 0.5f);
        TransitionImage.DOFade(1, .5f).onComplete = () => {
            SceneManager.LoadScene("PlayScene");
        };
    }

    public void Exit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }
}
