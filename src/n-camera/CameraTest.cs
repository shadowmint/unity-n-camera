using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraTest : MonoBehaviour
{
  // Edit in editor
  public Vector3 rotation;
  public Vector3 usedRotation;

  // Settings
  public GameObject worldNormal;

  // State
  CameraState state;

  public void Start()
  {
    state = new CameraState(gameObject, worldNormal.transform.up);
    rotation = state.lastKnownRotation;
  }

  public void Update()
  {
    state.SetRotation(rotation);
    gameObject.transform.rotation = state.rotation;
    usedRotation = state.rotation.eulerAngles;
  }
}

public class CameraState
{
  /// Global up axis
  public Vector3 up;

  /// Current rotation state
  public Quaternion rotation;

  /// Last rotation value
  public Vector3 lastKnownRotation;

  public CameraState(GameObject initialState, Vector3 up)
  {
    this.up = up;
    rotation = initialState.transform.rotation;
    lastKnownRotation = rotation.eulerAngles;
  }

  /// Set the absolute rotation on this target
  public void SetRotation(Vector3 value)
  {
    var delta = lastKnownRotation - value;
    if (delta.magnitude > 0f)
    {
      lastKnownRotation = value;
      Rotate(delta);
    }
  }

  /// Rotate somewhat in some direction with Euler angles
  public void Rotate(Vector3 change)
  {
    var up = Quaternion.AngleAxis(change.x, Right);
    var left = Quaternion.AngleAxis(change.y, this.up);
    var tilt = Quaternion.AngleAxis(change.z, Forward);
    rotation = left*up*tilt*rotation;
  }

  /// Housekeeping
  private float x
  {
    get { return rotation.x; }
  }

  private float y
  {
    get { return rotation.y; }
  }

  private float z
  {
    get { return rotation.z; }
  }

  private float w
  {
    get { return rotation.w; }
  }

  private Vector3 Forward
  {
    get { return new Vector3(2*(x*z + w*y), 2*(y*x - w*x), 1 - 2*(x*x + y*y)); }
  }

  private Vector3 Up
  {
    get { return new Vector3(2*(x*y - w*z), 1 - 2*(x*x + z*z), 2*(y*z + w*x)); }
  }

  private Vector3 Right
  {
    get { return new Vector3(1 - 2*(y*y + z*z), 2*(x*y + w*z), 2*(x*z - w*y)); }
  }
}