using UnityEngine;
using UnityEngine.Networking;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 10f;
        public float horizontalDragFactor = 1f;
        public float jumpForce = 1f;
        public float gravityMultiplier = 1f;
        public GameObject CameraObj;
        public GameObject CameraMountObj;
        public GameObject MeshObj;
        public Rigidbody rigidBody;
        public bool fly = false;

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
                if (fly)
                {
                    NoClipCameraPosition();
                }
                else
                {
                    RigidBodyCameraPosition();
                }
            }
        }

        void Update()
        {
            if (CameraObj.activeSelf)
            {
                CameraRotation();
            }
        }

        void NoClipCameraPosition()
        {
            pos = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
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
            }
            if (Input.GetKey(KeyCode.LeftShift)) pos.y = -speed;
            if (Input.GetKey(KeyCode.Space)) pos.y = speed;

            transform.Translate(transform.InverseTransformVector(pos * Time.fixedDeltaTime));
        }

        void CameraRotation()
        {
            rot = new Vector2(rot.x + Input.GetAxis("Mouse X") * 3, rot.y + Input.GetAxis("Mouse Y") * 3);

            if (rot.y < -90)
            {
                rot.y = -90;
            }
            if (rot.y > 90)
            {
                rot.y = 90;
            }

            MeshObj.transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up);
            CameraMountObj.transform.localRotation = Quaternion.AngleAxis(rot.x, Vector3.up) * Quaternion.AngleAxis(rot.y, Vector3.left);

            // transform.position += transform.forward * 3 * Input.GetAxis("Vertical");
            // transform.position += transform.right * 3 * Input.GetAxis("Horizontal");
        }

        void RigidBodyCameraPosition()
        {
            pos = Vector3.zero;

            float inputh = Input.GetAxis("Horizontal");
            float inputv = Input.GetAxis("Vertical");
            if (inputv != 0)
            {
                pos.x += Mathf.Sin(Mathf.Deg2Rad * MeshObj.transform.rotation.eulerAngles.y) * speed * inputv;
                pos.z += Mathf.Cos(Mathf.Deg2Rad * MeshObj.transform.rotation.eulerAngles.y) * speed * inputv;
            }
            if (inputh != 0)
            {
                pos.x += -Mathf.Cos(Mathf.Deg2Rad * MeshObj.transform.rotation.eulerAngles.y) * speed * -inputh;
                pos.z += Mathf.Sin(Mathf.Deg2Rad * MeshObj.transform.rotation.eulerAngles.y) * speed * -inputh;
            }

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
            if (Input.GetKey(KeyCode.Space))
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
