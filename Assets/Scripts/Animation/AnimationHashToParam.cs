using UnityEngine;

namespace Animation
{
    public static class AnimationHashToParam
    {
        public static readonly int Move = Animator.StringToHash("Move");
        public static readonly int Reload = Animator.StringToHash("Reload");
        public static readonly int Dead = Animator.StringToHash("Dead");
        public static readonly int HasTarget = Animator.StringToHash("HasTarget");
    }
}