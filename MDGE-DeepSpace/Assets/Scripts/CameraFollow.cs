using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField]
    private Camera mainCamera;

    private Vector2 viewportSize;

    // Awake is called when the script instance is being loaded
    private void Awake() {
        if (!mainCamera) {
            mainCamera = Camera.main;
        }        
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        
    }
}
