// 1. AnimationSettings.cs  (ScriptableObject - create via Assets ¡÷ Create ¡÷ AnimationSettings)
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "AnimationSettings", menuName = "DOTween/Animation Settings")]
public class AnimationSettings : ScriptableObject
{
    [Header("Jump")]
    public float JumpPower = 2.5f;
    public int NumJumps = 1;
    public float JumpDuration = 0.9f;
    public Ease JumpEase = Ease.OutQuad;
    public bool UseLocalJumpByDefault = true;   // can still override per call

    [Header("Y-Rotation Shake (on landing)")]
    public float ShakeDuration = 0.6f;
    public float ShakeStrengthY = 45f;
    public int ShakeVibrato = 12;
    public float ShakeRandomness = 40f;
    public bool ShakeFadeOut = true;

    [Header("Squeeze / Punch Scale")]
    public float SqueezeDuration = 0.4f;
    public Vector3 PunchScale = new Vector3(0.28f, -0.38f, 0.28f);
    public int PunchVibrato = 8;
    public float PunchElasticity = 0.7f;

    [Header("Move Away ¡÷ Return")]
    public Vector3 MoveAwayOffset = new Vector3(0, 0.8f, 2.5f);   // relative
    public float MoveAwayDuration = 0.7f;
    public float ReturnDuration = 0.8f;
    public Ease MoveEaseOut = Ease.OutQuad;
    public Ease MoveEaseReturn = Ease.InOutQuad;

    [Header("General")]
    public float PostSqueezePause = 0.1f;
    public float FullSequenceDelay = 0.2f;
}