using UnityEngine;

namespace AnimationSystem
{
    public class Animation
    {
        public bool isActive { get; private set; }
        public Transform transform { get; private set; }
        public SpriteRenderer spriteRenderer { get; private set; }
        IAnimationEvents events;

        public AnimationType type { get; private set; }

        float currentTime;
        float limitedTime;
        float delayTime;

        EaseTypes easeType;

        //Values
        public Vector3 initialV3 { get; private set; }
        public Vector3 destinationV3 { get; private set; }
        public Color initialColor { get; private set; }
        public Color destinationColor { get; private set; }

        public void Reset(Transform transform, Vector3 initialV3, Vector3 destinationV3, float limitedTime, AnimationType type, EaseTypes easeType, IAnimationEvents events, float delay)
        {
            isActive = true;
            currentTime = 0;
            this.transform = transform;
            this.initialV3 = initialV3;
            this.destinationV3 = destinationV3;
            this.limitedTime = limitedTime;
            this.delayTime = delay;
            this.type = type;
            this.easeType = easeType;
            this.events = events;
        }

        public void Reset(SpriteRenderer spriteRenderer, Color initialColor, Color destinationColor, float limitedTime, AnimationType type, EaseTypes easeType, IAnimationEvents events, float delay)
        {
            isActive = true;
            currentTime = 0;
            this.spriteRenderer = spriteRenderer;
            this.initialColor = initialColor;
            this.destinationColor = destinationColor;
            this.limitedTime = limitedTime;
            this.delayTime = delay;
            this.type = type;
            this.easeType = easeType;
            this.events = events;
        }

        public void Run()
        {
            currentTime += Time.deltaTime;

            if (currentTime < delayTime)
                return;

            float arrangedCurrentTime = currentTime - delayTime;

            float norm = arrangedCurrentTime > limitedTime ? 1 : arrangedCurrentTime / limitedTime;

            Vector3 resultV3 = Vector3.zero;
            Color resultColor = Color.white;
            bool isEnded = norm >= 1;

            switch (type)
            {
                case AnimationType.Position:
                case AnimationType.Rotation:
                case AnimationType.Scale:

                    resultV3 = Vector3.Lerp(initialV3, destinationV3, GetEaseValue(norm, easeType));
                    break;

                case AnimationType.Color:
                    resultColor = Color.Lerp(initialColor, destinationColor, GetEaseValue(norm, easeType));
                    break;
            }

            if (type == AnimationType.Position)
                transform.position = resultV3;
            else if (type == AnimationType.Rotation)
                transform.rotation = Quaternion.Euler(resultV3);
            else if (type == AnimationType.Scale)
                transform.localScale = resultV3;
            else if (type == AnimationType.Color)
                spriteRenderer.color = resultColor;

            if(events != null)
                events.AnimationIsUpdating(this);

            if (isEnded)
            {
                isActive = false;

                if(events != null)
                    events.AnimationIsCompleted(this);
            }
        }

        public void Stop(bool shouldApplyDestionationValue = true)
        {
            if (shouldApplyDestionationValue)
            {
                Vector3 resultV3 = Vector3.zero;
                Color resultColor = Color.white;

                switch (type)
                {
                    case AnimationType.Position:
                    case AnimationType.Rotation:
                    case AnimationType.Scale:

                        if (easeType == EaseTypes.SinWave)
                            resultV3 = initialV3;
                        else
                            resultV3 = Vector3.Lerp(initialV3, destinationV3, GetEaseValue(1, easeType));
                        break;

                    case AnimationType.Color:

                        if (easeType == EaseTypes.SinWave)
                            resultColor = initialColor;
                        else
                            resultColor = Color.Lerp(initialColor, destinationColor, GetEaseValue(1, easeType));
                        break;
                }

                if (type == AnimationType.Position)
                    transform.position = resultV3;
                else if (type == AnimationType.Rotation)
                    transform.rotation = Quaternion.Euler(resultV3);
                else if (type == AnimationType.Scale)
                    transform.localScale = resultV3;
                else if (type == AnimationType.Color)
                    spriteRenderer.color = resultColor;
            }

            isActive = false;
            events.AnimationIsCompleted(this);
        }

        public void Pause() => isActive = false;

        public void Continue() => isActive = true;

        float GetEaseValue(float norm, EaseTypes theEaseType)
        {
            switch (theEaseType)
            {
                case EaseTypes.Linear:
                    return norm;

                case EaseTypes.InSine:
                    return 1 - Mathf.Cos((norm * Mathf.PI) / 2);

                case EaseTypes.OutSine:
                    return Mathf.Sin((norm * Mathf.PI) / 2);

                case EaseTypes.InOutSine:
                    return -(Mathf.Cos(Mathf.PI * norm) - 1) / 2;

                case EaseTypes.SinWave:
                    if (norm <= 0.5f)
                        return GetEaseValue(Lerp(0, 0.5f, norm), EaseTypes.InSine);
                    else
                        return GetEaseValue(Lerp(0, 0.5f, 1 - norm), EaseTypes.OutSine);
                case EaseTypes.OutBounce:
                    float n1 = 7.5625f;
                    float d1 = 2.75f;
                    if (norm < 1 / d1)
                        return n1 * norm * norm;
                    else if (norm < 2 / d1)
                        return n1 * (norm -= 1.5f / d1) * norm + 0.75f;
                    else if (norm < 2.5 / d1)
                        return n1 * (norm -= 2.25f / d1) * norm + 0.9375f;
                    else
                        return n1 * (norm -= 2.625f / d1) * norm + 0.984375f;

                case EaseTypes.InQuint:
                    return norm * norm * norm * norm;
            }

            throw new System.Exception("The ease type has not been taken account. Ease type is: " + easeType);
        }

        public static float Lerp(float min, float max, float current)
        {
            if (max == min)
                return 0;

            return (current - min) / (max - min);
        }
    }
}