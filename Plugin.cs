using BepInEx;
using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenuUIExtension
{
    [BepInPlugin("tairasoul.mainmenu.uiextension", "MainMenuExtension", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        internal void Awake()
        {
            Logger.LogInfo("MainMenuExtension awake.");
        }

        internal KeyboardTrigger.Key FindButtonByKey(KeyCode key)
        {
            GameObject baseCanvas = GameObject.Find("Canvas/Image");
            KeyboardTrigger trigger = baseCanvas.GetComponent<KeyboardTrigger>();
            foreach (KeyboardTrigger.Key KeyTrigger in trigger.Choices)
            {
                foreach (GameObject obj in baseCanvas.GetChildren())
                {
                    Text text = obj.GetComponent<Text>();
                    if (text.text.StartsWith(key.ToString()))
                    {
                        return KeyTrigger;
                    }
                }
            }
            return null;
        }

        internal KeyboardTrigger.Key FindButtonByKey(string key)
        {
            GameObject baseCanvas = GameObject.Find("Canvas/Image");
            KeyboardTrigger trigger = baseCanvas.GetComponent<KeyboardTrigger>();
            foreach (KeyboardTrigger.Key KeyTrigger in trigger.Choices)
            {
                foreach (GameObject obj in baseCanvas.GetChildren())
                {
                    Text text = obj.GetComponent<Text>();
                    if (text.text.StartsWith(key))
                    {
                        return KeyTrigger;
                    }
                }
            }
            return null;
        }

        internal GameObject FindElectric()
        {
            foreach (ParticleSystem system in Resources.FindObjectsOfTypeAll(typeof(ParticleSystem)).Cast<ParticleSystem>())
            {
                if (system.name != "Dust" && system.name != "sound")
                {
                    return system.gameObject;
                }
            }
            return null;
        }

        internal GameObject FindLoadingScreen()
        {
            HomeScreen screen = GameObject.FindFirstObjectByType<HomeScreen>();
            return screen.gameObject.transform.Find("LoadingScreen").gameObject;
        }

        internal void SetupOtherButtons()
        {
            GameObject baseCanvas = GameObject.Find("Canvas/Image");
            baseCanvas.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                KeyboardTrigger trigger = baseCanvas.GetComponent<KeyboardTrigger>();
                trigger.active = false;
                try
                {
                    baseCanvas.SetActive(false);
                    FindElectric().SetActive(true);
                    FindLoadingScreen().SetActive(true);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                }
            });
        }

        internal void Update()
        {
            if (SceneManager.GetActiveScene().name == "Menu" && GameObject.Find("Canvas/Image/ModSettings") == null)
            {
                SetupOtherButtons();
                GameObject baseCanvas = GameObject.Find("Canvas/Image");
                GameObject[] defaultTexts = baseCanvas.GetChildren(3);
                KeyboardTrigger.Key key = new()
                {
                    key = KeyCode.G,
                    Event = new()
                };
                GameObject button = utils.CreateBaseUIButton();
                button.name = "ModSettings";
                button.GetComponent<Text>().text = "G/ Mod Settings";
                button.transform.SetParent(baseCanvas.transform, false);
                button.GetComponent<RectTransform>().anchoredPosition = new Vector2(-180.0165f, -27.2514f);
                GameObject Pages = new("OptionPages");
                Pages.transform.SetParent(baseCanvas.transform, false);
                Pages.AddComponent<RectTransform>();
                Logger.LogInfo("Created ModSettings button for main menu.");
                KeyboardTrigger trigger = baseCanvas.GetComponent<KeyboardTrigger>();
                key.Event.AddListener(() =>
                {
                    foreach (GameObject obj in defaultTexts)
                    {
                        obj?.SetActive(false);
                    }
                    button.SetActive(false);
                    Pages.SetActive(true);
                    trigger.active = false;
                });
                button.GetComponent<Button>().onClick.AddListener(() =>
                {
                    foreach (GameObject obj in defaultTexts)
                    {
                        obj?.SetActive(false);
                    }
                    button.SetActive(false);
                    Pages.SetActive(true);
                    trigger.active = false;
                });
                GameObject LiquidNoiseImage = GameObject.Find("Canvas/LiquidNoise");
                LiquidNoiseImage.GetComponent<Image>().raycastTarget = false;
                trigger.Choices = [.. trigger.Choices, key];
            }
        }
    }
}
