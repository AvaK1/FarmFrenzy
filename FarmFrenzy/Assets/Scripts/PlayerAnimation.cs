using System.Collections.Generic;
using System;
using UnityEngine;

//Got the animation scripts from a previous class, will be updating them to work with this project
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(SpriteAnimator))]
public class PlayerAnimation : MonoBehaviour
{
    public List<AnimationStateData> animations = new List<AnimationStateData>();
    private SpriteAnimator spriteAnimator;
    private PlayerAnimationState currentState = PlayerAnimationState.IDLE_DOWN;
    private Dictionary<PlayerAnimationState, AnimationData> animationDictionary = new Dictionary<PlayerAnimationState, AnimationData>();
    private PlayerController playerMovement;

    private void Start()
    {
        InitializeDictionary();
        playerMovement = GetComponent<PlayerController>();
        playerMovement.OnMove += SetAnimationState;
        spriteAnimator = GetComponent<SpriteAnimator>();

    }

    private void OnDestroy()
    {
        playerMovement.OnMove -= SetAnimationState;
    }

    public void InitializeDictionary()
    {
        foreach (AnimationStateData animationStateData in animations)
        {
            animationDictionary.Add(animationStateData.state, animationStateData.animation);
        }
    }

    public void SetAnimationState(Vector2 moveDirection)
    {
        if (moveDirection.y < 0)
        {
            currentState = PlayerAnimationState.WALK_DOWN;
        }
        else if (moveDirection.y > 0)
        {
            currentState = PlayerAnimationState.WALK_UP;
        }
        else if (moveDirection.x < 0)
        {
            currentState = PlayerAnimationState.WALK_LEFT;

        }

        else if (moveDirection.x > 0)
        {
            currentState = PlayerAnimationState.WALK_RIGHT;

        }

        if (moveDirection == Vector2.zero)
        {
            currentState = GetIdleState(currentState);
        }
        spriteAnimator.InitializeAnimation(animationDictionary[currentState]);
    }

    private PlayerAnimationState GetIdleState(PlayerAnimationState currentState)
    {
        PlayerAnimationState state = PlayerAnimationState.IDLE_UP;

        switch (currentState)
        {
            case PlayerAnimationState.WALK_UP:
                state = PlayerAnimationState.IDLE_UP;
                break;
            case PlayerAnimationState.WALK_DOWN:
                state = PlayerAnimationState.IDLE_DOWN;
                break;
            case PlayerAnimationState.WALK_LEFT:
                state = PlayerAnimationState.IDLE_LEFT;
                break;
            case PlayerAnimationState.WALK_RIGHT:
                state = PlayerAnimationState.IDLE_RIGHT;
                break;
            default:
                break;
        }

        return state;

    }
}

public enum PlayerAnimationState
{
    WALK_UP,
    WALK_DOWN,
    WALK_LEFT,
    WALK_RIGHT,
    IDLE_UP,
    IDLE_DOWN,
    IDLE_LEFT,
    IDLE_RIGHT
}

[Serializable]
public class AnimationStateData
{
    public PlayerAnimationState state;
    public AnimationData animation;
}
