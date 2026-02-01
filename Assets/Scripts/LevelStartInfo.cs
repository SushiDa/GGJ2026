using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="LevelStartInfo", menuName ="GGJ26/LevelStartInfo")]
public class LevelStartInfo : ScriptableObject
{
    internal LevelDefinition Level { get; private set; }

    internal void LoadNextLevel()
    {
        var levels = Resources.LoadAll<LevelDefinition>("Levels");
        var lvList = new List<LevelDefinition>();
        lvList.AddRange(levels);
        lvList.Sort((a,b) => a.LevelIndex.CompareTo(b.LevelIndex));

        if(Level == null && lvList.Count > 0)
        {
            Level = lvList[0];
        }
        else
        {
            var index = lvList.FindIndex(x => x.LevelIndex == Level.LevelIndex);
            if (index + 1 < lvList.Count) Level = lvList[index + 1];
            else Level = null;
        }
    }

    internal void LoadLevel(LevelDefinition level)
    {
        Level = level;
    }
}
