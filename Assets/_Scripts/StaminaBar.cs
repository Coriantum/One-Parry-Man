using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaBar;

    public void SetMaxStamina(float stamina){
        staminaBar.maxValue = stamina;
        staminaBar.value = stamina;
    }

    public void SetStamina(float stamina){
        staminaBar.value = stamina;
    }
}
