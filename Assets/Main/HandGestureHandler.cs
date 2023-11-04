using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;


    public class HandGestureHandler : MonoBehaviour
    {
        private bool isRecording = false;

        public OpenAI.WhisperKeys whisperKeysScript;


        private const float PinchThreshold = 0.7f;
        private const float GrabThreshold = 0.4f;

        void Update()
        {
            // Assuming you want to use right hand for the gestures
            // You can easily add conditions for the left hand as well
            if (IsPinching(Handedness.Right) && !isRecording)
            {
                StartRecording();
            }
            else if (IsGrabbing(Handedness.Right) && isRecording)
            {
                StopRecording();
            }
        }

        private bool IsPinching(Handedness trackedHand)
        {
            return HandPoseUtils.CalculateIndexPinch(trackedHand) > PinchThreshold;
        }

        private bool IsGrabbing(Handedness trackedHand)
        {
            return !IsPinching(trackedHand) &&
                   HandPoseUtils.MiddleFingerCurl(trackedHand) > GrabThreshold &&
                   HandPoseUtils.RingFingerCurl(trackedHand) > GrabThreshold &&
                   HandPoseUtils.PinkyFingerCurl(trackedHand) > GrabThreshold &&
                   HandPoseUtils.ThumbFingerCurl(trackedHand) > GrabThreshold;
        }

        private void StartRecording()
        {
            isRecording = true;
            whisperKeysScript.StartRecording();
        }

        private void StopRecording()
        {
            isRecording = false;
            whisperKeysScript.EndRecording();
        }
    }

