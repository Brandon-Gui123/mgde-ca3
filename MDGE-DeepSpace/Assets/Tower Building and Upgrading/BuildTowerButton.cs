using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTowerButton : MonoBehaviour
{
    public Transform buildLocation;

    public GameObject towerPrefab;

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
            bool tileHasTower = buildLocation.GetComponent<BuildingTile>().hasTower;
            if (!isTouchingUIFlower && pleaseBuildATower && !tileHasTower) {
                BuildTower(towerPrefab, buildLocation);
            }
        }
    }


    void BuildTower(GameObject towerPrefab, Transform buildLocation) {
        GameObject newTower = Instantiate(towerPrefab, buildLocation);
        buildLocation.GetComponent<BuildingTile>().hasTower = true;
        buildLocation.GetComponent<BuildingTile>().tower = newTower;
        Debug.Log("I built a tower called " + newTower.name);
    }
}
