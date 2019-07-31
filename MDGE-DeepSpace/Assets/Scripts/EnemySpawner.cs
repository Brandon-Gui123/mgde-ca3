﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    /// <summary>
    /// The possible sides of a rectangle.
    /// </summary>
    private enum SpawnSide
    {
        Top = 0,
        Right = 1,
        Bottom = 2,
        Left = 3
    }

    /// <summary>
    /// Whether this spawner can spawn enemies.
    /// </summary>
    public bool canSpawn = true;

    /// <summary>
    /// The <see cref="Transform"/> component of the map <see cref="GameObject"/>.
    /// This is used to get the lossy scale of the map.
    /// We use <see cref="Transform"/> because it is a reference and if our map size changes,
    /// we can sync with it as well.
    /// </summary>
    [SerializeField]
    private Transform mapTransform;

    /// <summary>
    /// The distance that enemies will be from the edge of the map when they spawn.
    /// </summary>
    public float extraDistance;

    /// <summary>
    /// The time that the spawner will stop creating enemies before it creates one.
    /// </summary>
    public float delayPerSpawn;

    /// <summary>
    /// Time left before the next spawn.
    /// </summary>
    private float spawnCountdown;

    /// <summary>
    /// The <see cref="GameObject"/> that is used as an enemy.
    /// </summary>
    public GameObject enemy;

    private Sprite mapSprite;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (canSpawn)
        {
            DoSpawningProcess();
        }
    }

    private void OnDrawGizmos()
    {
        //draw a the outline of a rectangle showing where enemies will spawn
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(mapTransform.position, mapTransform.lossyScale + Vector3.one * extraDistance);
    }

    private Vector2 PickSpawnLocation()
    {

        //calculate map size and map position
        Vector3 spawnSize = mapTransform.lossyScale + Vector3.one * extraDistance;
        Vector3 mapPosition = mapTransform.position;

        //choose which side of the rectangle to spawn
        SpawnSide side = (SpawnSide)Random.Range(0, 4);

        //position vector declarations
        float positionX = 0;
        float positionY = 0;

        switch (side)
        {

            case SpawnSide.Bottom:
            case SpawnSide.Top:

                //randomly choose an x-coordinate value
                positionX = Random.Range(mapPosition.x - spawnSize.x / 2, mapPosition.x + spawnSize.x / 2);

                //depending on whether it is the top or bottom, set the y-coordinate value as needed
                positionY = (side == SpawnSide.Bottom) ? mapPosition.y - spawnSize.y / 2 : mapPosition.y + spawnSize.y / 2;

                break;

            case SpawnSide.Left:
            case SpawnSide.Right:

                //depending on whether it is left or right, set the x-coordinate as needed
                positionX = (side == SpawnSide.Left) ? mapPosition.x - spawnSize.x / 2 : mapPosition.x + spawnSize.x / 2;

                //randomly choose an y-coordinate value
                positionY = Random.Range(mapPosition.y - spawnSize.y / 2, mapPosition.y + spawnSize.y / 2);

                break;
        }

        return new Vector2(positionX, positionY);
    }

    private void DoSpawningProcess()
    {

        //countdown the spawn time
        spawnCountdown -= Time.deltaTime;

        if (spawnCountdown <= 0)
        {
            SpawnEnemy();
        }

    }

    private void SpawnEnemy()
    {
        Vector2 spawnLocation = PickSpawnLocation();
        Instantiate(enemy, spawnLocation, Quaternion.identity);
    }
}
