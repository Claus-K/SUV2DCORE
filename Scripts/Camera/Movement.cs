using UnityEngine;

namespace Camera
{
    public class Movement : MonoBehaviour
    {
        public float baseSpeed = 2.0f;
        private float maxSpeed = 10.0f;
        private float acceleration = 2.5f;
        public float borderThickness = 10f;

        private float _currentSpeed;

        void Start()
        {
            _currentSpeed = baseSpeed;
        }

        // Update is called once per frame
        void Update()
        {
            var movement = Vector3.zero;
            bool isMoving = false;

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (horizontal != 0 || vertical != 0)
            {
                isMoving = true;
            }
            movement.x += horizontal;
            movement.y += vertical;
            
            if (IsMouseWithinScreenBounds())
            {
                if (Input.mousePosition.y >= Screen.height - borderThickness)
                {
                    movement.y += 1;
                    isMoving = true;
                }
                else if (Input.mousePosition.y <= borderThickness)
                {
                    movement.y -= 1;
                    isMoving = true;
                }

                if (Input.mousePosition.x <= borderThickness)
                {
                    movement.x -= 1;
                    isMoving = true;
                }
                else if (Input.mousePosition.x >= Screen.width - borderThickness)
                {
                    movement.x += 1;
                    isMoving = true;
                }
            }

            // Normalize the movement vector (in case of diagonal movement)
            movement.Normalize();

            _currentSpeed = isMoving
                ? Mathf.Clamp(_currentSpeed + acceleration * Time.deltaTime, baseSpeed, maxSpeed)
                : Mathf.Clamp(_currentSpeed - acceleration * Time.deltaTime, baseSpeed, maxSpeed);

            transform.position += movement * (_currentSpeed * Time.deltaTime);
        }

        private bool IsMouseWithinScreenBounds()
        {
            return Input.mousePosition.x >= 0 &&
                   Input.mousePosition.x <= Screen.width &&
                   Input.mousePosition.y >= 0 &&
                   Input.mousePosition.y <= Screen.height;
        }
    }
}