using System;
using PoseStamped = RosMessageTypes.Geometry.PoseStamped;
using Header = RosMessageTypes.Std.Header;
using Twist = RosMessageTypes.Geometry.Twist;
using Scan = RosMessageTypes.Sensor.LaserScan;
using ROSGeometry;
using UnityEngine;

public class ROSInterface : MonoBehaviour
{
    // ROS Connector
    private ROSConnection ros;

    // Variables required for ROS communication
    public string velocityMsgTopicName = "cmd_vel";
    public string odomTopicName = "odometry_frame";
    public string navigationTopicName = "move_base_simple/goal";
    public string scanTopicName = "laser_scan";

    public GameObject husky;
    public GameObject sensor;
    public GameObject target;

    private VelocityController controller;
    private LidarSensor lidar;

    private bool connected = false;
    private float timeElapsed = 0;
    private float publishMessageFrequency;

   
    void Start()
    {
        // Instantiate Husky Controller
        controller = gameObject.AddComponent<VelocityController>();
        controller.husky = husky;
        controller.Init();

        // Get reference to lidar sensor object
        lidar = sensor.GetComponent<LidarSensor>();
        publishMessageFrequency = (float) 1.0f / lidar.scansPerSec;

        // Get ROS connection static instance
        ConnectToRos();
    }

    public void ConnectToRos()
    {
        ros = ROSConnection.instance;
        ros.Subscribe<Twist>(velocityMsgTopicName, controller.ToWheelsSpeed);
        connected = true;
    }

    public void PublishTargetPosition()
    {
        var now = DateTime.Now;
        var header = new Header
        {
            seq = (uint)1, 
            stamp = new RosMessageTypes.Std.Time
            {
                secs = (uint)DateTimeOffset.Now.ToUnixTimeSeconds(),
                nsecs = 0,
            },
            frame_id = "map"
        };
        var targetPos = new PoseStamped
        {
            header = header,
            pose = new RosMessageTypes.Geometry.Pose
            {
                position = (target.transform.position).To<FLU>(),
                orientation = (target.transform.rotation).To<FLU>()
            }
        };
        ros.Send(navigationTopicName, targetPos);
    }

    public void publishOdometry()
    {
        var base_link = controller.GetBaseLinkTransform();
        var odomPose = new RosMessageTypes.Geometry.Pose
        {
            position = (base_link.position).To<FLU>(),
            orientation = (base_link.rotation).To<FLU>(),
        };
        ros.Send(odomTopicName, odomPose);
    }

    public void publishScan()
    {
        var scan = lidar.doScan();
        ros.Send(scanTopicName, scan);
    }

    public void Update()
    {
        if (connected)
        {
            // Apply cmd_vel to wheels if already received
            if (controller.receivedVelCommand)
            {
                controller.ApplyWheelsSpeed();
            }

            // Publish odometry and scan every X seconds
            timeElapsed += Time.deltaTime;
            if (timeElapsed > publishMessageFrequency)
            {
                publishOdometry();
                publishScan();
                timeElapsed = 0;
            }
        }
    }
}