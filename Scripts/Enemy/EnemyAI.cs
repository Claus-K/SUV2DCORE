using UnityEngine;

namespace Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        // private EnemyPathFinder pathFinder;
        private Vector3 startingPosition;
        private Vector3 roamPosition;

        // Start is called before the first frame update
        void Start()
        {
            startingPosition = transform.position;
            roamPosition = GetRoamingPosition();
        }

        // Update is called once per frame
        void Update()
        {
        }

        private Vector3 GetRoamingPosition()
        {
            return startingPosition + GetRandomDirection() * Random.Range(10f, 50f);
        }

        public static Vector3 GetRandomDirection()
        {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
    }
}