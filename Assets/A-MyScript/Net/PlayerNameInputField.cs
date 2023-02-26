using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Com.MyCompany.MyGunGame
{
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {
        private const string PlayerNamePrefKey = "PlayerName";
        private void Start()
        {
            if (TryGetComponent(out InputField inputField))
            {
                if (PlayerPrefs.HasKey(PlayerNamePrefKey))
                {
                    inputField.text = PlayerPrefs.GetString(PlayerNamePrefKey);
                }
                
                inputField.onEndEdit.AddListener(SetPlayerName);
                PhotonNetwork.NickName = inputField.text;
            }
        }

        public void SetPlayerName(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Player Name is null or empty");
                return;
            }

            PhotonNetwork.NickName = value;
            
            PlayerPrefs.SetString(PlayerNamePrefKey,value);
        }
    }
}
