using System.Collections;
using UnityEngine;

namespace Path_Generator
{
    public class Path : MonoBehaviour
    {
        public PrefabSettings prefabSettings = new PrefabSettings();

        public float delayAtPoint = 0f;
        public bool turnAtPoint = false;
        public float turnAtPointAngle = 90;


        public bool showPath = true;
        public bool loopPath = true;
        public float pointSize = .03f;

        private float _movementSpeed = 0;
        private int _prefabsCreated = 0;
        private float _nextSpawn = 0;

        private void FixedUpdate()
        {
            if (_prefabsCreated >= prefabSettings.numberOfPrefabs || !(Time.time > _nextSpawn)) return;

            _movementSpeed = prefabSettings.movementSpeed;
            _nextSpawn = Time.time + prefabSettings.nextSpawnDelay;
            Transform clone = Instantiate(prefabSettings.prefab, transform.position, Quaternion.identity).transform;
            Vector3[] path = GetPathPoints();

            StartCoroutine(loopPath ? FollowPathLoop(path, clone) : FollowPathBounce(path, clone));
            _prefabsCreated++;
        }

        private Vector3[] GetPathPoints()
        {
            Vector3[] pathPoints = new Vector3[transform.childCount];
            for (int i = 0; i < pathPoints.Length; i++)
            {
                Vector3 point = transform.GetChild(i).position;
                pathPoints[i] = new Vector3(point.x, transform.position.y, point.z) + transform.position;
            }

            return pathPoints;
        }

        #region Coroutines

        private static IEnumerator Wait(float delay)
        {
            yield return new WaitForSeconds(delay);
        }

        private IEnumerator FollowPathBounce(Vector3[] pathPoints, Transform prefab)
        {
            prefab.position = pathPoints[0];
            int counter = 0;
            int incrementer = 1;

            while (true)
            {
                if (counter < 0 || counter == pathPoints.Length - 1) incrementer *= -1;
                counter += incrementer;
                Vector3 destination = pathPoints[(counter == -1) ? counter + 1 : counter];
                yield return StartCoroutine(Move(destination, prefab));
                yield return Wait(delayAtPoint);
            }
        }

        private IEnumerator FollowPathLoop(Vector3[] pathPoints, Transform prefab)
        {
            prefab.position = pathPoints[0];
            int counter = 0;

            while (true)
            {
                Vector3 destination = pathPoints[counter % pathPoints.Length];
                counter++;
                if (turnAtPoint) yield return StartCoroutine(FacePathAtCheckPoint(destination, prefab));
                yield return StartCoroutine(Move(destination, prefab));
                yield return Wait(delayAtPoint);
            }
        }

        private IEnumerator Move(Vector3 destination, Transform prefab)
        {
            while (prefab.position != destination)
            {
                prefab.position = Vector3.MoveTowards(prefab.position, destination, Time.deltaTime * _movementSpeed);
                yield return null;
            }
        }

        private IEnumerator FacePathAtCheckPoint(Vector3 destination, Transform prefab)
        {
            Vector3 target = (destination - prefab.position).normalized;
            float targetAngle = Mathf.Atan2(target.x, target.z) * Mathf.Rad2Deg;

            while (Mathf.Abs(Mathf.DeltaAngle(targetAngle, prefab.eulerAngles.y)) > .05f)
            {
                float angle = Mathf.MoveTowardsAngle(prefab.eulerAngles.y, targetAngle, Time.deltaTime * turnAtPointAngle);
                prefab.eulerAngles = Vector3.up * angle;
                yield return null;
            }
        }

        #endregion

        private void OnDrawGizmos()
        {
            if (!showPath) return;

            Vector3 startPosition = transform.GetChild(0).position;
            Vector3 prevPosition = startPosition;

            foreach (Transform point in transform)
            {
                Gizmos.DrawLine(prevPosition, point.position);
                Gizmos.DrawSphere(point.position, pointSize);
                prevPosition = point.position;
            }

            if (loopPath) Gizmos.DrawLine(startPosition, prevPosition);
        }
    }
}