using UnityEngine;
using RosMessageTypes.Geometry;

public class VelocityController : MonoBehaviour
{
    public GameObject husky;

    private ArticulationBody[] wheelsArticulationBodies;
    private UnityEngine.Transform baseLink;

    private float radius = 1.0f;
    private float separation = 1.875f;
    private float velRight = 0;
    private float velLeft = 0;
    private float speedFactor = 5.0f;

    public bool receivedVelCommand = false;

    public void Init()
    {
        wheelsArticulationBodies = new ArticulationBody[4];
        string base_link = "base_link/base_footprint/";
        string front_left_wheel = base_link + "front_left_wheel_link";
        wheelsArticulationBodies[0] = husky.transform.Find(front_left_wheel).GetComponent<ArticulationBody>();
        string front_right_wheel = base_link + "front_right_wheel_link";
        wheelsArticulationBodies[1] = husky.transform.Find(front_right_wheel).GetComponent<ArticulationBody>();
        string rear_left_wheel = base_link + "rear_left_wheel_link";
        wheelsArticulationBodies[2] = husky.transform.Find(rear_left_wheel).GetComponent<ArticulationBody>();
        string rear_right_wheel = base_link + "rear_right_wheel_link";
        wheelsArticulationBodies[3] = husky.transform.Find(rear_right_wheel).GetComponent<ArticulationBody>();

        baseLink = husky.transform.Find("base_link");
    }

    public void ToWheelsSpeed(Twist cmd_vel)
    {
        var lin = cmd_vel.linear;
        var ang = cmd_vel.angular;
        velRight = (float)(180.0 / Mathf.PI * ((2 * lin.x + ang.z * separation) / (2 * radius)));
        velLeft = (float)(180.0 / Mathf.PI * ((2 * lin.x - ang.z * separation) / (2 * radius)));
        receivedVelCommand = true;
    }

    public void ApplyWheelsSpeed()
    {
        // Left wheels
        var joint1XDrive = wheelsArticulationBodies[0].xDrive;
        joint1XDrive.targetVelocity = speedFactor * velLeft / 2.0f;
        wheelsArticulationBodies[0].xDrive = joint1XDrive;

        joint1XDrive = wheelsArticulationBodies[2].xDrive;
        joint1XDrive.targetVelocity = speedFactor * velLeft / 2.0f;
        wheelsArticulationBodies[2].xDrive = joint1XDrive;

        // Right wheels
        joint1XDrive = wheelsArticulationBodies[1].xDrive;
        joint1XDrive.targetVelocity = speedFactor * velRight / 2.0f;
        wheelsArticulationBodies[1].xDrive = joint1XDrive;

        joint1XDrive = wheelsArticulationBodies[3].xDrive;
        joint1XDrive.targetVelocity = speedFactor * velRight / 2.0f;
        wheelsArticulationBodies[3].xDrive = joint1XDrive;

    }

    public UnityEngine.Transform GetBaseLinkTransform()
    {
        return this.baseLink;
    }

}
