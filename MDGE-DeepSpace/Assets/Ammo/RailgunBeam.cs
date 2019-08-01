using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailgunBeam : MonoBehaviour
{
    LineRenderer beamRenderer;

    Vector2 startingPoint;
    public float beamExtendSpeed = 175f;
    public float beamWidth = 0.2f;
    public float beamThickenSpeed = 0.5f;
    public float beamDuration = 0.5f;

    float currentBeamLength;
    float currentBeamWidth;

    public bool fireRailgunShot = false;

    void Start() {
        //Line renderer validation
        if (!GetComponent<LineRenderer>()) {
            gameObject.AddComponent<LineRenderer>();
        }
        beamRenderer = GetComponent<LineRenderer>();

        //Line renderer setup
        ResetBeam();
    }
    void Update() {
        startingPoint = transform.position;
        beamRenderer.SetPosition(0, startingPoint);
        if (fireRailgunShot) {
            CastBeam();
        }
        else {
            ResetBeam();
        }
    }

    void CastBeam() {
        //Beam length increases with time
        currentBeamLength += Time.deltaTime * beamExtendSpeed;
        currentBeamWidth += Time.deltaTime * beamWidth * beamThickenSpeed;
        //Set the position of the end of the beam
        beamRenderer.SetPosition(1, startingPoint + (Vector2)transform.up * currentBeamLength);
        beamRenderer.startWidth = currentBeamWidth;
        beamRenderer.endWidth = currentBeamWidth;
        GameObject[] langgar = GameObjectsInLine();
        for (int i = 0; i < langgar.Length; i++) {
            Debug.Log(langgar[i]);//deal damage method
        }
    }
    public GameObject[] GameObjectsInLine() {
        return InLine(transform.position, transform.up, currentBeamLength, currentBeamWidth);
    }
    public GameObject[] InLine(Vector2 origin, Vector2 direction, float distance, float width) {
        RaycastHit2D[] hitInfos = Physics2D.BoxCastAll(origin, new Vector2(width, width), transform.eulerAngles.z, direction, distance);
        GameObject[] hitObjects = new GameObject[hitInfos.Length];
        for (int i = 0; i < hitObjects.Length; i++) {
            hitObjects[i] = hitInfos[i].collider.gameObject;
        }
        return hitObjects;
    }

    void ResetBeam() {
        startingPoint = transform.position;
        beamRenderer.positionCount = 2;
        beamRenderer.SetPosition(0, startingPoint);
        beamRenderer.SetPosition(1, startingPoint);
        currentBeamWidth = beamWidth;
        beamRenderer.startWidth = beamWidth;
        beamRenderer.endWidth = beamWidth;
        currentBeamLength = 0f;
    }
}
