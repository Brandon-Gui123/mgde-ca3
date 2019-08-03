using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private bl_Joystick Joystick;//Joystick reference for assign in inspector

    [SerializeField] private float Speed = 5;
    
    void Update()
    {
        //Step #2
        //Change Input.GetAxis (or the input that you using) to Joystick.Vertical or Joystick.Horizontal
        float v = Joystick.Vertical; //get the vertical value of joystick
        float h = Joystick.Horizontal;//get the horizontal value of joystick

        //in case you using keys instead of axis (due keys are bool and not float) you can do this:
        //bool isKeyPressed = (Joystick.Horizontal > 0) ? true : false;

        //ready!, you not need more.
    
;
        if ( Input.touchCount > 0|| Input.GetKey(KeyCode.Mouse0))
        {
            float angle = Mathf.Atan2(h, v) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, -angle));

          
            
                Vector3 translate = Vector3.up * Speed * Time.deltaTime;
                transform.Translate(translate);
            
        

        }
        
       
    }
}
