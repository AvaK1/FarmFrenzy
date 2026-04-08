using UnityEngine;

[CreateAssetMenu(fileName = "AnimationSO", menuName = "Scriptable Objects/AnimationSO")]
public class AnimationData : ScriptableObject
{
    public string animationName;
    public Sprite[] frames;
    public float frameDelay;
    public bool loop;
}
