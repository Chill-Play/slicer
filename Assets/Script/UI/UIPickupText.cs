using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPickupText : MonoBehaviour
{
    [SerializeField] string[] textVariants;
    [SerializeField] Text textPrefab;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField] AnimationCurve moveUpCurve;
    [SerializeField] float moveUpDistance = 3f;
    [SerializeField] float time = 2f;
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
        for (int i = numbers.Count - 1; i >= 0; i--)
        {
            UINumber number = numbers[i];
            Vector3 upOffset = Vector3.up * moveUpCurve.Evaluate(number.t) * moveUpDistance;
            Vector3 screenPos = camera.WorldToScreenPoint(number.worldPos + upOffset);
            number.text.transform.position = screenPos;
            number.t += Time.deltaTime / time;
            number.text.transform.localScale = Vector3.one * scaleCurve.Evaluate(number.t);
            if (number.t >= scaleCurve.keys[scaleCurve.keys.Length - 1].time)
            {
                Destroy(number.text.gameObject);
                numbers.RemoveAt(i);
            }
        }
    }


    public void SpawnText(Vector3 worldPos)
    {
        UINumber uINumber = new UINumber();
        uINumber.text = Instantiate(textPrefab, transform);
        uINumber.text.transform.SetAsFirstSibling();
        uINumber.text.text = textVariants[Random.Range(0, textVariants.Length)];
        uINumber.worldPos = worldPos;
        numbers.Add(uINumber);
    }
}
