using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(TMP_Text))]
public class PrototypeText : MonoBehaviour
{
    void Start(){
     GetComponent<TMP_Text>().text = $"ver: {Application.version}";
    }
}

