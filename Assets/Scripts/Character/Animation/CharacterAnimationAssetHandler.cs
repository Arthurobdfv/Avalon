using System; // Needed for struct, Enum constraints
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationAssetHandler : MonoBehaviour
{
    [SerializeField] Animator _characterAnimator;

    // TODO: Move to a common utility class
    public static readonly Dictionary<AvalonAnimationType, string> AnimationStateNames = new Dictionary<AvalonAnimationType, string>
    {
        { AvalonAnimationType.IDLE, "Idle" },
        { AvalonAnimationType.WALK, "Walk" },
        { AvalonAnimationType.RUN, "Running" },
        { AvalonAnimationType.ATTACK, "Attack" },
    };

    // Fix CS0453: Add 'where T : struct, Enum' constraint to match PlayerAssetData<T> requirements
    public void SetAnimations<T>(PlayerAssetData<T> assetData) where T : struct, Enum
    {
        //var states = _characterAnimator.layers[0].stateMachine.states;
        //var walkAnim = states.FirstOrDefault(x => x.state.name == "Walk").state;
        //var idleAnim = states.FirstOrDefault(x => x.state.name == "Idle").state;
        //var runningAnim = states.FirstOrDefault(x => x.state.name == "Running").state;
    }
}
