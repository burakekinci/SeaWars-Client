using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;


public class PlayerListItem : MonoBehaviour
{
    public string PlayerName;
    public int ConnectionID;
    public ulong PlayerSteamID;
    private bool AvatarReceived;

    public Text playerNameText;
    public RawImage playerIcon;

    protected Callback<AvatarImageLoaded_t> ImageLoaded;

    private void Start() 
    {
        ImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnImageLoaded);
    }


    void GetPlayerIcon()
    {
        int ImageID = SteamFriends.GetLargeFriendAvatar((CSteamID)PlayerSteamID);
        if(ImageID == -1) {return;}
        playerIcon.texture = GetSteamImageAsTexture(ImageID); 
    }

    private void OnImageLoaded(AvatarImageLoaded_t callback)
    {
        if(callback.m_steamID.m_SteamID == PlayerSteamID)
        {
            playerIcon.texture = GetSteamImageAsTexture(callback.m_iImage);
        }
        else
        {
            return;    
        }
    }

    private Texture2D GetSteamImageAsTexture(int iImage)
    {
        Texture2D texture = null;

        bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint heigth);
        if(isValid)
        {
            byte[] image = new byte[width*heigth*4];
            isValid = SteamUtils.GetImageRGBA(iImage,image,(int)(width*heigth*4));

            if(isValid)
            {
                texture = new Texture2D((int)width,(int)heigth, TextureFormat.RGBA32, false,true);
                texture.LoadRawTextureData(image);
                texture.Apply();
            }
        }
        AvatarReceived = true;
        return texture;
    }

    public void SetPlayerValues()
    {
        playerNameText.text = PlayerName;
        if(!AvatarReceived)
        {
            GetPlayerIcon();
        }
    }

}
