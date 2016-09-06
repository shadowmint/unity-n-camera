using UnityEngine;
using N;
using N.Package.Core;

namespace N.Package.Cameras
{
  /// Basic follow camera moves with object, but ignores target rotations
  [AddComponentMenu("N/Camera/FollowCamera")]
  public class FollowCamera : MonoBehaviour
  {
    [Tooltip("The GameObject instance on the scene to match positions with (ie. Don't pick a prefab from the Assets list)")] public GameObject target;

    /// The original position of the camera
    private Vector3 cameraOrigin;

    /// The original position of the target
    private Vector3 targetOrigin;

    public void Start()
    {
      if (target == null) return;
      cameraOrigin = this.Position();
      targetOrigin = target.Position();
    }

    public void Update()
    {
      if (target == null) return;
      var pos = target.transform.position;

      if (pos == targetOrigin) return;
      var newPos = cameraOrigin + (pos - targetOrigin);
      gameObject.Move(newPos);
    }
  }
}