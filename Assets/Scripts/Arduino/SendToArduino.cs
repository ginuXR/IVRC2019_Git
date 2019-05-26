using UnityEngine;
using System.Collections;

public class SendToArduino : MonoBehaviour
{
    public SerialHandler serialHandler;
    private float duration;
    private bool start;

    private void Start()
    {
        //時間計測開始
        duration = 0;

        start = true;
    }

    void Update()
    {

        //フレーム書き換えごとに経過時間を累積
        duration += Time.deltaTime;

        //Arduinoに文字を送る

        if (duration > 2.0f && start)
        {
            serialHandler.Write("0");
            start = false;
        }

        //キー入力
        if (Input.GetKeyDown(KeyCode.S))
        {
            serialHandler.Write("0");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            serialHandler.Write("1");
        }

    }
}