using UnityEngine ;
using UnityEngine.UI ;
using DG.Tweening ;
using UnityEngine.Serialization;

public class SwitchToggle : MonoBehaviour 
{
   [FormerlySerializedAs("uiHandleRectTransform")] [ SerializeField] RectTransform _uiHandleRectTransform ;
   [FormerlySerializedAs("backgroundActiveColor")] [ SerializeField] Color _backgroundActiveColor ;
   [FormerlySerializedAs("handleActiveColor")] [ SerializeField] Color _handleActiveColor ;
   [SerializeField] SwitchType _switchType ;

   private enum SwitchType { Music, Sound, Haptic, Outlines }

   Image _backgroundImage, _handleImage ;

   Color _backgroundDefaultColor, _handleDefaultColor ;

   Toggle _toggle ;

   Vector2 _handlePosition ;

   private bool _firstLoad = true;

   void Awake ( ) {
      _toggle = GetComponent <Toggle> ( ) ;

      _handlePosition = _uiHandleRectTransform.anchoredPosition ;

      _backgroundImage = _uiHandleRectTransform.parent.GetComponent <Image> ( ) ;
      _handleImage = _uiHandleRectTransform.GetComponent <Image> ( ) ;

      _backgroundDefaultColor = _backgroundImage.color ;
      _handleDefaultColor = _handleImage.color ;

      _toggle.onValueChanged.AddListener (OnSwitch) ;

      switch(_switchType)
      {
         case SwitchType.Music:
            _toggle.isOn = Settings.Instance.Music == 1 ? true : false;
            break;
         case SwitchType.Sound:
            _toggle.isOn = Settings.Instance.Sound == 1 ? true : false;
            break;
         case SwitchType.Haptic:
            _toggle.isOn = Settings.Instance.Haptic == 1 ? true : false;
            break;
         case SwitchType.Outlines:
            _toggle.isOn = Settings.Instance.Outlines == 1 ? true : false;
            break;
      }

      if (_toggle.isOn)
         OnSwitch (true) ;
   }

   void OnSwitch (bool on) {

      Settings.Instance.PlayButtonSound();

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
            break;
         case SwitchType.Outlines:
            OutlinesToggle ( ) ;
            break ;
      }

      //_uiHandleRectTransform.anchoredPosition = on ? _handlePosition * -1 : _handlePosition ; // no anim
      _uiHandleRectTransform.DOAnchorPos (on ? _handlePosition * -1 : _handlePosition, .4f).SetEase (Ease.InOutBack) ;

      //_backgroundImage.color = on ? _backgroundActiveColor : _backgroundDefaultColor ; // no anim
      _backgroundImage.DOColor (on ? _backgroundActiveColor : _backgroundDefaultColor, .6f) ;

      //_handleImage.color = on ? _handleActiveColor : _handleDefaultColor ; // no anim
      _handleImage.DOColor (on ? _handleActiveColor : _handleDefaultColor, .4f);

      _firstLoad = false;
   }

   void OnDestroy ( ) {
      _toggle.onValueChanged.RemoveListener (OnSwitch) ;
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
   private void OutlinesToggle()
   {
      if(Settings.Instance.Outlines == 1)
      {
         Settings.Instance.Outlines = 0;
         Settings.Instance.SetOutlines(false);
      }
      else
      {
         Settings.Instance.Outlines = 1;
         Settings.Instance.SetOutlines(true);
      }
   }
}
