using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppInfo : MonoBehaviour
{
    [Header("Info")]
    public string NameGame;
    public string AppID;
    public string BundleID;
    
    [Header("Facebook Analytic")]
    public string FacebookID;

    [Header("Game Analytic")]
    public string GameKey;
    public string SecretKey;
}
