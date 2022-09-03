namespace Lunkums.Animation
{
    using UnityEngine;

    public class TriggeredAnimator : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private string _animation = "None";

        public void SetAnimation(string nextAnimation)
        {
            animator.ResetTrigger(_animation);
            _animation = nextAnimation;
            animator.SetTrigger(_animation);
        }

        public bool HasAnimationEnded()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;
        }
    }
}