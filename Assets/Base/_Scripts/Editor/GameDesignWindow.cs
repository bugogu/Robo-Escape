using UnityEditor;
using UnityEngine;

public class GameDesignWindow : EditorWindow
{
    #region Fields

    private GameDesignData _designData;
    private bool _showCharacterMovementSettings = false;
    private bool _showCharacterEnergySettings = false;
    private bool _showUIPopUpTextSettings = false;
    private bool _showGeneralUISettings = false;
    private bool _showPowerUpSettings = false;
    private bool _showCameraShakeSettings = false;
    private bool _showMaxCapacitySettings = false;

    private Vector2 _scrollPosition;

    #endregion

    #region Setup
    
    [MenuItem("Design/Game Design Window")]
    public static void ShowWindow()
    {
        GetWindow<GameDesignWindow>("Game Design Window");
    }

    private void OnEnable()
    {
        _designData = Resources.Load<GameDesignData>("GameDesignData");
    }

    private void OnGUI()
    {
         if (_designData == null) return;

         _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
         {
             DrawPlayerSettingsContainer();
             DrawUISettingsContainer();
             DrawCameraSettingsContainer();
             DrawUpgradeSettingsContainer();
         }
         EditorGUILayout.EndScrollView();
        
        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(_designData);
            AssetDatabase.SaveAssets();
        }
    }

    #endregion

    #region Containers

    private void DrawPlayerSettingsContainer()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField("Player Settings", EditorStyles.boldLabel);

            EditorGUI.indentLevel++; 

            DrawCharacterMovementSettings();

            EditorGUILayout.Space(5);

            DrawCharacterEnergySettings();

            EditorGUILayout.Space(5);

            DrawCharacterPowerUpSettings();

            EditorGUILayout.Space(5);

            EditorGUI.indentLevel--; 
        }
        
        EditorGUILayout.EndVertical(); 
    }

    private void DrawUISettingsContainer()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField("User Interface Settings", EditorStyles.boldLabel);

            EditorGUI.indentLevel++; 

            DrawGeneralUISettings();

            EditorGUILayout.Space(5);

            DrawEnergyPopTextSettings();

            EditorGUILayout.Space(5);

            EditorGUI.indentLevel--; 
        }
        
        EditorGUILayout.EndVertical(); 
    }

    private void DrawCameraSettingsContainer()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField("Camera Settings", EditorStyles.boldLabel);

            EditorGUI.indentLevel++; 

            DrawCameraShakeSettings();

            EditorGUILayout.Space(5);

            EditorGUI.indentLevel--; 
        }
        
        EditorGUILayout.EndVertical(); 
    }

    private void DrawUpgradeSettingsContainer()
    {
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        {
            EditorGUILayout.LabelField("Upgrade Settings", EditorStyles.boldLabel);

            EditorGUI.indentLevel++; 

            DrawMaxCapacitySettings();

            EditorGUILayout.Space(5);

            EditorGUI.indentLevel--; 
        }
        
        EditorGUILayout.EndVertical(); 
    }
    
    #endregion

    #region Foldouts

    private void DrawCharacterMovementSettings()
    {
        _showCharacterMovementSettings = EditorGUILayout.Foldout(
        _showCharacterMovementSettings, 
        "Movement Settings", 
        true, 
        EditorStyles.boldLabel
        );

        if (_showCharacterMovementSettings)
        {
            EditorGUI.indentLevel++; 

        EditorGUILayout.LabelField("Run Speed", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.CharacterRunSpeed = EditorGUILayout.FloatField(_designData.CharacterRunSpeed, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Walk Speed", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.CharacterWalkSpeed = EditorGUILayout.FloatField(_designData.CharacterWalkSpeed, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Turn Speed", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.CharacterTurnSpeed = EditorGUILayout.FloatField(_designData.CharacterTurnSpeed, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Magnetism Speed Multiplier", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.MagnetismSpeedMultiplier = EditorGUILayout.FloatField(_designData.MagnetismSpeedMultiplier, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Movement Threshold", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.MovementThreshold = EditorGUILayout.FloatField(_designData.MovementThreshold, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Stop Threshold", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.StopThreshold = EditorGUILayout.FloatField(_designData.StopThreshold, EditorStyles.miniTextField);

        EditorGUI.indentLevel--; 

        }
    }

    private void DrawCharacterEnergySettings()
     {
        _showCharacterEnergySettings = EditorGUILayout.Foldout(
        _showCharacterEnergySettings, 
        "Energy Settings", 
        true, 
        EditorStyles.boldLabel
        );

        if (_showCharacterEnergySettings)
        {
            EditorGUI.indentLevel++; 

        EditorGUILayout.LabelField("Max Energy Capacity", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.MaxEnergyCapacity = EditorGUILayout.FloatField(_designData.MaxEnergyCapacity, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Replenish Amount", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.ReplenishAmount = EditorGUILayout.FloatField(_designData.ReplenishAmount, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);
        
        EditorGUILayout.LabelField("Consume Amount", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.ConsumeAmount = EditorGUILayout.FloatField(_designData.ConsumeAmount, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Magnetism Consumption Multiplier", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.MagnetismConsumptionMultiplier = EditorGUILayout.FloatField(_designData.MagnetismConsumptionMultiplier, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Magnetism Replenish Multiplier", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.MagnetismReplenishMultiplier = EditorGUILayout.FloatField(_designData.MagnetismReplenishMultiplier, EditorStyles.miniTextField);

            EditorGUI.indentLevel--;
        }
     }

    private void DrawEnergyPopTextSettings()
    {
        _showUIPopUpTextSettings = EditorGUILayout.Foldout(
        _showUIPopUpTextSettings, 
        "Energy PopUp Text", 
        true, 
        EditorStyles.boldLabel
        );

        if (_showUIPopUpTextSettings)
        {
            EditorGUI.indentLevel++; 

        EditorGUILayout.LabelField("Move Distance", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.MoveDistance = EditorGUILayout.FloatField(_designData.MoveDistance, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Move Duration", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.MoveDuration = EditorGUILayout.FloatField(_designData.MoveDuration, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Return Duration", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.ReturnDuration = EditorGUILayout.FloatField(_designData.ReturnDuration, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Display Duration", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.DisplayDuration = EditorGUILayout.FloatField(_designData.DisplayDuration, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        _designData.PositiveColor = EditorGUILayout.ColorField(new GUIContent("Positive Color", "Energy Replenish"),_designData.PositiveColor, GUILayout.Width(205f));

        EditorGUILayout.Space(5);

        _designData.NegativeColor = EditorGUILayout.ColorField(new GUIContent("Negative Color", "Energy Consumption"),_designData.NegativeColor, GUILayout.Width(205f));

        EditorGUI.indentLevel--; 
    } 
  }
    
    private void DrawGeneralUISettings()
    {
        _showGeneralUISettings = EditorGUILayout.Foldout(
        _showGeneralUISettings, 
        "General", 
        true, 
        EditorStyles.boldLabel
        );

        if (_showGeneralUISettings)
        {
            EditorGUI.indentLevel++; 

        EditorGUILayout.LabelField("Home Button Delay", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.MenuLoadDelay = EditorGUILayout.FloatField(_designData.MenuLoadDelay, EditorStyles.miniTextField);
        }
    }

    private void DrawCharacterPowerUpSettings()
    {
        _showPowerUpSettings = EditorGUILayout.Foldout(
        _showPowerUpSettings, 
        "Power Ups", 
        true, 
        EditorStyles.boldLabel
        );

        if (_showPowerUpSettings)
        {
            EditorGUI.indentLevel++; 

        EditorGUILayout.LabelField("Flash Speed Multiplier", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.FlashSpeedMultiplier = EditorGUILayout.FloatField(_designData.FlashSpeedMultiplier, EditorStyles.miniTextField);

        EditorGUILayout.LabelField("Flash Power Up Duration", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.FlashPowerUpDuration = EditorGUILayout.FloatField(_designData.FlashPowerUpDuration, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Flash Outline Color", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.FlashOutlineColor = EditorGUILayout.ColorField(_designData.FlashOutlineColor, GUILayout.Width(205f));

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Shield Outline Color", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.ShieldOutlineColor = EditorGUILayout.ColorField(_designData.ShieldOutlineColor, GUILayout.Width(205f));

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("EMP Outline Color", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.EmpOutlineColor = EditorGUILayout.ColorField(_designData.EmpOutlineColor, GUILayout.Width(205f));

        EditorGUILayout.Space(5);

            EditorGUI.indentLevel--; 
        
        }
    }

    private void DrawCameraShakeSettings()
    {
        _showCameraShakeSettings = EditorGUILayout.Foldout(
        _showCameraShakeSettings, 
        "Camera Shake", 
        true, 
        EditorStyles.boldLabel
        );

        if (_showCameraShakeSettings)
        {
        
        EditorGUI.indentLevel++; 

        EditorGUILayout.LabelField("Shake Duration", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.CameraShakeDuration = EditorGUILayout.FloatField(_designData.CameraShakeDuration, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Shake Amplitude", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.CameraShakeAmplitude = EditorGUILayout.FloatField(_designData.CameraShakeAmplitude, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Shake Frequency", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.CameraShakeFrequency = EditorGUILayout.FloatField(_designData.CameraShakeFrequency, EditorStyles.miniTextField);

        EditorGUI.indentLevel--; 
        
        }
    }

    private void DrawMaxCapacitySettings()
    {
        _showMaxCapacitySettings = EditorGUILayout.Foldout(
        _showMaxCapacitySettings, 
        "Energy Capacity", 
        true, 
        EditorStyles.boldLabel
        );

        if (_showMaxCapacitySettings)
        {
        
        EditorGUI.indentLevel++; 

        EditorGUILayout.LabelField("Increase Amount", EditorStyles.label);
        EditorGUILayout.Space(2);
        _designData.EnergyCapacityUpgradeAmount = EditorGUILayout.FloatField(_designData.EnergyCapacityUpgradeAmount, EditorStyles.miniTextField);

        EditorGUI.indentLevel--; 
        
        }
    }

    #endregion
  
}