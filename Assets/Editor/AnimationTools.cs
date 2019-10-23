//using Assets.Scripts.Resources;
//
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;
//using static UnityEngine.GUIUtility;


// CANNOT BE USED IN BUILD!

//public class AnimationTools : MonoBehaviour
//{
//    public AnimationClip GetAnimation(Resource resource)
//    {
//        return ToAnimation(resource.Animation.Select(i => i.ToSprite(StaticHelpers.CentralPivot)));
//    }

//    public AnimationClip ToAnimation(IEnumerable<Sprite> sprites, int frameRate = 6)
//    {
//        var spriteArray = sprites.ToArray();
//        var animClip = new AnimationClip()
//        {
//            frameRate = frameRate,
//            wrapMode = WrapMode.Loop
//        };

//        AnimationUtility.GetAnimationClipSettings(animClip).loopTime = true;

//        var spriteBinding = new EditorCurveBinding
//        {
//            type = typeof(SpriteRenderer),
//            path = "",
//            propertyName = "m_Sprite",
//        };

//        var spriteKeyFrames = new ObjectReferenceKeyframe[spriteArray.Length];

//        for (int i = 0; i < spriteArray.Length; i++)
//        {
//            spriteKeyFrames[i] = new ObjectReferenceKeyframe
//            {
//                time = (float)i / frameRate,
//                value = spriteArray[i]
//            };

//        }

//        AnimationClipSettings animationClipSettings = new AnimationClipSettings();

//        animationClipSettings.loopTime = true;

//        AnimationUtility.SetAnimationClipSettings(animClip, animationClipSettings);
//        AnimationUtility.SetObjectReferenceCurve(animClip, spriteBinding, spriteKeyFrames);

//        return animClip;
//    }
//}
