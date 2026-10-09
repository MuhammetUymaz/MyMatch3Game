namespace AnimationSystem
{
    public interface IAnimationEvents
    {
        void AnimationIsCompleted(Animation animation);
        void AnimationIsUpdating(Animation animation);
    }
}