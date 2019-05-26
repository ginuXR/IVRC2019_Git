using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ReceiveFromArduino : MonoBehaviour
{

    public SerialHandler serialHandler;
    public Text text;

    private float duration;

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

            if (Sensor[0] == "Arduino")
            {
                text.text = "Arduino:" + Sensor[0];
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }
}