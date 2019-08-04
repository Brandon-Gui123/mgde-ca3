﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTowerButton : MonoBehaviour
{
    [HideInInspector]
    public GameObject tower;

    public Transform buildLocation;

    public List<Sprite> UpgradeButtonSpriteTiers = new List<Sprite>();
    SpriteRenderer currentUpgradeButtonTierSprite;

    int currentUpgradeTier = 0;

    float baseUpgradeCost = 3f;

    void Start() {
        currentUpgradeButtonTierSprite = GetComponent<SpriteRenderer>();
    }

    void Update() {
        tower = buildLocation.GetComponent<BuildingTile>().tower;


        if (Input.touchCount > 0) {
            Touch[] tArr = Input.touches;
            bool isTouchingTurret = false;
            bool pleaseUpgradeTower = false;
            for (int i = 0; i < tArr.Length; i++) {
                RaycastHit2D[] hitInfo = Physics2D.RaycastAll(Camera.main.ScreenToWorldPoint(tArr[i].position), transform.forward);
                for (int h = 0; h < hitInfo.Length; h++) {
                    if (tArr[i].phase.Equals(TouchPhase.Began)) {
                        if (hitInfo[h].collider.gameObject.Equals(gameObject)) {
                            pleaseUpgradeTower = true;
                        } else if (hitInfo[h].collider.gameObject.Equals(tower)) {
                            isTouchingTurret = true;
                        }
                    }
                }
            }
            if (!isTouchingTurret && pleaseUpgradeTower) {
                UpgradeTower();
            }
        }
    }
    void UpgradeTower() {
        if (tower) {
            Turret turretSettings = tower.GetComponent<Turret>();
            currentUpgradeTier++;
            SetUpgradeButtonSprite(currentUpgradeTier);
            //player's money -= tier * baseUpgradeCost
            //turretSettings.range *= 1.1f;

            Debug.Log("I upgraded a turret called " + tower.name);
        } else {
            Debug.Log("UpgradeButtonError: no turret to upgrade");
        }
        
    }
    void SetUpgradeButtonSprite(int spriteTier) {
        if (spriteTier < UpgradeButtonSpriteTiers.Count) {
            currentUpgradeButtonTierSprite.sprite = UpgradeButtonSpriteTiers[spriteTier];
        } else {
            currentUpgradeButtonTierSprite.sprite = null;
        }
       
    }
}