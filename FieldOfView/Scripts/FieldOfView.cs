using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace FieldOfView.Scripts
{
    public class FieldOfView : MonoBehaviour
    {
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float fov;
        [SerializeField] private float viewDistance;

        private Mesh mesh;
        private Vector3 origin;
        private float startingAngle;
        private const int rayCount = 50;

        public Vector3 Origin
        {
            get => origin;
            set => origin = value;
        }

        public float StartingAngle
        {
            get => startingAngle;
            set => startingAngle = value;
        }

        private void Awake()
        {
            var x = GetComponent<MeshRenderer>();
            x.sortingLayerName = "Agents Body";
            x.sortingOrder = 9;
            var meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material = new Material(Shader.Find("Sprites/Default"))
            {
                color = new Color(1, 0, 1, 0.5f)
            };

        }

        private void Start()
        {
            mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;
            Origin = transform.position;  // Initialize origin to transform position
            StartingAngle = 45f;
        }

        private void LateUpdate()
        {
            Origin = transform.position; // Ensure origin is updated to the current transform position
            var angle = startingAngle;
            var angleIncrease = fov / rayCount;

            var vertices = new Vector3[rayCount + 2];
            var uv = new Vector2[vertices.Length];
            var triangles = new int[rayCount * 3];

            vertices[0] = Vector3.zero;  // Origin in local space

            var vertexIndex = 1;
            var triangleIndex = 0;
            for (var i = 0; i <= rayCount; i++)
            {
                Vector3 vertex;
                var rayDirection = GetVectorFromAngle(angle);
                var raycastHit2D = Physics2D.Raycast(Origin, rayDirection, viewDistance, layerMask);

                // Visualize the ray with Debug.DrawRay
                Debug.DrawRay(Origin, rayDirection * viewDistance, Color.red);

                if (raycastHit2D.collider == null)
                {
                    vertex = transform.InverseTransformPoint(Origin + rayDirection * viewDistance);
                }
                else
                {
                    vertex = transform.InverseTransformPoint(raycastHit2D.point);
                }

                vertices[vertexIndex] = vertex;
                if (i > 0)
                {
                    triangles[triangleIndex + 0] = 0;
                    triangles[triangleIndex + 1] = vertexIndex - 1;
                    triangles[triangleIndex + 2] = vertexIndex;

                    triangleIndex += 3;
                }

                vertexIndex += 1;
                angle -= angleIncrease;
            }

            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
            mesh.bounds = new Bounds(Vector3.zero, Vector3.one * 1000f);  // Set bounds relative to local origin

        }

        

        private Vector3 GetVectorFromAngle(float angle)
        {
            var angleRad = angle * (Mathf.PI / 180f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        private float GetAngleFromVectorFloat(Vector3 dir)
        {
            dir = dir.normalized;
            var n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            return n;
        }

        public void SetDirection(Vector3 direction)
        {
            StartingAngle = GetAngleFromVectorFloat(direction) - fov / 2f;
        }
        
        public void SetDirection(float parentAngleZ)
        {
            StartingAngle = parentAngleZ + 90 - fov / 2f;
        }
    }
}
