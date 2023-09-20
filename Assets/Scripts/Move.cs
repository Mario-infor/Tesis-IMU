using UnityEngine;
using System.IO.Ports;
using System;
using UnityEditor;

public class Move : MonoBehaviour
{
    SerialPort data_stream = new SerialPort("COM3", 9600);
    private string val;
    private Vector3 linealAccelAngle;
    private float W, X, Y, Z = 0.0f;
    private float newW, newX, newY, newZ = 0.0f;
    private float X_1, Y_1, Z_1 = 0.0f;
    private float newX_1, newY_1, newZ_1 = 0.0f;
    private bool read = false;
   
    public float accelSensitivity = 1f;
    public Vector3 rotationOffset;
    public Transform accel_end_Axis;
    public LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        data_stream.Open();
    }

    // Update is called once per frame
    void Update()
    {
        val = data_stream.ReadLine();

        try
        {
            if (val != null && read)
            {
                string[] split = val.Split('@');
                string[] value_1 = split[0].Split(',');
                string[] value_2 = split[1].Split(',');

                float[] xyz = new float[3];
                for (int i = 0; i < value_1.Length; i++)
                {
                    string[] keyValue = value_1[i].Split(':');
                    xyz[i] = float.Parse(keyValue[1]);
                }
                newX_1 = xyz[0];
                newY_1 = xyz[1];
                newZ_1 = xyz[2];

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
            if (val != null && read)
            {
                if (Math.Abs(X_1 - newX_1) > 0.1)
                {
                    X_1 = newX_1;
                }
                if (Math.Abs(Y_1 - newY_1) > 0.1)
                {
                    Y_1 = newY_1;
                }
                if (Math.Abs(Z_1 - newZ_1) > 0.1)
                {
                    Z_1 = newZ_1;
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

        //Debug.Log("X: " + X + " , " + "Y: " + Y + " , " + "Z: " + Z);

        transform.localRotation = new Quaternion(X, Y, Z, W);
        read = true;

        transform.parent.transform.eulerAngles = rotationOffset;

        //Debug.Log("X_1: " + X_1 + " , " + "Y_1: " + Y_1 + " , " + "Z_1: " + Z_1);
        linealAccelAngle = new Vector3(X_1, Y_1, Z_1);
        accel_end_Axis.transform.localPosition = linealAccelAngle * accelSensitivity;

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, accel_end_Axis.position);

        //rotationAngleTheta = (float)Math.Acos(Vector3.Dot(accel_Axis.transform.rotation.eulerAngles, linealAccelAngle) / (Vector3.Magnitude(accel_Axis.transform.rotation.eulerAngles) * magnitud));
        //rotationAxis = Vector3.Cross(accel_Axis.transform.rotation.eulerAngles, linealAccelAngle);
    }
}
