using TMPro;
using UnityEngine;

public class KeyboardActivator : MonoBehaviour
{
    [SerializeField] private GameObject KeyboardObject;

    public void Activate()
    {
        KeyboardObject.SetActive(true);
    }
}
