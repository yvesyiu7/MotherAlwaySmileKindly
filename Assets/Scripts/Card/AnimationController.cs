// 2. ProjectileAnimator.cs  (put this on the object you want to animate)
using UnityEngine;
using DG.Tweening;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private AnimationSettings settings;   // drag in inspector

    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    private void Awake()
    {
        // Auto-load if not assigned in Inspector
        if (settings == null)
        {
            // Assumes your ScriptableObject asset is named "AnimationSettings" 
            // and placed in "Assets/Resources/AnimationSettings.asset"
            // (create the Resources folder if needed)
            settings = Resources.Load<AnimationSettings>("AnimationSettings");

            if (settings == null)
            {
                Debug.LogError($"{name}: Could not auto-load AnimationSettings! " +
                               "Check if it's in Assets/Resources/AnimationSettings.asset", this);
                enabled = false;
                return;
            }
            else
            {
                Debug.Log($"{name}: Auto-loaded default AnimationSettings.");
            }
        }

        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;
    }

    // ¢w¢w¢w 1. Jump to specific destination ¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w
    public Tween JumpTo(Vector3 destination, bool? useLocal = null, float? customDuration = null)
    {
        transform.DOKill(true);

        bool local = useLocal ?? settings.UseLocalJumpByDefault;
        Vector3 target = local ? originalPosition + destination : destination;

        return transform.DOJump(
            target,
            settings.JumpPower,
            settings.NumJumps,
            customDuration ?? settings.JumpDuration
        ).SetEase(settings.JumpEase);
    }

    // ¢w¢w¢w 2. Y-axis shake ¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w
    public Tween ShakeY()
    {
        return transform.DOShakeRotation(
            settings.ShakeDuration,
            new Vector3(0, settings.ShakeStrengthY, 0),
            settings.ShakeVibrato,
            settings.ShakeRandomness,
            settings.ShakeFadeOut
        );
    }

    // ¢w¢w¢w 3. Squeeze/squash ¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w
    public Tween Squeeze()
    {
        return transform.DOPunchScale(
            settings.PunchScale,
            settings.SqueezeDuration,
            settings.PunchVibrato,
            settings.PunchElasticity
        );
    }

    // ¢w¢w¢w 4. Move away (using offset) ¡÷ back to original ¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w
    public Sequence MoveAwayAndReturn(Vector3? customOffset = null, float? awayDur = null, float? returnDur = null)
    {
        Vector3 offset = customOffset ?? settings.MoveAwayOffset;

        Sequence seq = DOTween.Sequence();

        seq.Append(
            transform.DOMove(
                originalPosition + offset,
                awayDur ?? settings.MoveAwayDuration
            ).SetEase(settings.MoveEaseOut)
        );

        seq.Append(
            transform.DOMove(
                originalPosition,
                returnDur ?? settings.ReturnDuration
            ).SetEase(settings.MoveEaseReturn)
        );

        return seq;
    }

    public Sequence ScaleUp()
    {
        transform.localScale = Vector3.zero;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(1,1f));
        return seq;
    }

    public Sequence MoveToAndReturn(Vector3 pos, float? awayDur = null, float? returnDur = null)
    {
        

        Sequence seq = DOTween.Sequence();

        seq.Append(
            transform.DOMove(
                pos,
                awayDur ?? settings.MoveAwayDuration
            ).SetEase(settings.MoveEaseOut)
        );

        seq.Append(
            transform.DOMove(
                pos,
                returnDur ?? settings.ReturnDuration
            ).SetEase(settings.MoveEaseReturn)
        );

        return seq;
    }

    // ¢w¢w¢w Convenience: full classic combo ¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w¢w
    [ContextMenu("Play Full Combo")]
    public void PlayFullCombo(Vector3 jumpDestination)
    {
        transform.DOKill();

        Sequence full = DOTween.Sequence();

        if (settings.FullSequenceDelay > 0)
            full.AppendInterval(settings.FullSequenceDelay);

        full.Append(JumpTo(jumpDestination));
        full.Append(ShakeY());
        full.Join(Squeeze());                   // shake + squeeze together
        full.AppendInterval(settings.PostSqueezePause);
        full.Append(MoveAwayAndReturn());

        // Safety reset
        full.OnComplete(() =>
        {
            transform.rotation = originalRotation;
            transform.localScale = originalScale;
        });

        full.Play();
    }

    [ContextMenu("Reset to Original")]
    public void ResetToOriginal()
    {
        transform.DOKill();
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        transform.localScale = originalScale;
    }
}