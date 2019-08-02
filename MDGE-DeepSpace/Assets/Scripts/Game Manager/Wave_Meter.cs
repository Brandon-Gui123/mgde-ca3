using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Wave_Meter : MonoBehaviour
{
    public float Duration = 60;
    public Image Progress_Item;
    public Image Progress_Bar;

    private float currentTime = 0;
    private float unitsperSecond; //use to move icon per second based on total length of progress bar
    private float timer = 0;


    // Start is called before the first frame update
    void Start()
    {

        unitsperSecond = Progress_Bar.rectTransform.sizeDelta.x / Duration; //1 second equal to how much bar;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; //check for a second

        if (timer >= 1) //update per second to keep in line with the unit per second
        {
            float currentPosition = Progress_Item.rectTransform.localPosition.x; //check current position
            updateProgressbar(currentPosition + (unitsperSecond)); //update the bar by the unit seconds
            timer = 0;
        }
    }

    public void updateProgressbar(float shift)
    {
        Progress_Item.rectTransform.localPosition = new Vector3(shift, 0, 0); //shift the meter
    }
}
