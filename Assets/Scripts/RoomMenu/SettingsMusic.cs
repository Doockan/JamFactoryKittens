using System.Collections;
using System.Collections.Generic;
using Core_Logic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMusic : MonoBehaviour
{
    // Start is called before the first frame update
    private Slider SoundSlider;
    private Slider MusicSlider;

    private MonoBehaviourSingleton mainSingleton;

    void Awake()
    {
        mainSingleton = GameObject.Find("MainSingleton").GetComponent<MonoBehaviourSingleton>();
    }

    

}
