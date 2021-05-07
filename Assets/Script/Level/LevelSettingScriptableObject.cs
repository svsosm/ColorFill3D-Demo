using UnityEngine;

[CreateAssetMenu(fileName = "New Level Settings", menuName = "Level Settings", order =1)]
public class LevelSettingScriptableObject : ScriptableObject
{
    public Vector3 startPos;
    public int width;
    public int height;
    public int percentageOfWin;
}
