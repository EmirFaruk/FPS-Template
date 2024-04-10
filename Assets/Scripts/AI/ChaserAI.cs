using RenownedGames.AITree;
using System.Threading.Tasks;
using UnityEngine;

namespace RunnnerGame.Enemy
{
    [RequireComponent(typeof(BehaviourRunner))]
    public class ChaserAI : MonoBehaviour
    {
        #region VARIABLES
        private BehaviourRunner behaviourRunner;
        private Blackboard blackboard;

        [Header("Attack")]
        [SerializeField] private int damage = 10;
        [SerializeField] private float attackCooldown = 2f;
        [SerializeField] private float attackDistance = 4f;
        private BoolKey canAttackKey;
        private bool canAttack = false;
        private bool onCooldown = false;

        private Transform player;
        private TransformKey playerTransformKey;
        private FloatKey distanceToPlayerKey;
        #endregion

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
            if (!onCooldown) canAttackKey.SetValue(canAttack = distanceToPlayerKey.GetValue() <= attackDistance);
        }

        public async void OnAttack()
        {
            if (canAttack && !destroyCancellationToken.IsCancellationRequested)
            {
                PlayerHealth.OnTakeDamage?.Invoke(-damage);
                Debug.Log("Attack : " + damage);

                canAttackKey.SetValue(canAttack = false);
                onCooldown = true;

                await Task.Delay(((int)(attackCooldown * 1000)));

                onCooldown = false;
                canAttackKey.SetValue(canAttack = true);
            }
        }
    }
}

