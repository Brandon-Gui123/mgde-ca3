using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTile : MonoBehaviour
{
    [HideInInspector]
    public bool hasTower = false;

    public List<GameObject> ListOfButtons = new List<GameObject>();
    public float buttonAngleSpacing = 30f;
    public float distanceFromOrigin = 2f;
    public float UIOpenTime = 15f;
    public float UICloseTime = 10f;

    bool openUI = false;

    void OnValidate() {
        ShowButtons();
    }

    void Update() {
        if (Input.touchCount > 0) {
            Touch[] tArr = Input.touches;
            for (int i = 0; i < tArr.Length; i++) {
                RaycastHit2D[] hitInfo = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(tArr[i].position), transform.forward);
                if(hitInfo.Length == 0){
                    openUI = false;
                } else {
                    for (int h = 0; h < hitInfo.Length; h++) {
                        if (tArr[i].phase.Equals(TouchPhase.Began)) {
                            if (hitInfo[h].collider.gameObject.Equals(gameObject)) {
                                openUI = !openUI;
                                break;
                            } else {
                                openUI = false;
                            }
                        }                    
                    }
                }                
            }
            if (openUI) {
                StartCoroutine(ToggleButtonActive(openUI));
            } else {
                for (int i = 0; i < tArr.Length; i++) {
                    if (tArr[i].phase.Equals(TouchPhase.Began)) {
                        StartCoroutine(ToggleButtonActive(openUI));
                        break;
                    }
                }
                    
            }
        }
        if (!hasTower) {
            if (openUI) {
                ShowButtons();
                StartCoroutine(ToggleButtonActive(true));
            } else {
                HideButtons();
                StartCoroutine(ToggleButtonActive(openUI));
            }
        }
        
    }


    void ShowButtons() {
        float angleMultiplier = -ListOfButtons.Count / 2f + 0.5f;
        for (int i = 0; i < ListOfButtons.Count; i++) {
            ListOfButtons[i].transform.position = Vector3.Lerp(ListOfButtons[i].transform.position, transform.position + Quaternion.AngleAxis(buttonAngleSpacing * (angleMultiplier + i), Vector3.back) * transform.up * distanceFromOrigin, Time.deltaTime * UICloseTime);
        }
    }
    void HideButtons() {
        foreach (GameObject g in ListOfButtons) {
            g.transform.position = Vector3.Lerp(g.transform.position, transform.position, Time.deltaTime * UIOpenTime);
        }
    }
    IEnumerator ToggleButtonActive(bool enable) {
        if (enable) {
            yield return new WaitForSeconds(Time.deltaTime);
        } else {
            yield return new WaitForSeconds(UICloseTime * Time.deltaTime);
        }
        foreach (GameObject button in ListOfButtons) {
            button.SetActive(enable);
        }
    }
}
