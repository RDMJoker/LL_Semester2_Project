using Scriptables.Holder;
using UnityEngine;

namespace Interface
{
    public class TextSendHandler : MonoBehaviour
    {
        [SerializeField] TextHolder textHolder;
        [SerializeField] MainMenu mainMenu;


        public void SendText()
        {
            mainMenu.SetText(textHolder);
        }
    }
}