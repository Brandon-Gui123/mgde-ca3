using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    /// <summary>
    /// The main camera in the scene.
    /// We use a field for the main camera because doing Camera.main, 
    /// which is just GameObject.FindGameObjectWithTag("MainCamera"),
    /// hurts performance, so we should drag-and-drop the camera or
    /// get the result from Camera.main and cache it in here.
    /// </summary>
    [SerializeField]
    private Camera mainCamera;

    /// <summary>
    /// The orthographic size of the Camera component before it was changed.
    /// Used to keep track of changes in <see cref="Camera.orthographicSize"/>.
    /// </summary>
    private float previousCamSize;

    /// <summary>
    /// The size of the camera's viewport, in world space.
    /// </summary>
    private Vector2 viewportSize;

    /// <summary>
    /// The <see cref="Transform"/> component of the target that this camera
    /// will be following.
    /// </summary>
    public Transform target;

    /// <summary>
    /// The <see cref="Transform"/> component of the map.
    /// We need this for map size and position and because
    /// <see cref="Transform"/> is a reference, changes to the <see cref="Transform"/>
    /// component are immediately reflected in here.
    /// </summary>
    public Transform mapTransform;

    /// <summary>
    /// The actual size of the map, accounting for pixels per unit, sprite size and scaling.
    /// </summary>
    private Vector2 mapSize;

    /// <summary>
    /// The sprite that the map uses.
    /// </summary>
    private Sprite mapSprite;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        //placed on awake to ensure that we are able to get our camera
        if (!mainCamera)
        {
            mainCamera = Camera.main;
        }

        previousCamSize = mainCamera.orthographicSize;
    }

    // Start is called before the first frame update
    void Start()
    {
        //calculate the size of the viewport in world units
        viewportSize = CalculateViewportSize(mainCamera);

        if (!mapSprite)
        {
            mapSprite = mapTransform.GetComponent<SpriteRenderer>().sprite;
        }

        //calculate the actual map size
        mapSize = new Vector2(
                mapSprite.rect.width / mapSprite.pixelsPerUnit * mapTransform.lossyScale.x,
                mapSprite.rect.height / mapSprite.pixelsPerUnit * mapTransform.lossyScale.y
            );
    }

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        //recalculate viewport size if the camera's size changes
        if (DidCamPixelRectChange())
        {
            viewportSize = CalculateViewportSize(mainCamera);
        }
    }


    // LateUpdate is called every frame, if the Behaviour is enabled
    private void LateUpdate()
    {
        //what is our camera's destination?
        //store this in a vector because we want to manipulate it
        Vector3 destination = new Vector3(target.position.x, target.position.y, transform.position.z);

        //clamp the values of the destination vector so that it doesn't go out of the map
        destination = ClampToMapBoundaries(destination, mapTransform.position, mapSize, viewportSize);

        //apply destination values to the position of the camera
        transform.position = destination;
    }

    /// <summary>
    /// Clamps the values of a given vector and ensure that its values stay within the boundaries of the map.
    /// The minimum and maximum of both the x-coordinate and y-coordinate of the vector is the left and right edges and
    /// top and bottom edges respectively.
    /// However, since we need to calculate for point position, we need to subtract or add some amount accordingly such that
    /// we reflect the correct position of the point, and hence, clamp the position vector of the point.
    /// </summary>
    /// <param name="destination">The target destination for a given <see cref="Camera"/>.</param>
    /// <param name="mapPosition">The current position of the map.</param>
    /// <param name="mapLossyScale">The globa scale of the map.</param>
    /// <param name="viewportSize">The size of the <see cref="Camera"/> viewport, in world space.</param>
    /// <returns>A <see cref="Vector3"/> that has its values clamped.</returns>
    private Vector3 ClampToMapBoundaries(Vector3 destination, Vector3 mapPosition, Vector3 mapSize, Vector3 viewportSize)
    {
        destination.x = Mathf.Clamp(destination.x, mapPosition.x - mapSize.x / 2 + viewportSize.x / 2, mapPosition.x + mapSize.x / 2 - viewportSize.x / 2);
        destination.y = Mathf.Clamp(destination.y, mapPosition.y - mapSize.y / 2 + viewportSize.y / 2, mapPosition.y + mapSize.y / 2 - viewportSize.y / 2);
        return destination;
    }

    // This function is called when the script is loaded or a value is changed in the inspector (Called in the editor only)
    private void OnValidate()
    {
        viewportSize = mainCamera.ScreenToWorldPoint(new Vector2(mainCamera.scaledPixelWidth, mainCamera.scaledPixelHeight)) * 2;
    }

    // Implement this OnDrawGizmosSelected if you want to draw gizmos only if the object is selected
    private void OnDrawGizmos()
    {
        //draw the viewport of the camera as a Gizmo
        //also helps to check if our viewport size calculation is correct
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(mainCamera.transform.position, viewportSize);
    }

    /// <summary>
    /// A method that checks to see if the main camera's orthographic size has changed and returns a boolean
    /// stating if it did change.
    /// </summary>
    /// <returns>A <see cref="bool"/> which is <see langword="true"/> when the size has changed and <see langword="false"/> otherwise.</returns>
    private bool DidCamPixelRectChange()
    {
        if (mainCamera.orthographicSize != previousCamSize)
        {
            previousCamSize = mainCamera.orthographicSize;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Calculates the size of the viewport of a given <see cref="Camera"/> component in world units.
    /// </summary>
    /// <param name="camera">The <see cref="Camera"/> component whose viewport size is what you want to calculate.</param>
    /// <returns>A <see cref="Vector2"/> representing the size of the viewport in world units.</returns>
    private Vector2 CalculateViewportSize(Camera camera)
    {
        //double the world vector because of how rectangles are handled
        return camera.ScreenToWorldPoint(new Vector2(camera.scaledPixelWidth, camera.scaledPixelHeight)) * 2;
    }

}
