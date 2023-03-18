using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mBackground : MonoBehaviour
{
    public UITexture background;
    public Texture[] backgrounds;

    private void Start()
    {
        int i = UnityEngine.Random.Range(0, 8);
        background.mainTexture = backgrounds[i];
    }
}