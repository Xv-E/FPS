using UnityEngine;
using UnityEngine.EventSystems;

using Photon.Pun;

using System.Collections;

namespace Com.MyCompany.MyGame
{
    /// <summary>
    /// Player manager.
    /// Handles fire Input and Beams.
    /// </summary>
    public class PlayerManager2 : MonoBehaviourPunCallbacks,IPunObservable
    {
        #region Public Fields
        [Tooltip("The current Health of our player")]
        public float Health = 1f;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;
        public float speed = 5.0f;
        public float shootSpeed;
        public GameObject bulletPrefab;
        public Transform fpoint;
        public float jumpSpeed = 400f;
        public Rigidbody playerRigid;
        public GameObject player_camera;
        public float SensitivityX = 10f;
        public float SensitivityY = 10f;
        public Transform player_transform;

        #endregion

        #region Private Fields


        //True, when the user is firing
        [Tooltip("The Beams GameObject to control")]
        [SerializeField]
        private GameObject beams;
        bool IsFiring;
        float RotationX;
        float RotationY;

        #endregion

        #region MonoBehaviour CallBacks

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during early initialization phase.
        /// </summary>
        void Awake()
        {
            if (this.beams == null)
            {
                Debug.LogError("<Color=Red><b>Missing</b></Color> Beams Reference.", this);
            }
            else
            {
                this.beams.SetActive(false);
            }
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                LocalPlayerInstance = this.gameObject;
            }
            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(this.gameObject);
            
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity during initialization phase.
        /// </summary>
        void Start()
        {
            //UnityEngine.SceneManagement.SceneManager.sceneLoaded += (scene, loadingMode) =>
            //{
            //    this.CalledOnLevelWasLoaded(scene,buildIndex);
            //};
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;

           
            if(photonView.IsMine)
            {
                player_camera.GetComponent<Camera>().depth = 0;
                
            }
            
            
        }

        /// <summary>
        /// MonoBehaviour method called on GameObject by Unity on every frame.
        /// </summary>
        void Update()
        {
            
            if (photonView.IsMine)
            {
                ProcessInputs();
                if (Health <= 0f)
                {
                    GameManager.Instance.LeaveRoom();
                }
            }
            if (this.beams != null && this.IsFiring != this.beams.activeInHierarchy)
            {
                this.beams.SetActive(this.IsFiring);
            }


        }
        public override void OnDisable()
        {
            // Always call the base to remove callbacks
            base.OnDisable();
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        /// <summary>
        /// MonoBehaviour method called when the Collider 'other' enters the trigger.
        /// Affect Health of the Player if the collider is a beam
        /// Note: when jumping and firing at the same, you'll find that the player's own beam intersects with itself
        /// One could move the collider further away to prevent this or check if the beam belongs to the player.
        /// </summary>
        void OnTriggerStay(Collider other)
        {
            if (!photonView.IsMine)
            {
                Debug.Log("is mine");
                return;
            }
            
            // We are only interested in Beamers
            // we should be using tags but for the sake of distribution, let's simply check by name.
            if ( (other.name.Contains("Beam"))|| (other.name.Contains("Bullet")) )
            {
                if (other.GetComponent<PhotonView>().IsMine)
                {
                    Debug.Log("parent is mine");
                    return;
                }
                Health -= 0.1f * Time.deltaTime;
                Debug.Log("health reduce");
            }

            
           
        }
        /// <summary>
        /// MonoBehaviour method called once per frame for every Collider 'other' that is touching the trigger.
        /// We're going to affect health while the beams are touching the player
        /// </summary>
        /// <param name="other">Other.</param>


        #endregion

        #region PUN Callbacks
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            
            if (stream.IsWriting&&PhotonNetwork.IsConnected)
            {
                // We own this player: send the others our data
                //stream.SendNext(IsFiring);
                stream.SendNext(Health);
                Debug.Log(PhotonNetwork.NickName+"send:" + Health);
            }
            else
            {
                // Network player, receive data
                //this.IsFiring = (bool)stream.ReceiveNext();
                this.Health = (float)stream.ReceiveNext();
                Debug.Log(PhotonNetwork.NickName+"receive:" + Health);
            }
        }
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }
        }
        #endregion

        #region Custom

        /// <summary>
        /// Processes the inputs. Maintain a flag representing when the user is pressing Fire.
        /// </summary>
        void ProcessInputs()
        {
            if (Input.GetKey(KeyCode.W))
            {
                player_transform.Translate(new Vector3(0, 0, 1) * Time.deltaTime * speed);
                
            }
            else if (Input.GetKey(KeyCode.S))
            {
                player_transform.Translate(new Vector3(0, 0, -1) * Time.deltaTime * speed);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                player_transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * speed);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                player_transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * speed);
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                playerRigid.AddForce(Vector3.up * jumpSpeed);
            }
            RotationX += Input.GetAxis("Mouse X") * SensitivityX;
            RotationY += Input.GetAxis("Mouse Y") * SensitivityY;
            //RotationY = Mathf.Clamp(RotationY, -180f, 180f);
            player_transform.localEulerAngles = new Vector3(0, RotationX, 0);
            player_camera.transform.localEulerAngles = new Vector3(-RotationY, 0, 0);
            if (Input.GetButtonDown("Fire1"))
            {
                if (!IsFiring)
                {
                    IsFiring = true;
                }
                GameObject bullet = PhotonNetwork.Instantiate(this.bulletPrefab.name, fpoint.transform.position, fpoint.transform.rotation, 0);
                //GameObject bullet = Instantiate(bulletPrefab, fpoint.transform.position, fpoint.transform.rotation);
                //bullet.transform.parent = this.transform;
                bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * shootSpeed;
                
            }
            if (Input.GetButtonUp("Fire1"))
            {
                if (IsFiring)
                {
                    IsFiring = false;
                }
            }
        }
        

        void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
        {
            this.CalledOnLevelWasLoaded(scene.buildIndex);
        }
        
        #endregion
    }
}
