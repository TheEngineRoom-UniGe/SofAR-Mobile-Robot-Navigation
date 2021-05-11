using System;
using UnityEngine;
using Scan = RosMessageTypes.Sensor.LaserScan;
using Header = RosMessageTypes.Std.Header;
using Time = RosMessageTypes.Std.Time;

public class LidarSensor : MonoBehaviour
{
    // Sensor parameters
    public float arcAngle;
    public int numLines;
    public float maxDist;
    public int scansPerSec;

    private float[] ranges;

    private float angleMin;
    private float angleMax;
    private float angleIncrement;

    private float scanFreq { get; set; }

    public void Start()
    {
        scanFreq = 1.0f / scansPerSec;

        angleMin = -arcAngle / 2 + arcAngle / (2 * numLines);
        angleIncrement = arcAngle / numLines;
        angleMax = -arcAngle / 2 + ((numLines - 1) * angleIncrement) + arcAngle / (2 * numLines);
        
    }

    public Scan doScan()
    {
        ranges = new float[numLines];
        var color = Color.red;
        color.a = 0.3f;
        for (int l = 0; l < numLines; l++)
        {
            var shootVec = transform.rotation * Quaternion.AngleAxis(-1 * arcAngle / 2 + (l * arcAngle / numLines) + arcAngle / (2 * numLines), Vector3.up) * Vector3.forward;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, shootVec, out hit, maxDist))
            {
                Debug.DrawLine(transform.position, hit.point, color, 0.2f);
                ranges[l] = hit.distance;
            }
            else ranges[l] = maxDist;
        }
        
        DateTime now = DateTime.Now;
        var scanMsg = new Scan
        {
            header = new Header
            {
                seq = (uint)1,
                stamp = new Time
                {
                    secs = (uint)DateTimeOffset.Now.ToUnixTimeSeconds(),
                    nsecs = 0,
                },
                frame_id = "base_laser"
            },
            angle_min = angleMin*(float)Math.PI/180.0f,
            angle_max = angleMax * (float)Math.PI / 180.0f,
            angle_increment = angleIncrement * (float)Math.PI / 180.0f,
            time_increment = scanFreq,
            scan_time = scanFreq,
            range_min = 0,
            range_max = maxDist,
            ranges = ranges
        };
        return scanMsg;
    }
}
