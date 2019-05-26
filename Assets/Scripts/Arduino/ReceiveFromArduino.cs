using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ReceiveFromArduino : MonoBehaviour
{

    public SerialHandler serialHandler;
    public Text text;
    public GameObject Cube;

    private float duration;
    public float ratio;
    private float[] angle = new float[4];
    private float[] pre_angle = new float[4];
    Vector3 calibration;

    // Use this for initialization
    void Start()
    {
        //信号を受信したときに、そのメッセージの処理を行う
        serialHandler.OnDataReceived += OnDataReceived;

        //時間計測開始
        duration = 0;

    }

    // Update is called once per frame
    void Update()
    {
        //フレーム書き換えごとに経過時間を累積
        duration += Time.deltaTime;
    }

    /*
	 * シリアルを受け取った時の処理
	 */
    void OnDataReceived(string message)
    {

        try
        {
            //Arduinoからの値をコンマ区切りでSensor[]にぶち込む
            string[] Sensor = message.Split(',');

            if (Sensor[0] == "quat")
            {


                //Arduinoから来た加速度センサの値をそのままSegwayにぶち込んでいる
                for (int i = 0; i < 4; i++)
                {
                    angle[i] = float.Parse(Sensor[i + 1]);
                    angle[i] = ratio * angle[i] + (1 - ratio) * pre_angle[i];
                    pre_angle[i] = angle[i];
                }

                //Vector3 EulerAngle = new Vector3(-angle[2], angle[0], angle[1]);
                //Cube.transform.localRotation = Quaternion.Euler(EulerAngle);

                Quaternion Qangle = new Quaternion(angle[3], angle[2], angle[0], angle[1]);
                Cube.transform.localRotation = Qangle;

                Vector3 EulerAngle = Cube.transform.localEulerAngles;

                //if (Input.GetKeyDown(KeyCode.C))
                //{
                //    //現在の値を保存する
                //    calibration.x = EulerAngle.x;
                //    calibration.y = EulerAngle.y;
                //    calibration.z = EulerAngle.z;
                //}

                EulerAngle.x = -EulerAngle.x - calibration[0];
                EulerAngle.y = -EulerAngle.y - calibration[1];
                EulerAngle.z = EulerAngle.z - calibration[2] - 180;
                Cube.transform.localRotation = Quaternion.Euler(EulerAngle);

                // シリアルの値をテキストに表示
                //text.text = "x:" + Xangle + ", " + "y:" + Yangle + "z:" + Zangle + "\n";
                text.text = "Arduino:" + Sensor[0];

            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
}