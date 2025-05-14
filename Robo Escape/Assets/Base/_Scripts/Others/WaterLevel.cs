using UnityEngine.UI;
using UnityEngine;

public class WaterLevel : MonoBehaviour
{
    [SerializeField] private Image _waterFill;
    [SerializeField] private TMPro.TMP_Text _waterCounterText;

    private float _timeToFill;
    private float _currentFillAmount = 0f;
    private bool _canFill;
    private float _currentTime;

    void Awake()
    {
        _canFill = GameManager.Instance.WaterLevel;
        _timeToFill = LevelManager.Instance.LevelData.WaterFillTime;
    }

    void Update()
    {
        if(!_canFill) return;

        if(_waterFill.fillAmount >= 1f)
        {
            GameManager.Instance.ChangeGameState(GameState.Lose);
            _canFill = false;
            return;
        }
        
        _currentFillAmount += Time.deltaTime / _timeToFill;
        _currentTime += Time.deltaTime / 1;
        SetWaterCounter(_currentTime);
        SetWaterLevel(_currentFillAmount);
    }

    private void SetWaterLevel(float value) =>  _waterFill.fillAmount = Mathf.Clamp01(value);

    private void SetWaterCounter(float value)
    {
        _waterCounterText.text = $"{value.ToString("0")}/{_timeToFill}";
    }

    private void Fillable(GameState gameState)
    {
        if(gameState != GameState.Intro) _canFill = true;
        else _canFill = false;
    }

    void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += Fillable;
    }

    void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= Fillable;
    }
}
