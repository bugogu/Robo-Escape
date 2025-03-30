using UnityEngine ;
using UnityEngine.UI ;
using DG.Tweening ;

public class SwitchToggle : MonoBehaviour {
   [SerializeField] RectTransform uiHandleRectTransform ;
   [SerializeField] Color backgroundActiveColor ;
   [SerializeField] Color handleActiveColor ;
   [SerializeField] SwitchType _switchType ;
   private enum SwitchType { Music, Sound, Haptic }

   Image backgroundImage, handleImage ;

   Color backgroundDefaultColor, handleDefaultColor ;

   Toggle toggle ;

   Vector2 handlePosition ;

   private bool _firstLoad = true;

   void Awake ( ) {
      toggle = GetComponent <Toggle> ( ) ;

      handlePosition = uiHandleRectTransform.anchoredPosition ;

      backgroundImage = uiHandleRectTransform.parent.GetComponent <Image> ( ) ;
      handleImage = uiHandleRectTransform.GetComponent <Image> ( ) ;

      backgroundDefaultColor = backgroundImage.color ;
      handleDefaultColor = handleImage.color ;

      toggle.onValueChanged.AddListener (OnSwitch) ;

      switch(_switchType)
      {
         case SwitchType.Music:
            toggle.isOn = Settings.Instance.Music == 1 ? true : false;
            break;
         case SwitchType.Sound:
            toggle.isOn = Settings.Instance.Sound == 1 ? true : false;
            break;
         case SwitchType.Haptic:
            toggle.isOn = Settings.Instance.Haptic == 1 ? true : false;
            break;
      }

      if (toggle.isOn)
         OnSwitch (true) ;
   }

   void OnSwitch (bool on) {

      if (!_firstLoad)
      switch (_switchType) {
         case SwitchType.Music:
            MusicToggle ( ) ;
            break ;
         case SwitchType.Sound:
            SoundToggle ( ) ;
            break ;
         case SwitchType.Haptic:
            HapticToggle ( ) ;
            break ;
      }

      //uiHandleRectTransform.anchoredPosition = on ? handlePosition * -1 : handlePosition ; // no anim
      uiHandleRectTransform.DOAnchorPos (on ? handlePosition * -1 : handlePosition, .4f).SetEase (Ease.InOutBack) ;

      //backgroundImage.color = on ? backgroundActiveColor : backgroundDefaultColor ; // no anim
      backgroundImage.DOColor (on ? backgroundActiveColor : backgroundDefaultColor, .6f) ;

      //handleImage.color = on ? handleActiveColor : handleDefaultColor ; // no anim
      handleImage.DOColor (on ? handleActiveColor : handleDefaultColor, .4f);

      _firstLoad = false;
   }

   void OnDestroy ( ) {
      toggle.onValueChanged.RemoveListener (OnSwitch) ;
   }

   private void MusicToggle()
   {
      if(Settings.Instance.Music == 1)
      {
         Settings.Instance.Music = 0;
         BackgroundMusic.Instance.MusicEnabled();
      }
      else
      {
         Settings.Instance.Music = 1;
         BackgroundMusic.Instance.MusicEnabled();
      }
         
   }
   private void SoundToggle()
   {
      if(Settings.Instance.Sound == 1)
         Settings.Instance.Sound = 0;
      else
         Settings.Instance.Sound = 1;
   }
   private void HapticToggle()
   {
      if(Settings.Instance.Haptic == 1)
         Settings.Instance.Haptic = 0;
      else
         Settings.Instance.Haptic = 1;
   }
}
