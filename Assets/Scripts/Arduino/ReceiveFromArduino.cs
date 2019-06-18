using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ReceiveFromArduino : MonoBehaviour
{

    public SerialHandler serialHandler;
    public GameObject FIRE_HOSE;
    public GameObject LoadingUI;

    private float duration;
    private float ratio = 0.5f;
    private float[] angle = new float[4];
    private float[] pre_angle = new float[4];
    private float[] calibrate_angles = new float[3];

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

        Quaternion Qangle = new Quaternion(angle[3], angle[2], angle[0], angle[1]);
        FIRE_HOSE.transform.localRotation = Qangle;

        Vector3 EulerAngle = FIRE_HOSE.transform.localEulerAngles;

        EulerAngle.x = -EulerAngle.x - calibrate_angles[0];
        EulerAngle.y = -EulerAngle.y - calibrate_angles[1];
        EulerAngle.z = EulerAngle.z - 180 - calibrate_angles[2];
        FIRE_HOSE.transform.localRotation = Quaternion.Euler(EulerAngle);

        if (Input.GetKeyDown(KeyCode.C))
        {
            //現在の値を保存する
            calibrate_angles[0] += FIRE_HOSE.transform.localEulerAngles.x;
            calibrate_angles[1] += FIRE_HOSE.transform.localEulerAngles.y;
            calibrate_angles[2] += FIRE_HOSE.transform.localEulerAngles.z;

            LoadingUI.SetActive(false);
        }
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
                //Arduinoから来た加速度センサの値をそのままぶち込んでいる
                for (int i = 0; i < 4; i++)
                {
                    angle[i] = float.Parse(Sensor[i + 1]);
                    angle[i] = ratio * angle[i] + (1 - ratio) * pre_angle[i];
                    pre_angle[i] = angle[i];
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
}