using UnityEngine;
using System.IO.Ports;
using System;
using UnityEditor;

public class Move : MonoBehaviour
{
    [Tooltip("Sensitivity of acceleration vector (lenght of the black line.)")]
    [SerializeField] private float accelSensitivity = 1f;
    [Tooltip("Fix the rotation and put it to the position you need.")]
    [SerializeField] private Vector3 rotationOffset;
    [Tooltip("Object for moving the tip of the acceleration axis.")]
    [SerializeField] private Transform accelEndAxis;
    [Tooltip("Object needed to draw the acceleration line.")]
    [SerializeField] private LineRenderer lineRenderer;

    private SerialPort dataStream = new SerialPort("COM3", 9600);
    private string val;
    private Vector3 linealAccelAngle;
    private float W, X, Y, Z = 0.0f;
    private float newW, newX, newY, newZ = 0.0f;
    private float X1, Y1, Z1 = 0.0f;
    private float newX1, newY1, newZ1 = 0.0f;
    private bool read = false;

    // Start is called before the first frame update
    void Start()
    {
        dataStream.Open();
    }

    // Update is called once per frame
    void Update()
    {
        val = dataStream.ReadLine();

        try
        {
            // Check if there is a new value to read.
            if (val != null && read)
            {
                /* 
                 * Split text line by separators to get individual values and turn them into floats.
                 * Example of expected text line:
                 * X:0.03,Y:-0.01,Z:0.26@W:0.9999,X:-0.0131,Y:0.0019,Z:0.0000
                 * 
                 * Left of the @ is for accel and right of the @ is for quaternion.
                */
                string[] split = val.Split('@');
                string[] value_1 = split[0].Split(',');
                string[] value_2 = split[1].Split(',');

                // Convert all accel values to floats.
                float[] xyz = new float[3];
                for (int i = 0; i < value_1.Length; i++)
                {
                    string[] keyValue = value_1[i].Split(':');
                    xyz[i] = float.Parse(keyValue[1]);
                }
                newX1 = xyz[0];
                newY1 = xyz[1];
                newZ1 = xyz[2];

                // Convert all quaternion values to floats.
                float[] wxyz = new float[4];
                for (int i = 0; i < value_2.Length; i++)
                {
                    string[] keyValue = value_2[i].Split(':');
                    wxyz[i] = float.Parse(keyValue[1]);
                }
                newW = wxyz[0];
                newX = wxyz[1];
                newY = wxyz[2];
                newZ = wxyz[3];
            }
        }
        catch (Exception)
        {
            Debug.Log("Ocurrió un error");
        }


        try
        {
            // Check if the difference between one capture and the other is large enough.
            if (val != null && read)
            {
                if (Math.Abs(X1 - newX1) > 0.1)
                {
                    X1 = newX1;
                }
                if (Math.Abs(Y1 - newY1) > 0.1)
                {
                    Y1 = newY1;
                }
                if (Math.Abs(Z1 - newZ1) > 0.1)
                {
                    Z1 = newZ1;
                }

                if (Math.Abs(W - newW) > 0.01)
                {
                    W = newW;
                }
                if (Math.Abs(X - newX) > 0.01)
                {
                    X = newX;
                }
                if (Math.Abs(Y - newY) > 0.01)
                {
                    Y = newY;
                }
                if (Math.Abs(Z - newZ) > 0.01)
                {
                    Z = newZ;
                }
            }
        }
        catch (Exception)
        {
            Debug.Log("Ocurrió un error en la rotación");
        }

        // Apply rotation to the model.
        transform.localRotation = new Quaternion(X, Y, Z, W);
        read = true;

        transform.parent.transform.eulerAngles = rotationOffset;

        // Applay acceleration and draw line.
        linealAccelAngle = new Vector3(X1, Y1, Z1);
        accelEndAxis.transform.localPosition = linealAccelAngle * accelSensitivity;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, accelEndAxis.position);

        //rotationAngleTheta = (float)Math.Acos(Vector3.Dot(accel_Axis.transform.rotation.eulerAngles, linealAccelAngle) / (Vector3.Magnitude(accel_Axis.transform.rotation.eulerAngles) * magnitud));
        //rotationAxis = Vector3.Cross(accel_Axis.transform.rotation.eulerAngles, linealAccelAngle);
    }
}
