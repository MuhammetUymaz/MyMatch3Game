using System.Collections.Generic;
using UnityEngine;

namespace AnimationSystem
{
    public class AnimationManager : IUpdate
    {
        List<Animation> animations;

        public AnimationManager()
        {
            animations = new List<Animation>();
            
            SL.Get<UpdateManager>().observers.Add(this);
        }

        public Animation StartPositionAnimation(Transform transform, Vector3 initial, Vector3 destination, float limitedTime, AnimationType type, EaseTypes easeType, IAnimationEvents events, float delayTime = 0)
        {
            Animation animation = GetAnimation();

            animation.Reset(transform, initial, destination, limitedTime, type, easeType, events, delayTime);

            return animation;
        }

        public Animation StartRotationAnimation(Transform transform, Vector3 initial, Vector3 destination, float limitedTime, AnimationType type, EaseTypes easeType, IAnimationEvents events, float delayTime = 0)
        {
            Animation animation = GetAnimation();

            animation.Reset(transform, initial, destination, limitedTime, type, easeType, events, delayTime);

            return animation;
        }

        public Animation StartScaleAnimation(Transform transform, Vector3 initial, Vector3 destination, float limitedTime, AnimationType type, EaseTypes easeType, IAnimationEvents events, float delayTime = 0)
        {
            Animation animation = GetAnimation();

            animation.Reset(transform, initial, destination, limitedTime, type, easeType, events, delayTime);

            return animation;
        }

        public Animation StartSpriteRendererColorAnimation(SpriteRenderer spriteRenderer, Color initial, Color destination, float limitedTime, AnimationType type, EaseTypes easeType, IAnimationEvents events, float delayTime = 0)
        {
            Animation animation = GetAnimation();

            animation.Reset(spriteRenderer, initial, destination, limitedTime, type, easeType, events, delayTime);

            return animation;
        }

        Animation GetAnimation()
        {
            for (int i = 0; i < animations.Count; i++)
            {
                if (!animations[i].isActive)
                    return animations[i];
            }

            Animation newAnimation = new Animation();
            animations.Add(newAnimation);

            return newAnimation;
        }

        public void StopAllAnimations()
        {
            for(int i = 0; i < animations.Count; i++)
            {
                Animation animation = animations[i];
                if(animation.isActive)
                    animation.Stop();
            }
        }

        public void UpdateMe()
        {
            for (int i = 0; i < animations.Count; i++)
                if (animations[i].isActive)
                    animations[i].Run();
        }

        public bool IsThereActiveAnimation()
        {
            for (int i = 0; i < animations.Count; i++)
                if (animations[i].isActive)
                    return true;

            return false;
        }

        public UpdatingType updatingType { get => UpdatingType.AllTime; }
    }
}