using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
//using Finger;

public class HandAnimator : MonoBehaviour
{
    public float speed = 5.0f;
    public XRController controller = null; 
    
    private Animator animator = null ; 

    private readonly List<Finger> gripfingers = new List<Finger>()
    {
        new Finger(FingerType.Middle),
        new Finger(FingerType.Ring),
        new Finger(FingerType.Pinky)
    };

      private readonly List<Finger> pointfingers = new List<Finger>()
    {
        new Finger(FingerType.Index),
        new Finger(FingerType.Thumb)
    };
     
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Store Input 
        CheckGrip();
        CheckPointer();

        // Smooth input values 
        SmoothFinger(pointfingers);
        SmoothFinger(gripfingers);

        // Aply smooth values 
        AnimateFinger(pointfingers);
        AnimateFinger(gripfingers);
    }

    private void CheckGrip()
    {
        if(controller.inputDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            SetFingerTargets(gripfingers, gripValue);
    }

    private void CheckPointer()
    {
        if(controller.inputDevice.TryGetFeatureValue(CommonUsages.grip, out float pointerValue))
            SetFingerTargets(pointfingers, pointerValue);
    }

    private void SetFingerTargets(List<Finger> fingers, float value)
    {
        foreach (Finger finger in fingers)
            finger.target = value; 
    }

    private void SmoothFinger(List<Finger> fingers)
    {
        foreach (Finger finger in fingers)
        {
            float time = speed*Time.unscaledDeltaTime;
            finger.current = Mathf.MoveTowards(finger.current, finger.target, time);
        }
            
    }

    private void AnimateFinger(List<Finger> fingers)
    {
        foreach (Finger finger in fingers)
            AnimateFinger(finger.type.ToString(), finger.current);
    }

    private void AnimateFinger(string finger, float blend)
    {
            animator.SetFloat(finger, blend);
    }
}