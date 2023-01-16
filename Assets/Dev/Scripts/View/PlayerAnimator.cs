using UnityEngine;

public class PlayerAnimator
{
	private Animator _animator;

	private const string StateAnimatorName = "AnimationState";

	private const string AtackTriggerAnimationName = "Attack";
	private const string JumpTriggerAnimationName = "Jump";

	private const string IsGroundedAnimationName = "IsGrounded";

    public enum AnimationState { Idle = 0, Run = 1, Attack = 2}
    public enum AnimationAction { Jump = 0, Attack = 1}
	public enum AnimationBoolean { IsGrounded = 0 }

	public PlayerAnimator(Animator animator) 
	{
		_animator = animator;
	}

	public void SetAnimationBoolean(AnimationBoolean animationBoolean, bool value)
	{
		switch (animationBoolean) 
		{ 
			case AnimationBoolean.IsGrounded:
				_animator.SetBool(IsGroundedAnimationName, value);
				break;
		}
	}

	public void PlayAnimation(AnimationAction animationAction)
	{
		switch (animationAction)
		{
			case AnimationAction.Jump:
				_animator.SetTrigger(JumpTriggerAnimationName);
				break;
		}
	}

	public void ChangeState(AnimationState state)
	{
		_animator.SetInteger(StateAnimatorName, (int)state);
	}
}