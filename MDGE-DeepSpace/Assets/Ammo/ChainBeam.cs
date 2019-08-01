using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBeam : MonoBehaviour
{
    LineRenderer beamRenderer;

    Vector2 startingPoint;
    public float beamStartWidth = 0.2f, beamEndWidth = 0.2f;
    public float beamDuration = 0.5f;

    //gameobjects in radius
    public GameObject[] inRange;

    public float detectionRange = 3f;//radius
    public float maxTargetsToHit = 4f;

    void Start() {
        //Line renderer validation
        if (!GetComponent<LineRenderer>()) {
            gameObject.AddComponent<LineRenderer>();
        }
        beamRenderer = GetComponent<LineRenderer>();

    }
    private void OnDrawGizmos() {
        //
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    void Update() {
        inRange = ScanRadius(transform.position, detectionRange, maxTargetsToHit);
        DrawBeamBranch(transform.position, detectionRange, maxTargetsToHit);
    }

    //draw lineRenderer across objects in radius
    void DrawBeamBranch(Vector2 origin, float scanRadius, float returnCount) {
        GameObject[] bunchOfTargets = ScanRadius(origin,  scanRadius, returnCount);
        beamRenderer.positionCount = bunchOfTargets.Length + 1;
        beamRenderer.SetPosition(0, transform.position);
        beamRenderer.startWidth = beamStartWidth;
        beamRenderer.endWidth = beamEndWidth;
        for (int i = 0; i < bunchOfTargets.Length; i++) {
            beamRenderer.SetPosition(i + 1, bunchOfTargets[i].transform.position);
        }
    }

    //returns nearest few(returnCount) objects
    GameObject[] ScanRadius(Vector2 origin, float scanRadius, float returnCount) {
        Collider2D[] scannedColliders = Physics2D.OverlapCircleAll(origin, scanRadius, Physics2D.AllLayers);//change the layermask

        GameObject[] scannedObjects = new GameObject[scannedColliders.Length];
        for (int i = 0; i < scannedColliders.Length; i++) {
            scannedObjects[i] = scannedColliders[i].gameObject;
        }

        float[] distancesFromOrigin = new float[scannedColliders.Length];
        for (int i = 0; i < scannedColliders.Length; i++) {
            distancesFromOrigin[i] = Vector2.Distance(scannedColliders[i].transform.position, origin);
        }

        List<int> ignoredIndex = new List<int>();

        for (int i = 0; i < returnCount; i++) {
            List<int> tempMask = ignoredIndex;
            ignoredIndex.Add(FindMinIndex(distancesFromOrigin, tempMask));
        }
        List<GameObject> toBeReturned = new List<GameObject>();
        if (ignoredIndex[0]>-1) {
            for (int i = 0; i < ignoredIndex.Count && i < distancesFromOrigin.Length; i++) {
                toBeReturned.Add(scannedObjects[ignoredIndex[i]]);
            }
        }
        return toBeReturned.ToArray();        
    }

    //finds the lowest value (closest distance) from a floatArray, not considering the elements with indexes in the 'iterator mask'
    int FindMinIndex(float[] listOfFloats, List<int> iteratorMask) {
        float nextLowest = Mathf.Max(listOfFloats);
        int index = -1;
        for (int i = 0; i < listOfFloats.Length; i++) {
            //check ignore indexes
            bool skip = false;
            for (int j = 0; j < iteratorMask.Count; j++) {
                if (i == iteratorMask[j]) {
                    skip = true;
                    break;
                }
            }
            if (!skip) {
                if (listOfFloats[i] <= nextLowest) {
                    nextLowest = listOfFloats[i];
                    index = i;
                }
            }            
        }
        return index;
    }

}
