using TMPro;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [SerializeField] LevelStartInfo startInfo;
    [SerializeField] TMP_Text LevelText;
    private LevelDefinition CurrentLevel;
    
    internal void AssignLevel(LevelDefinition definition)
    {
        LevelText.text = definition.LevelIndex.ToString();
        CurrentLevel = definition;
    }

    public void SelectLevel()
    {
        startInfo.LoadLevel(CurrentLevel);
        FindFirstObjectByType<MainMenu>()?.GoTolevelScene();
    }

}
