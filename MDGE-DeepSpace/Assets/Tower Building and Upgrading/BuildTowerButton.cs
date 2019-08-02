using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTowerButton : MonoBehaviour
{
    public Transform buildLocation;

    public GameObject tower;

    //if touch down and up on that button, build

    void Update() {
        if (Input.touchCount > 0) {
            Touch[] tArr = Input.touches;
            bool isTouchingUIFlower = false;
            bool pleaseBuildATower = false;
            for (int i = 0; i < tArr.Length; i++) {
                RaycastHit2D[] hitInfo = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(tArr[i].position), transform.forward);
                for (int h = 0; h < hitInfo.Length; h++) {
                    if (tArr[i].phase.Equals(TouchPhase.Began)) {
                        if (hitInfo[h].collider.gameObject.Equals(gameObject)) {
                            pleaseBuildATower = true;
                        } else if (hitInfo[h].collider.gameObject.Equals(buildLocation.gameObject)) {
                            isTouchingUIFlower = true;
                        }
                    }
                }
            }
            if (!isTouchingUIFlower && pleaseBuildATower) {
                BuildTower(tower, buildLocation);
            }
        }
    }


    void BuildTower(GameObject tower, Transform buildLocation) {
        Instantiate(tower, buildLocation);
        buildLocation.GetComponent<BuildingTile>().hasTower = true;
        Debug.Log("I build a tower" + gameObject.name);
    }
}
