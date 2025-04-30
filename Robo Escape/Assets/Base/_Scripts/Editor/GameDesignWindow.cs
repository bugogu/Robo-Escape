using System;
using UnityEditor;
using UnityEngine;

public class GameDesignWindow : EditorWindow
{
    private GameDesignData designData;
    private bool showCharacterMovementSettings = false;
    private bool showCharacterEnergySettings = false;
    private bool showUIPopUpTextSettings = false;
    private bool showGeneralUISettings = false;
    private bool showPowerUpSettings = false;

    private Vector2 scrollPosition;
    
    [MenuItem("Design/Game Design Window")]
    public static void ShowWindow()
    {
        GetWindow<GameDesignWindow>("Game Design Window");
    }

    private void OnEnable()
    {
        designData = Resources.Load<GameDesignData>("GameDesignData");
    }

    private void OnGUI()
    {
         if (designData == null) return;

         scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
         {
             DrawPlayerSettingsContainer();
             DrawUISettingsContainer();
         }
         EditorGUILayout.EndScrollView();
        
        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(designData);
            AssetDatabase.SaveAssets();
        }
    }

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
    
    #endregion

    #region Foldouts

    private void DrawCharacterMovementSettings()
    {
        showCharacterMovementSettings = EditorGUILayout.Foldout(
        showCharacterMovementSettings, 
        "Movement Settings", 
        true, 
        EditorStyles.boldLabel
        );

        if (showCharacterMovementSettings)
        {
            EditorGUI.indentLevel++; 

        EditorGUILayout.LabelField("Run Speed", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.characterRunSpeed = EditorGUILayout.FloatField(designData.characterRunSpeed, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Walk Speed", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.characterWalkSpeed = EditorGUILayout.FloatField(designData.characterWalkSpeed, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Turn Speed", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.characterTurnSpeed = EditorGUILayout.FloatField(designData.characterTurnSpeed, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Magnetism Speed Multiplier", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.magnetismSpeedMultiplier = EditorGUILayout.FloatField(designData.magnetismSpeedMultiplier, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Movement Threshold", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.movementThreshold = EditorGUILayout.FloatField(designData.movementThreshold, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Stop Threshold", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.stopThreshold = EditorGUILayout.FloatField(designData.stopThreshold, EditorStyles.miniTextField);

        EditorGUI.indentLevel--; 

        }
    }

    private void DrawCharacterEnergySettings()
     {
        showCharacterEnergySettings = EditorGUILayout.Foldout(
        showCharacterEnergySettings, 
        "Energy Settings", 
        true, 
        EditorStyles.boldLabel
        );

        if (showCharacterEnergySettings)
        {
            EditorGUI.indentLevel++; 

        EditorGUILayout.LabelField("Max Energy Capacity", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.maxEnergyCapacity = EditorGUILayout.FloatField(designData.maxEnergyCapacity, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Replenish Amount", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.replenishAmount = EditorGUILayout.FloatField(designData.replenishAmount, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);
        
        EditorGUILayout.LabelField("Consume Amount", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.consumeAmount = EditorGUILayout.FloatField(designData.consumeAmount, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Magnetism Consumption Multiplier", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.magnetismConsumptionMultiplier = EditorGUILayout.FloatField(designData.magnetismConsumptionMultiplier, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Magnetism Replenish Multiplier", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.magnetismReplenishMultiplier = EditorGUILayout.FloatField(designData.magnetismReplenishMultiplier, EditorStyles.miniTextField);

            EditorGUI.indentLevel--;
        }
     }

    private void DrawEnergyPopTextSettings()
    {
        showUIPopUpTextSettings = EditorGUILayout.Foldout(
        showUIPopUpTextSettings, 
        "Energy PopUp Text", 
        true, 
        EditorStyles.boldLabel
        );

        if (showUIPopUpTextSettings)
        {
            EditorGUI.indentLevel++; 

        EditorGUILayout.LabelField("Move Distance", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.moveDistance = EditorGUILayout.FloatField(designData.moveDistance, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Move Duration", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.moveDuration = EditorGUILayout.FloatField(designData.moveDuration, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Return Duration", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.returnDuration = EditorGUILayout.FloatField(designData.returnDuration, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Display Duration", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.displayDuration = EditorGUILayout.FloatField(designData.displayDuration, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        designData._positiveColor = EditorGUILayout.ColorField(new GUIContent("Positive Color", "Energy Replenish"),designData._positiveColor, GUILayout.Width(205f));

        EditorGUILayout.Space(5);

        designData._negativeColor = EditorGUILayout.ColorField(new GUIContent("Negative Color", "Energy Consumption"),designData._negativeColor, GUILayout.Width(205f));

        EditorGUI.indentLevel--; 
    } 
  }
    
    private void DrawGeneralUISettings()
    {
        showGeneralUISettings = EditorGUILayout.Foldout(
        showGeneralUISettings, 
        "General", 
        true, 
        EditorStyles.boldLabel
        );

        if (showGeneralUISettings)
        {
            EditorGUI.indentLevel++; 

        EditorGUILayout.LabelField("Home Button Delay", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.menuLoadDelay = EditorGUILayout.FloatField(designData.menuLoadDelay, EditorStyles.miniTextField);
        }
    }

    private void DrawCharacterPowerUpSettings()
    {
        showPowerUpSettings = EditorGUILayout.Foldout(
        showPowerUpSettings, 
        "Power Ups", 
        true, 
        EditorStyles.boldLabel
        );

        if (showPowerUpSettings)
        {
            EditorGUI.indentLevel++; 

        EditorGUILayout.LabelField("Flash Speed Multiplier", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.flashSpeedMultiplier = EditorGUILayout.FloatField(designData.flashSpeedMultiplier, EditorStyles.miniTextField);

        EditorGUILayout.LabelField("Flash Power Up Duration", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.flashPowerUpDuration = EditorGUILayout.FloatField(designData.flashPowerUpDuration, EditorStyles.miniTextField);

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Flash Outline Color", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.flashOutlineColor = EditorGUILayout.ColorField(designData.flashOutlineColor, GUILayout.Width(205f));

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("Shield Outline Color", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.shieldOutlineColor = EditorGUILayout.ColorField(designData.shieldOutlineColor, GUILayout.Width(205f));

        EditorGUILayout.Space(5);

        EditorGUILayout.LabelField("EMP Outline Color", EditorStyles.label);
        EditorGUILayout.Space(2);
        designData.empOutlineColor = EditorGUILayout.ColorField(designData.empOutlineColor, GUILayout.Width(205f));

        EditorGUILayout.Space(5);
    }

    #endregion
  
  }
}