//using UnityEngine;
//using UnityEngine.UI;
//using Photon.Pun;

//using System.Collections;


//namespace Com.MyCompany.MyGame
//{
//    public class PlayerUI : MonoBehaviourPunCallbacks
//    {
//        #region Private Fields


//        [Tooltip("UI Text to display Player's Name")]
//        [SerializeField]
//        private Text playerNameText;
//        [SerializeField]
//        private Text playerHealthText;
//        [SerializeField]
//        private Text playerNameTextSelf;
//        [SerializeField]
//        private Text playerHealthTextSelf;


//        [Tooltip("UI Slider to display Player's Health")]
//        [SerializeField]
//        private Slider playerHealthSlider;
//        [SerializeField]
//        private PlayerManager target;
//        [SerializeField]
//        private Slider playerHealthSliderSelf;

//        #endregion



//        #region MonoBehaviour Callbacks

//        private void Start()
//        {
//            if (target!=null&&playerNameText != null)
//            {
//                playerNameText.text = target.photonView.Owner.NickName;
//                playerNameTextSelf.text = target.photonView.Owner.NickName;
//            }
//            if(!photonView.IsMine)
//            {
//                playerHealthSliderSelf.gameObject.SetActive(false);
//            }
//        }

//        void Update()
//        {
//            // Reflect the Player Health
//            if (playerHealthSlider != null)
//            {
//                playerHealthSlider.value = target.Health;
//                playerHealthText.text = "health:"+target.Health.ToString();
//                playerHealthSliderSelf.value = target.Health;
//                playerHealthTextSelf.text = "health:" + target.Health.ToString();
//            }
//        }

//        #endregion


//        #region Public Methods


//        #endregion


//    }
//}
