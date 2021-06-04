using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupColorGradientModifier : MonoBehaviour, ISlicedObjectsGroupModifier
{
    [SerializeField] Color sourceColor;
    [SerializeField] Color targetColor;

    public void OnSpawn(SpawnInfo info)
    {
        Color.RGBToHSV(sourceColor, out float sourceHue, out float sourceSaturation, out float sourceValue);
        Color.RGBToHSV(targetColor, out float targetHue, out float targetSaturation, out float targetValue);

        float t = (float)info.number / info.total;
        float hue = Mathf.Lerp(sourceHue, targetHue, t);
        float saturation = Mathf.Lerp(sourceSaturation, targetSaturation, t);
        float value = Mathf.Lerp(sourceValue, targetValue, t);
        Color color = Color.HSVToRGB(hue, saturation, value);
        info.spawnedObject.GetComponent<SlicedObjectColorSetter>().SetColor(color);
    }
}
