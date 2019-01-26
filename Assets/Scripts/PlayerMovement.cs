using UnityEngine;
using UnityEngine.Networking;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 10f;
        public float turnSpeed = 2f;
        public float horizontalDragFactor = 1f;
        public float jumpForce = 1f;
        public float gravityMultiplier = 1f;
        public GameObject CameraObj;
        public GameObject CameraMountObj;
        public GameObject MeshObj;
        public Rigidbody rigidBody;
        public bool fly = false;

        public ResourceManager resources;

        Vector3 pos;
        Vector2 rot;
        bool jumping = false;

        void Start()
        {
            MeshObj.SetActive(true);
            // Offset camera just as much as the inspector says so when we
            // first click the Game window nothing everything is fine.
            var angles = transform.rotation.eulerAngles;
            rot = new Vector2(angles.y, -angles.x);
        }

        void FixedUpdate()
        {
            if(CameraObj.activeSelf)
            {
                RigidBodyCameraPosition();
            }
        }

        void RigidBodyCameraPosition()
        {
            if (resources.Fuel <= 0)
                return;
            pos = Vector3.zero;

            float inputv = Input.GetAxis("Vertical");
            if (inputv != 0)
            {
                pos.x += Mathf.Sin(Mathf.Deg2Rad * MeshObj.transform.rotation.eulerAngles.y) * speed * inputv;
                pos.z += Mathf.Cos(Mathf.Deg2Rad * MeshObj.transform.rotation.eulerAngles.y) * speed * inputv;
            }

            float inputh = Input.GetAxis("Horizontal");
            rot = new Vector2(rot.x + inputh * turnSpeed, 0);

            MeshObj.transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
            CameraMountObj.transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);

            /*if (Input.GetKey(KeyCode.W))
            {
                pos.x += Mathf.Sin(Mathf.Deg2Rad * MeshObj.transform.rotation.eulerAngles.y) * speed;
                pos.z += Mathf.Cos(Mathf.Deg2Rad * MeshObj.transform.rotation.eulerAngles.y) * speed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                pos.x += -Mathf.Sin(Mathf.Deg2Rad * MeshObj.transform.rotation.eulerAngles.y) * speed;
                pos.z += -Mathf.Cos(Mathf.Deg2Rad * MeshObj.transform.rotation.eulerAngles.y) * speed;
            }
            if (Input.GetKey(KeyCode.A))
            {
                pos.x += -Mathf.Cos(Mathf.Deg2Rad * MeshObj.transform.rotation.eulerAngles.y) * speed;
                pos.z += Mathf.Sin(Mathf.Deg2Rad * MeshObj.transform.rotation.eulerAngles.y) * speed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                pos.x += Mathf.Cos(Mathf.Deg2Rad * MeshObj.transform.rotation.eulerAngles.y) * speed;
                pos.z += -Mathf.Sin(Mathf.Deg2Rad * MeshObj.transform.rotation.eulerAngles.y) * speed;
            }*/
            //if (Input.GetKey(KeyCode.LeftShift)) {}
            if (Input.GetKey(KeyCode.Space) || Input.GetButton("Jump"))
            {
                if (!jumping && (rigidBody.velocity.y < 0.001) && (rigidBody.velocity.y > -0.001))
                {
                    jumping = true;
                    rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
            }
            else
            {
                if (rigidBody.velocity.y > 0.001)
                {
                    gravityMultiplier = 2f;
                }
                else if (rigidBody.velocity.y < -0.001)
                {
                    gravityMultiplier = 2.5f;
                }
                else
                {
                    gravityMultiplier = 1f;
                }

                if (jumping)
                {
                    jumping = false;
                }
            }

            rigidBody.AddForce(pos * Time.fixedDeltaTime, ForceMode.Impulse);

            //float rbSpeed = rigidBody.velocity.magnitude;
            //rigidBody.velocity = rigidBody.velocity / speed * Mathf.Min(rbSpeed, maxSpeed);
        }

        /*private void OnDisable()
        {
            rigidBody.isKinematic = true;
        }

        private void OnEnable()
        {
            rigidBody.isKinematic = false;
        }*/
    }
}
