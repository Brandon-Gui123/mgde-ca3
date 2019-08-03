using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBeam : MonoBehaviour
{
    LineRenderer beamRenderer;

    Vector2 startingPoint;
    public float beamStartWidth = 0.2f, beamEndWidth = 0.2f;
    public float beamDuration = 0.5f;
    public float maxChainCount = 5f;
    public GameObject[] inRange;

    void Start() {
        //Line renderer validation
        if (!GetComponent<LineRenderer>()) {
            gameObject.AddComponent<LineRenderer>();
        }
        beamRenderer = GetComponent<LineRenderer>();

    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(transform.position, 3);
    }

    void Update() {
        inRange = ScanRadius(transform.position, 3, 4);
        DrawBeamBranch(transform.position, 3, 4);
    }
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
    GameObject[] ScanRadius(Vector2 origin, float scanRadius, float returnCount) {
        Collider2D[] scannedColliders = Physics2D.OverlapCircleAll(origin, scanRadius, Physics2D.AllLayers);

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
