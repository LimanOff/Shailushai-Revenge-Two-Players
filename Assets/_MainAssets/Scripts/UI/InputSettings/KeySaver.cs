﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class KeySaver
{
    [SerializeField] private string _key;

    public void LoadSavedKeyBinding(Key keyToLoad, int keyBindIndex,ref PlayerInputController inputController)
    {
        if (PlayerPrefs.HasKey(_key))
        {
            string savedKeyPath = PlayerPrefs.GetString(_key);

            switch (savedKeyPath)
            {
                case "←":
                    savedKeyPath = "Left Arrow";
                    break;
                case "→":
                    savedKeyPath = "Right Arrow";
                    break;
                case "↑":
                    savedKeyPath = "Up Arrow";
                    break;
                case "↓":
                    savedKeyPath = "Down Arrow";
                    break;
            }
            inputController.GameInput.FindAction(keyToLoad.RebindInputActionReference.action.name).ApplyBindingOverride(keyBindIndex, savedKeyPath);
            keyToLoad.SetText(inputController.GameInput.FindAction(keyToLoad.RebindInputActionReference.action.name).bindings[keyBindIndex].ToDisplayString());
        }
        else
        {
            keyToLoad.SetText(inputController.GameInput.FindAction(keyToLoad.RebindInputActionReference.action.name).bindings[keyBindIndex].ToDisplayString());
        }
    }

    public void SaveKeyBinding(string keyPath)
    {
        switch (keyPath)
        {
            case "←":
                keyPath = "Left Arrow";
                break;
            case "→":
                keyPath = "Right Arrow";
                break;
            case "↑":
                keyPath = "Up Arrow";
                break;
            case "↓":
                keyPath = "Down Arrow";
                break;
        }
        PlayerPrefs.SetString(_key, keyPath);
        PlayerPrefs.Save();
    }
}
