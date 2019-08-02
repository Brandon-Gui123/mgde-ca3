using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellTowerButton : MonoBehaviour
{
    public GameObject tower;
    
    public Transform buildLocation;
    
    void Update()
    {
        if (Input.touchCount > 0) {
            Touch[] tArr = Input.touches;
            bool isTouchingTurret = false;
            bool pleasSellTower = false;
            for (int i = 0; i < tArr.Length; i++) {
                RaycastHit2D[] hitInfo = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(tArr[i].position), transform.forward);
                for (int h = 0; h < hitInfo.Length; h++) {
                    if (tArr[i].phase.Equals(TouchPhase.Began)) {
                        if (hitInfo[h].collider.gameObject.Equals(gameObject)) {
                            pleasSellTower = true;
                        } else if (hitInfo[h].collider.gameObject.Equals(tower)) {
                            isTouchingTurret = true;
                        }
                    }
                }
            }
            if (!isTouchingTurret && pleasSellTower) {
                SellTower();
            }
        }
    }
    void SellTower() {
        //Destroy(tower);
        buildLocation.GetComponent<BuildingTile>().hasTower = false;
        Debug.Log("I sold a turret");
    }
}
