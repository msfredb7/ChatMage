using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CCC.UI
{
    [RequireComponent(typeof(Button))]
    public class ButtonSound : MonoBehaviour
    {
        public AudioClip clip;
        void Start()
        {
            PersistentLoader.LoadIfNotLoaded();
            GetComponent<Button>().onClick.AddListener(OnClick);
        }
        void OnClick()
        {
            DefaultAudioSources.PlaySFX(clip);
        }
    }
}
