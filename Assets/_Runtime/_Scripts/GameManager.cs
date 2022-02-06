using System.Collections.Generic;
using AI;
using UnityEngine;

namespace _Runtime._Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        [SerializeField] private List<AIAgent> _players = new List<AIAgent>();
        [SerializeField] private List<Material> _cMaterials = new List<Material>();

        public AIAgent _pursuer;

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
            SetPursuer(_players[Random.Range(0, _players.Count)]);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void SetPursuer(AIAgent agent)
        {
            if (_pursuer != null)
            {
                _pursuer._ePlayerState = PlayerState.EPlayerState.Frozen;
                _pursuer.trackedTarget = null;
                _pursuer.SetMaterial(_cMaterials[1]);
            }
            _pursuer = agent;
            _pursuer.SetMaterial(_cMaterials[0]);
            _pursuer._ePlayerState = PlayerState.EPlayerState.Tagged;
            _pursuer.trackedTarget = FindNearestTarget().transform;
            _pursuer.gameObject.AddComponent<LookWhereYouAreGoing>();
            _pursuer.gameObject.AddComponent<Seek>();
        }

        private AIAgent FindNearestTarget()
        {
            var closestPlayer = _players[0];
            foreach (var player in _players)
            {
                if (player == _pursuer) continue;
                var distance = player.transform.position - _pursuer.transform.position;
                if (distance.magnitude < (closestPlayer.transform.position - _pursuer.transform.position).magnitude)
                {
                    closestPlayer = player;
                }
            }
            return closestPlayer;
        }
    
    }
}
