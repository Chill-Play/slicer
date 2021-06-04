using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;


class UINumber
{
    public Text text;
    public Vector3 worldPos;
    public float t;
}


public class UINumbers : MonoBehaviour
{
    [SerializeField] Text textPrefab;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] AnimationCurve moveUpCurve;
    [SerializeField] float moveUpDistance = 3f;
    List<UINumber> numbers = new List<UINumber>();
    Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = numbers.Count - 1; i >= 0; i--)
        {
            UINumber number = numbers[i];
            Vector3 upOffset = Vector3.up * moveUpCurve.Evaluate(number.t) * moveUpDistance;
            Vector3 screenPos = camera.WorldToScreenPoint(number.worldPos + upOffset);
            number.text.transform.position = screenPos;
            number.t += Time.deltaTime;
            number.text.transform.localScale = Vector3.one * scaleCurve.Evaluate(number.t);
            if(number.t >= scaleCurve.keys[scaleCurve.keys.Length - 1].time)
            {
                Destroy(number.text.gameObject);
                numbers.RemoveAt(i);
            }
        }
    }


    public void SpawnNumber(Vector3 worldPos)
    {
        UINumber uINumber = new UINumber();
        uINumber.text = Instantiate(textPrefab, transform);
        uINumber.text.transform.SetAsFirstSibling();
        uINumber.worldPos = worldPos;
        numbers.Add(uINumber);
    }
}
