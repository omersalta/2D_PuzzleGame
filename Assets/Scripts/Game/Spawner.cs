using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public int columnNo { get; private set; }
    
    public void Initialize(int myColumnNo) {
        columnNo = myColumnNo;
    }
}
