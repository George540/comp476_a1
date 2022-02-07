using _Runtime._Scripts;
using UnityEngine;

namespace AI
{
    public class AIAgent : MonoBehaviour
    {
        public float maxSpeed;
        public float maxDegreesDelta;
        public bool lockY = true;
        public bool debug;

        public enum EBehaviorType { Kinematic, Steering }
        public EBehaviorType behaviorType;
        public PlayerState.EPlayerState _ePlayerState;

        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
        private Animator animator;

        public Transform trackedTarget;
        public Vector3 targetPosition;
        public Vector3 TargetPosition
        {
            get => trackedTarget != null ? trackedTarget.position : targetPosition;
        }
        public Vector3 TargetForward
        {
            get => trackedTarget != null ? trackedTarget.forward : Vector3.forward;
        }
        public Vector3 TargetVelocity
        {
            get
            {
                Vector3 v = Vector3.zero;
                if (trackedTarget != null)
                {
                    AIAgent targetAgent = trackedTarget.GetComponent<AIAgent>();
                    if (targetAgent != null)
                        v = targetAgent.Velocity;
                }

                return v;
            }
        }

        public Vector3 Velocity { get; set; }

        public void TrackTarget(Transform targetTransform)
        {
            trackedTarget = targetTransform;
        }

        public void UnTrackTarget()
        {
            trackedTarget = null;
        }

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            //CheckPosition();
            
            if (debug)
                Debug.DrawRay(transform.position, Velocity, Color.red);

            if (behaviorType == EBehaviorType.Kinematic)
            {
                // TODO: average all kinematic behaviors attached to this object to obtain the final kinematic output and then apply it
				GetKinematicAvg(out Vector3 kinematicAvg, out Quaternion rotation);

                Velocity = kinematicAvg.normalized * maxSpeed;

                transform.position += Velocity * Time.deltaTime;

                rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
                transform.rotation = rotation;
            }
            else
            {
                // TODO: combine all steering behaviors attached to this object to obtain the final steering output and then apply it
				GetSteeringSum(out Vector3 steeringASum, out Quaternion rotation);

                Vector3 acceleration = steeringASum / 1;
                Velocity += acceleration * Time.deltaTime;
                Velocity = Vector3.ClampMagnitude(Velocity, maxSpeed);

                transform.position += Velocity * Time.deltaTime;

                rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, transform.rotation * rotation, maxDegreesDelta * Time.deltaTime);
            }

            animator.SetBool("walking", Velocity.magnitude > 0);
            animator.SetBool("running", Velocity.magnitude > maxSpeed/2);
        }

        private void GetKinematicAvg(out Vector3 kinematicAvg, out Quaternion rotation)
        {
            kinematicAvg = Vector3.zero;
            Vector3 eulerAvg = Vector3.zero;
            AIMovement[] movements = GetComponents<AIMovement>();
            int count = 0;
            foreach (AIMovement movement in movements)
            {
                kinematicAvg += movement.GetKinematic(this).linear;
                eulerAvg += movement.GetKinematic(this).angular.eulerAngles;

                ++count;
            }

            if (count > 0)
            {
                kinematicAvg /= count;
                eulerAvg /= count;
                rotation = Quaternion.Euler(eulerAvg);
            }
            else
            {
                kinematicAvg = Velocity;
                rotation = transform.rotation;
            }
        }

        private void GetSteeringSum(out Vector3 steeringForceSum, out Quaternion rotation)
        {
            steeringForceSum = Vector3.zero;
            rotation = Quaternion.identity;
            AIMovement[] movements = GetComponents<AIMovement>();
            foreach (AIMovement movement in movements)
            {
                steeringForceSum += movement.GetSteering(this).linear;
                rotation *= movement.GetSteering(this).angular;
            }
        }

        public void SetMaterial(Material mat)
        {
            _skinnedMeshRenderer.material = mat;
        }

        // Wraps agent's position within a certain perimeter.
        // If agent exceeds X-Z limits, it wraps on the other side
        void CheckPosition()
        {
            var offset = GameManager.Instance.maxOffset;
            if (transform.position.x < -offset)
            {
                transform.position = new Vector3(offset - 1, transform.position.y, transform.position.z);
            }
            else if (transform.position.x > offset)
            {
                transform.position = new Vector3(-offset + 1, transform.position.y, transform.position.z);
            }
            
            if (transform.position.z < -offset)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, offset - 1);
            }
            else if (transform.position.z > offset)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -offset + 1);
            }
        }
    }
}