using UnityEngine;
using N;
using N.Package.Core;

namespace N.Package.Cameras
{
  /// Trail after a target, always facing it
  [AddComponentMenu("N/Camera/TrailCamera")]
  public class TrailCamera : MonoBehaviour
  {
    [Tooltip("The GameObject instance on the scene to match positions with (ie. Don't pick a prefab from the Assets list)")]
    public GameObject target;

    [Tooltip("Apply rotation changes only on these axes")]
    public Vector3 follow;

    [Tooltip("Apply this position offset for in-game zoom, etc.")]
    public Vector3 positionOffset;

    [Tooltip("Apply this rotation offset for in-game look around, etc.")]
    public Vector3 lookAtOffset;

    [Tooltip("The speed at which to close towards the target location. 0 for instant~")]
    public float followSpeed = 0f;

    [Tooltip("The amount of time to 'chill' if we hit a target while moving")]
    public float chillTime = 1f;

    /// The original delta to the camera
    private Vector3 delta;

    /// Initial target orientation
    private Vector3 rotation;

    /// Chillout zone
    private float cooldown = 0f;

    public void Start()
    {
      if (target != null)
      {
        delta = this.Position() - target.Position();
        rotation = target.Rotation();
        Follow();
      }
    }

    public void Update()
    {
      Follow();
    }

    /// Follow the target
    private void Follow()
    {
      if (target != null)
      {
        var pos = target.Position();
        var rot = target.Rotation();
        var change = rot - rotation;
        var filtered = new Vector3(change[0]*follow[0], change[1]*follow[1], change[2]*follow[2]);
        var new_pos = pos + Quaternion.Euler(filtered)*delta;
        var target_pos = new_pos + positionOffset;
        gameObject.transform.LookAt(target.Position() + lookAtOffset);
        if (cooldown <= 0f)
        {
          Step(target_pos);
        }
        else
        {
          cooldown -= Time.deltaTime;
        }
      }
    }

    /// Step towards target
    private void Step(Vector3 target)
    {
      if (followSpeed == 0f)
      {
        this.SetPosition(target);
      }
      else
      {
        this.gameObject.MoveTowards(target, followSpeed*Time.deltaTime);
      }
    }

    /// If we collide with something, chill out for a while
    public void OnCollisionStay(Collision collision)
    {
      if (cooldown <= 0f)
      {
        cooldown = chillTime;
        _.Log("Collision, cooldown...");
      }
    }
  }
}