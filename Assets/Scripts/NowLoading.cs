using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NowLoading : MonoBehaviour
{
    public float Speed;
    public float BackSpeed;
    private float timeElapsed;

    public Text LoadNum;
    public GameObject Back;

    IEnumerator Progress()
    {
        //メータの進み具合
        for (int i = 0; i <= 1000; i++)
        {
            GetComponent<Image>().fillAmount = i * Speed / 500.0f;

            LoadNum.GetComponent<Text>().text = "" + (int)(GetComponent<Image>().fillAmount * 100);
            yield return new WaitForSeconds(0.01f);
        }
    }

    void Start()
    {
        StartCoroutine(Progress());
    }

    void FixedUpdate()
    {
        timeElapsed += Time.deltaTime;
        Back.transform.localEulerAngles = new Vector3(0, 0, timeElapsed * 500 * BackSpeed);
    }
}
