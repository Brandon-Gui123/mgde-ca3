using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTile : MonoBehaviour
{
    [HideInInspector]
    public bool hasTower = false;

    [HideInInspector]
    //the tower the tile contains
    public GameObject tower;

    public List<GameObject> ListOfButtons = new List<GameObject>();
    public List<GameObject> ListOfUpgrades = new List<GameObject>();
    public float buttonAngleSpacing = 30f;
    public float distanceFromOrigin = 2f;
    public float UIOpenTime = 15f;
    public float UICloseTime = 10f;

    bool openUI = false;
    bool openUpgrades = false;

    void OnValidate() {
        ShowButtons(ListOfButtons);
        ShowButtons(ListOfUpgrades);
    }

    void Update() {
        if (Input.touchCount > 0) {
            Touch[] tArr = Input.touches;
            for (int i = 0; i < tArr.Length; i++) {
                RaycastHit2D[] hitInfo = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(tArr[i].position), transform.forward);
                if(hitInfo.Length == 0){
                    openUI = false;
                    openUpgrades = false;
                } else {
                    for (int h = 0; h < hitInfo.Length; h++) {
                        if (tArr[i].phase.Equals(TouchPhase.Began)) {
                            if (hitInfo[h].collider.gameObject.Equals(gameObject)) {
                                openUI = !openUI;
                                openUpgrades = !openUpgrades;
                                break;
                            } else {
                                openUI = false;
                                openUpgrades = false;
                            }
                        }                    
                    }
                }                
            }
        }
        if (!hasTower) {
            if (openUI) {
                ShowButtons(ListOfButtons);
            } else {
                HideButtons(ListOfButtons);
            }
        } else {
            if (openUpgrades) {
                ShowButtons(ListOfUpgrades);
            } else {
                HideButtons(ListOfUpgrades);
            }
        }
        
    }


    void ShowButtons(List<GameObject> ListOfButtons) {
        float angleMultiplier = -ListOfButtons.Count / 2f + 0.5f;
        for (int i = 0; i < ListOfButtons.Count; i++) {
            ListOfButtons[i].transform.position = Vector3.Lerp(ListOfButtons[i].transform.position, transform.position + Quaternion.AngleAxis(buttonAngleSpacing * (angleMultiplier + i), Vector3.back) * transform.up * distanceFromOrigin, Time.deltaTime * UICloseTime);
            ListOfButtons[i].SetActive(true);
        }
    }
    void HideButtons(List<GameObject> ListOfButtons) {
        foreach (GameObject g in ListOfButtons) {
            g.transform.position = Vector3.Lerp(g.transform.position, transform.position, Time.deltaTime * UIOpenTime);
            StartCoroutine(HideButton(g, 0.2f));
        }
    }
    IEnumerator HideButton(GameObject button, float delay) {
        yield return new WaitForSeconds(delay);
        button.SetActive(false);
    }
    IEnumerator ToggleButtonActive(bool enable, List<GameObject> ListOfButtons) {
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
