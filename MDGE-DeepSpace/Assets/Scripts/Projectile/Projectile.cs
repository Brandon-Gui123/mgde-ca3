using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{

    public IEnumerator DamageEnemy(GameObject[] enemiesArr, float projectileDamage)
    {
        foreach (GameObject enemy in enemiesArr)
        {
            enemy.GetComponent<EnemyController>().health -= projectileDamage;
        }
        
        yield return null;
    }

    public void AnimateTheProjectile(Animator animator, string animationName)
    {
        animator.Play(animationName);
    }

    //returns nearest few (returnCount) objects
    public virtual GameObject[] ScanRadius(Vector2 origin, float scanRadius, float returnCount)
    {
        Collider2D[] scannedColliders = Physics2D.OverlapCircleAll(origin, scanRadius, Physics2D.AllLayers);//change the layermask

        GameObject[] scannedObjects = new GameObject[scannedColliders.Length];
        for (int i = 0; i < scannedColliders.Length; i++)
        {
            scannedObjects[i] = scannedColliders[i].gameObject;
        }

        float[] distancesFromOrigin = new float[scannedColliders.Length];
        for (int i = 0; i < scannedColliders.Length; i++)
        {
            distancesFromOrigin[i] = Vector2.Distance(scannedColliders[i].transform.position, origin);
        }

        List<int> ignoredIndex = new List<int>();

        for (int i = 0; i < returnCount; i++)
        {
            List<int> tempMask = ignoredIndex;
            ignoredIndex.Add(FindMinIndex(distancesFromOrigin, tempMask));
        }
        List<GameObject> toBeReturned = new List<GameObject>();
        if (ignoredIndex[0] > -1)
        {
            for (int i = 0; i < ignoredIndex.Count && i < distancesFromOrigin.Length; i++)
            {
                toBeReturned.Add(scannedObjects[ignoredIndex[i]]);
            }
        }
        return toBeReturned.ToArray();
    }

    //finds the lowest value (closest distance) from a floatArray, not considering the elements with indexes in the 'iterator mask'
    int FindMinIndex(float[] listOfFloats, List<int> iteratorMask)
    {
        float nextLowest = Mathf.Max(listOfFloats);
        int index = -1;
        for (int i = 0; i < listOfFloats.Length; i++)
        {
            //check ignore indexes
            bool skip = false;
            for (int j = 0; j < iteratorMask.Count; j++)
            {
                if (i == iteratorMask[j])
                {
                    skip = true;
                    break;
                }
            }
            if (!skip)
            {
                if (listOfFloats[i] <= nextLowest)
                {
                    nextLowest = listOfFloats[i];
                    index = i;
                }
            }
        }
        return index;
    }
}