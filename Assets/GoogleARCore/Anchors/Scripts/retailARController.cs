namespace GoogleARCore.Examples.ObjectManipulation
{
    using GoogleARCore;
    using UnityEngine.EventSystems;
    using UnityEngine;
    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class retailARController : Manipulator
    {
        public Camera FirstPersonCamera;
        public GameObject obj1_prefab;
        public GameObject obj2_prefab;
        public GameObject ManipulatorPrefab;
        private bool firstBtnClicked = true;
        /// <summary>
        /// Returns true if the manipulation can be started for the given gesture.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        /// <returns>True if the manipulation can be started.</returns>
        protected override bool CanStartManipulationForGesture(TapGesture gesture)
        {
            if (gesture.TargetObject == null)
            {
                return true;
            }

            return false;
        }
        /// <summary>
        /// Function called when the manipulation is ended.
        /// </summary>
        /// <param name="gesture">The current gesture.</param>
        protected override void OnEndManipulation(TapGesture gesture)
        {
            if (gesture.WasCancelled)
            {
                return;
            }
            // If gesture is targeting an existing object we are done.
            if (gesture.TargetObject != null)
            {
                return;
            }
            // Should not handle input if the player is pointing on UI.

            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon;
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                //not toch UI
                if (Frame.Raycast(
                        gesture.StartPosition.x, gesture.StartPosition.y, raycastFilter, out hit))
                {
                    // Use hit pose and camera pose to check if hittest is from the
                    // back of the plane, if it is, no need to create the anchor.
                    if ((hit.Trackable is DetectedPlane) &&
                        Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                            hit.Pose.rotation * Vector3.up) < 0)
                    {
                        Debug.Log("Hit at back of the current DetectedPlane");
                    }
                    else
                    {
                        GameObject obj = null;

                        if (firstBtnClicked == true)
                        {
                            // Instantiate first Asset model at the hit pose.
                            obj = Instantiate(obj1_prefab, hit.Pose.position, hit.Pose.rotation);
                        }
                        else
                        {
                            // Second button clicked: Instantiate second Asset model at the hit pose.
                            obj = Instantiate(obj2_prefab, hit.Pose.position, hit.Pose.rotation);
                        }

                        // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                        // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                        // world evolves.
                        var manipulator = Instantiate(ManipulatorPrefab, hit.Pose.position, hit.Pose.rotation);
                        manipulator.name = "myobject";
                        obj.transform.parent = manipulator.transform;

                        var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                        // Make the object model a child of the anchor.
                        manipulator.transform.parent = anchor.transform;

                        manipulator.GetComponent<Manipulator>().Select();
                    }
                }
            }
        }

        public void firstButtonClick()
        {
            firstBtnClicked = true;
        }

        public void secondButtonClick()
        {
            firstBtnClicked = false;
        }

        public void delete()
        {
            //Test if already an object exists and delete it:
            if (GameObject.Find("Anchor/myobject") != null)
            {
                GameObject parentanchor = GameObject.Find("Anchor/myobject").transform.parent.gameObject;
                Destroy(parentanchor);
            }
        }
    }
}
