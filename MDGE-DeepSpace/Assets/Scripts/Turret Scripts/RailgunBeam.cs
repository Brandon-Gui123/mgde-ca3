﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunBeam : MonoBehaviour
{

    /// <summary>
    /// The script component of the railgun turret.
    /// </summary>
    [SerializeField]
    private RailgunTurret railgunTurret;

    /// <summary>
    /// The time left before all enemies in the beam will get damaged.
    /// </summary>
    private float beamDamageIntervalCountdown;

    /// <summary>
    /// Whether or not the beam is allowed to damage enemies.
    /// </summary>
    public bool canDamageEnemies;

    /// <summary>
    /// Do we damage enemies in this frame?
    /// </summary>
    private bool doDamageEnemies;

    /// <summary>
    /// The LineRenderer responsible for displaying the beam.
    /// </summary>
    private LineRenderer beamLineRenderer;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        beamDamageIntervalCountdown = railgunTurret.beamDamageInterval;

        if (!beamLineRenderer)
        {
            beamLineRenderer = GetComponent<LineRenderer>();
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (canDamageEnemies)
        {
            //do countdown
            beamDamageIntervalCountdown -= Time.deltaTime;

            if (beamDamageIntervalCountdown <= 0)
            {
                //damage each enemy in the beam
                foreach (EnemyController enemy in GetEnemiesInBeam())
                {
                    enemy.Damage(railgunTurret.beamDamage);
                }

                //reset countdown
                beamDamageIntervalCountdown = railgunTurret.beamDamageInterval;
            }

            //display the railgun beam
            beamLineRenderer.SetPositions(
                new Vector3[] { Vector3.zero, new Vector3(railgunTurret.beamRange, 0, 0) }
            );
        }
        else
        {
            beamLineRenderer.SetPositions(
                new Vector3[] { Vector3.zero, new Vector3(0, 0, 0) }
            );
        }
    }

    private EnemyController[] GetEnemiesInBeam()
    {
        RaycastHit2D[] hitInfos =
            Physics2D.BoxCastAll(
                transform.position,
                Vector2.one * beamLineRenderer.startWidth,
                0,
                transform.right,
                railgunTurret.beamRange,
                LayerMask.GetMask("Enemy")
            );

        EnemyController[] enemyControllers = new EnemyController[hitInfos.Length];

        for (int i = 0; i < hitInfos.Length; i++)
        {
            enemyControllers[i] = hitInfos[i].collider.GetComponent<EnemyController>();
        }

        return enemyControllers;
    }

}