using RenownedGames.AITree;
using System.Threading.Tasks;
using UnityEngine;

namespace RunnnerGame.Enemy
{
    [RequireComponent(typeof(BehaviourRunner))]
    public class ChaserAI : MonoBehaviour
    {
        private BehaviourRunner behaviourRunner;
        private Blackboard blackboard;

        [Header("Attack")]
        [SerializeField] private int damage = 10;
        [SerializeField] private float attackCooldown = 2f;
        [SerializeField] private float attackDistance = 4f;
        private BoolKey canAttackKey;
        private bool canAttack = true;

        private Transform player;
        private TransformKey playerTransformKey;
        private FloatKey distanceToPlayerKey;

        private void Awake()
        {
            behaviourRunner = GetComponent<BehaviourRunner>();
            blackboard = behaviourRunner.GetBlackboard();
            player = GameObject.FindGameObjectWithTag("Player").transform;

            blackboard.SetFoundKey("AttackDistance", new FloatKey(), attackCooldown);
            canAttackKey = (BoolKey)blackboard.SetFoundKey("CanAttack", new BoolKey(), canAttack);

            if (blackboard.TryFindKey("Player", out TransformKey playerTransformKey))
            {
                this.playerTransformKey = playerTransformKey;
            }

            if (blackboard.TryFindKey("Distance", out FloatKey distanceToPlayerKey))
            {
                this.distanceToPlayerKey = distanceToPlayerKey;
            }
        }

        private void Update()
        {
            CheckDistance();
        }

        void CheckDistance()
        {
            playerTransformKey.SetValue(player);
            distanceToPlayerKey.SetValue(Vector3.Distance(transform.position, player.position));
        }

        public async void OnAttack()
        {
            if (canAttack && !destroyCancellationToken.IsCancellationRequested)
            {
                canAttackKey.SetValue(canAttack = false);
                Debug.Log("Attack : " + damage);
                PlayerHealth.OnTakeDamage?.Invoke(-damage);

                await Task.Delay(((int)(attackCooldown * 1000)));

                canAttackKey.SetValue(canAttack = true);
            }
        }
    }
}

