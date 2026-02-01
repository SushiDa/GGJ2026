using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="LevelDefinition", menuName ="GGJ26/LevelDefinition")]
public class LevelDefinition : ScriptableObject
{
    [SerializeField] internal int LevelIndex;
    [SerializeField] internal string LevelName;
    [SerializeField] internal float CameraZoom = 8;
    [SerializeField] internal List<PrimaryColorMask> AvailableMasks;
    [SerializeField] internal GameObject LevelPrefab;
}
