using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainBeam : Projectile
{
    //ProjectileProperties
    public float projectileDamage = 1f;
    public float projectileDamageRate = 0.5f;
    public float projectileLifetime = 5f;
    public Animator animator;
    
    LineRenderer beamRenderer;


    Vector2 startingPoint;
    public float beamStartWidth = 0.2f, beamEndWidth = 0.2f;
    public float beamDuration = 0.5f;

    //gameobjects in radius
    public GameObject[] inRange;

    public float detectionRange = 3f;//radius
    public float maxTargetsToHit = 4f;
    
    public bool renderLine = false;

    void Start() {
        //Line renderer validation
        if (!GetComponent<LineRenderer>()) {
            gameObject.AddComponent<LineRenderer>();
        }
        beamRenderer = GetComponent<LineRenderer>();
        StartCoroutine(UnRender(beamDuration));
    }
    private void OnDrawGizmos() {
        //
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
    
    void Update() {
        if (renderLine) {
            inRange = ScanRadius(transform.position, detectionRange, maxTargetsToHit);
            DrawBeamBranch(transform.position, detectionRange, maxTargetsToHit);         
        } else {
            DrawBeamBranch(transform.position, 0, 1);
        }
    }

    IEnumerator UnRender(float time) {
        yield return new WaitForSeconds(time);
        renderLine = !renderLine;
        StartCoroutine(DamageEnemy(inRange, projectileDamage));
        StartCoroutine(UnRender(time));
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
    public override GameObject[] ScanRadius(Vector2 origin, float scanRadius, float returnCount) {
        return base.ScanRadius(origin, scanRadius, returnCount);
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
