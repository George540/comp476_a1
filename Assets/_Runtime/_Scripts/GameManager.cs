using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

namespace _Runtime._Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private List<AIAgent> _players = new List<AIAgent>();
        public List<Material> _cMaterials = new List<Material>();

        public AIAgent _pursuer;
        public float _tagDistance = 1f;
        
        public float maxOffset = 40f;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            foreach (var player in _players)
            {
                player.gameObject.AddComponent<Wander>();
                player.gameObject.AddComponent<LookWhereYouAreGoing>();
                player.transform.rotation = Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f);
            }
            SetPursuer(_players[Random.Range(0, _players.Count)]);
        }

        // Update is called once per frame
        void Update()
        {
            CheckIfTagged();
        }

        private void SetPursuer(AIAgent agent)
        {
            _pursuer = agent;
            Destroy(_pursuer.gameObject.GetComponent<Wander>());
            Destroy(_pursuer.gameObject.GetComponent<LookWhereYouAreGoing>());
            _pursuer.maxSpeed *= 1.5f;
            _pursuer.SetMaterial(_cMaterials[0]);
            _pursuer._ePlayerState = PlayerState.EPlayerState.Tagged;
            _pursuer.TrackTarget(FindNearestTargetToFreeze().transform);
            _pursuer.gameObject.AddComponent<LookWhereYouAreGoing>();
            _pursuer.gameObject.AddComponent<Seek>();
        }

        private AIAgent FindNearestTargetToFreeze()
        {
            AIAgent closestPlayer = null;
            float closestDistanceSqr = Mathf.Infinity;
            foreach (var player in _players)
            {
                // skip if it compares itself or someone who is already frozen
                if (player._ePlayerState == PlayerState.EPlayerState.Tagged || player._ePlayerState == PlayerState.EPlayerState.Frozen) continue;

                var distance = player.transform.position - _pursuer.transform.position;
                float dSqrToTarget = distance.sqrMagnitude;
                // check if current comparison is smaller than the current closest player to target
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    closestPlayer = player;
                }
            }

            if (closestPlayer != null)
            {
                if (closestPlayer.GetComponent<Wander>() != null)
                {
                    Destroy(closestPlayer.GetComponent<Wander>());
                }
                if (closestPlayer.GetComponent<LookWhereYouAreGoing>() != null)
                {
                    Destroy(closestPlayer.GetComponent<LookWhereYouAreGoing>());
                }
                closestPlayer.TrackTarget(_pursuer.transform);
                closestPlayer.behaviorType = AIAgent.EBehaviorType.Steering;
                closestPlayer._ePlayerState = PlayerState.EPlayerState.Targeted;
                closestPlayer.gameObject.AddComponent<Flee>();
                closestPlayer.gameObject.AddComponent<FaceAway>();
            }
            return closestPlayer;
        }
        
        // Find closest unfrozen player who can unfreeze the frozen agent
        private AIAgent FindNearestTargetToUnfreeze(AIAgent frozenGuy)
        {
            AIAgent closestPlayer = null;
            float closestDistanceSqr = Mathf.Infinity;
            foreach (var player in _players)
            {
                if (player._ePlayerState == PlayerState.EPlayerState.Unfrozen)
                {
                    var distance = player.transform.position - frozenGuy.transform.position;
                    float dSqrToTarget = distance.sqrMagnitude;
                    // check if current comparison is smaller than the current closest player to target
                    if (dSqrToTarget < closestDistanceSqr)
                    {
                        closestDistanceSqr = dSqrToTarget;
                        closestPlayer = player;
                    }
                }
            }

            if (closestPlayer != null)
            {
                if (closestPlayer.GetComponent<Wander>() != null)
                {
                    Destroy(closestPlayer.GetComponent<Wander>());
                }
                if (closestPlayer.GetComponent<LookWhereYouAreGoing>() != null)
                {
                    Destroy(closestPlayer.GetComponent<LookWhereYouAreGoing>());
                }
                closestPlayer.TrackTarget(frozenGuy.transform);
                closestPlayer.behaviorType = AIAgent.EBehaviorType.Steering;
                closestPlayer._ePlayerState = PlayerState.EPlayerState.Unfrozen;
                closestPlayer.gameObject.AddComponent<LookWhereYouAreGoing>();
                closestPlayer.gameObject.AddComponent<Arrive>();
            }

            return closestPlayer;
        }
        
        void CheckIfTagged()
        {
            if (_pursuer == null) return;
            if (Vector3.Distance(_pursuer.TargetPosition, _pursuer.transform.position) <= _tagDistance)
            {
                Destroy(_pursuer.trackedTarget.gameObject.GetComponent<Flee>());
                Destroy(_pursuer.trackedTarget.gameObject.GetComponent<FaceAway>());
                var target = _pursuer.trackedTarget.gameObject.GetComponent<AIAgent>();
                target.behaviorType = AIAgent.EBehaviorType.Kinematic;
                target.gameObject.AddComponent<Stop>();
                target.SetMaterial(_cMaterials[2]);
                target._ePlayerState = PlayerState.EPlayerState.Frozen;
                target.UnTrackTarget();

                _pursuer.UnTrackTarget();
                _pursuer.trackedTarget = FindNearestTargetToFreeze().transform;
                
                target.TrackTarget(FindNearestTargetToUnfreeze(target).transform);
            }
        }
    }
}
