using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Road : MonoBehaviour {
    [Header("Road Infos")] 
    public SO_Road soRoad_;

    [Header("UI")] 
    [SerializeField] TextMeshProUGUI uiRoadName_;
    // Start is called before the first frame update
    void Start() {
        uiRoadName_.text = soRoad_.roadName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnValidate() {
       // uiRoadName_.text = soRoad_.roadName;
    }
}
