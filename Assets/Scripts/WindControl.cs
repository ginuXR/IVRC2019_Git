using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindControl : MonoBehaviour
{
    public GameObject DirPicture;
    private float duration;

    void WindPower(float power)
    {
        //風の強さ変更
        this.GetComponent<WindZone>().windTurbulence = power;
    }

    void WindDirection(float[] direction)
    {
        //風の方向変更
        this.transform.localEulerAngles = new Vector3(0.0f, direction[1], 0.0f);

        //UIの矢印の方向
        DirPicture.transform.localEulerAngles = new Vector3(0.0f, 0.0f, -direction[1]);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<WindZone>().windTurbulence = 0.0f;
        this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

        //時間計測開始
        duration = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //フレーム書き換えごとに経過時間を累積
        duration += Time.deltaTime;

        float[] arr = { 0.0f, duration * 10, 0.0f };

        WindPower(duration);
        WindDirection(arr);
    }
}
