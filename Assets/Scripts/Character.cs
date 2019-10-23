using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Character : MonoBehaviour {

    public int DefaultSpawnX = -1;
    public int DefaultSpawnY = 0;

    //public AnimationClip animationClip;
    public Animator animator;
    //protected AnimatorOverrideController animatorOverrideController;

    void Start () {
        //animator = GetComponent<Animator>();

        //animatorOverrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
        //animator.runtimeAnimatorController = animatorOverrideController;
        //animatorOverrideController["idle"] = animationClip;
    }
}
