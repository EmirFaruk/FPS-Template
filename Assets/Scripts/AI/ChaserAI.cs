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

        private enum KeyName
        {
            AttackDistance,
            CanAttack,
            Player,
            Distance
        }
        #endregion

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            CheckDistance();
        }

        private void Initialize()
        {
            behaviourRunner = GetComponent<BehaviourRunner>();
            blackboard = behaviourRunner.GetBlackboard();
            player = GameObject.FindGameObjectWithTag("Player").transform;

            InitializeKeys();
        }

        private void InitializeKeys()
        {
            blackboard.SetFoundKey(KeyName.AttackDistance.ToString(), new FloatKey(), attackCooldown);
            canAttackKey = (BoolKey)blackboard.SetFoundKey(KeyName.CanAttack.ToString(), new BoolKey(), canAttack);
            playerTransformKey = (TransformKey)blackboard.SetFoundKey(KeyName.Player.ToString(), new TransformKey(), player);

            if (blackboard.TryFindKey(KeyName.Distance.ToString(), out FloatKey distanceToPlayerKey))
            {
                this.distanceToPlayerKey = distanceToPlayerKey;
            }
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
