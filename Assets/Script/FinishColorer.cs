using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishColorer : MonoBehaviour
{
    [SerializeField] MeshRenderer[] targetsRenderers;
    [SerializeField] Color sourceColor;
    [SerializeField] Color targetColor;
    [SerializeField] Color sourceColorTarget;
    [SerializeField] Color targetColorTarget;
    [SerializeField] AnimationCurve animationCurve;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < targetsRenderers.Length; i++)
        {
            Color color = GetColor(i, sourceColor, targetColor);
            Color colorTarget = GetColor(i, sourceColorTarget, targetColorTarget);
            targetsRenderers[i].materials[0].SetColor("_MainColor", colorTarget);
            targetsRenderers[i].materials[1].SetColor("_MainColor", color);
            SlicedObject slicedObject = targetsRenderers[i].GetComponent<SlicedObject>();
            if (slicedObject != null)
            {
                slicedObject.Color = colorTarget;
            }
        }
    }

    private Color GetColor(int i, Color sourceColor, Color targetColor)
    {
        Color.RGBToHSV(sourceColor, out float sourceHue, out float sourceSaturation, out float sourceValue);
        Color.RGBToHSV(targetColor, out float targetHue, out float targetSaturation, out float targetValue);

        float t = (float)i / targetsRenderers.Length;
        float hue = Mathf.Lerp(sourceHue, targetHue, t);
        float saturation = Mathf.Lerp(sourceSaturation, targetSaturation, t);
        float value = Mathf.Lerp(sourceValue, targetValue, t);
        Color color = Color.HSVToRGB(hue, saturation, value);
        return color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
