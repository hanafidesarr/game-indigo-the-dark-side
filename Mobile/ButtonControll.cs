using System;
using UnityEngine;

namespace UnityStandardAssets.CrossPlatformInput
{
    public class ButtonControll : MonoBehaviour
    {

        public string Name;


        public void SetDownState()
        {
            CrossPlatformInputManager.SetButtonDown(Name);
        }


        public void SetUpState()
        {
            CrossPlatformInputManager.SetButtonUp(Name);
        }


    }
}

// using System;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityStandardAssets.CrossPlatformInput;

// public class ButtonControll : MonoBehaviour
// {
//     public Sprite crouchSprite;
//     public Sprite standSprite;

//     public string Name;

//     private Image buttonImage;
//     private bool isCrouching = false;

//     private void Start()
//     {
//         buttonImage = GetComponent<Image>();
//         if (buttonImage != null && standSprite != null)
//         {
//             buttonImage.sprite = standSprite;
//         }
//     }

//     // Fungsi toggle saat tombol diklik
//     public void ToggleCrouch()
//     {
//         if (!isCrouching)
//         {
//             Crouch();
//         }
//         else
//         {
//             Stand();
//         }
//     }

//     // Ini fungsi yang bisa juga dipanggil dari event lain
//     public void SetDownState()
//     {
//         Crouch();
//     }

//     public void SetUpState()
//     {
//         Stand();
//     }

//     // Logika crouch sebenarnya
//     private void Crouch()
//     {
//         isCrouching = true;

//         if (buttonImage != null && crouchSprite != null)
//         {
//             buttonImage.sprite = crouchSprite;
//         }

//         CrossPlatformInputManager.SetButtonDown(Name);

//         // TODO: Panggil crouch karakter di sini
//         Debug.Log("Crouch ON");
//     }

//     // Logika berdiri sebenarnya
//     private void Stand()
//     {
//         isCrouching = false;

//         if (buttonImage != null && standSprite != null)
//         {
//             buttonImage.sprite = standSprite;
//         }

//         CrossPlatformInputManager.SetButtonUp(Name);

//         // TODO: Kembali berdiri di sini
//         Debug.Log("Crouch OFF");
//     }
// }
