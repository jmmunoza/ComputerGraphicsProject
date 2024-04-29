using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UILogic : MonoBehaviour
{
    public GameObject element;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        FloatField xField = root.Q<FloatField>("X");
        FloatField yField = root.Q<FloatField>("Y");
        FloatField zField = root.Q<FloatField>("Z");

        Slider xRotationSlider = root.Q<Slider>("RotationX");
        Slider yRotationSlider = root.Q<Slider>("RotationY");
        Slider zRotationSlider = root.Q<Slider>("RotationZ");

        Slider scaleSlider = root.Q<Slider>("Scale");
        scaleSlider.value = 1;

        Button apply = root.Q<Button>("ApplyButton");

        apply.clicked += () =>
        {
            Matrix4x4 t = T(xField.value, yField.value, zField.value);
            element.transform.position = Times(t, element.transform.position);

            Matrix4x4 s = S(scaleSlider.value, scaleSlider.value, scaleSlider.value);
            element.transform.localScale = Times(s, element.transform.localScale);

            Matrix4x4 rx = Rx(xRotationSlider.value);
            Matrix4x4 ry = Ry(yRotationSlider.value);
            Matrix4x4 rz = Rz(zRotationSlider.value);
            Matrix4x4 r = rz * ry * rx;

            element.transform.rotation = Quaternion.LookRotation(
             Times(r, element.transform.forward),
             Times(r, element.transform.up)
            );
        };
    }

    private Vector3 Times(Matrix4x4 a, Vector3 b)
    {
        Vector3 vector = new()
        {
            x = a.GetRow(0).x * b.x + a.GetRow(0).y * b.y + a.GetRow(0).z * b.z + a.GetRow(0).w,
            y = a.GetRow(1).x * b.x + a.GetRow(1).y * b.y + a.GetRow(1).z * b.z + a.GetRow(1).w,
            z = a.GetRow(2).x * b.x + a.GetRow(2).y * b.y + a.GetRow(2).z * b.z + a.GetRow(2).w
        };

        return vector;
    }

    private Matrix4x4 T(float x, float y, float z)
    {
        Matrix4x4 matrix = new();

        matrix.SetRow(0, new Vector4(1, 0, 0, x));
        matrix.SetRow(1, new Vector4(0, 1, 0, y));
        matrix.SetRow(2, new Vector4(0, 0, 1, z));
        matrix.SetRow(3, new Vector4(0, 0, 0, 1));

        return matrix;
    }

    private Matrix4x4 S(float x, float y, float z)
    {
        Matrix4x4 matrix = new();

        matrix.SetRow(0, new Vector4(x, 0, 0, 0));
        matrix.SetRow(1, new Vector4(0, y, 0, 0));
        matrix.SetRow(2, new Vector4(0, 0, z, 0));
        matrix.SetRow(3, new Vector4(0, 0, 0, 1));

        return matrix;
    }

    private Matrix4x4 Rx(float angle)
    {
        Matrix4x4 matrix = new();

        float radians = angle * Mathf.Deg2Rad;

        matrix.SetRow(0, new Vector4(1, 0, 0, 0));
        matrix.SetRow(1, new Vector4(0, Mathf.Cos(radians), -Mathf.Sin(radians), 0));
        matrix.SetRow(2, new Vector4(0, Mathf.Sin(radians), Mathf.Cos(radians), 0));
        matrix.SetRow(3, new Vector4(0, 0, 0, 1));

        return matrix;
    }

    private Matrix4x4 Ry(float angle)
    {
        Matrix4x4 matrix = new();

        float radians = angle * Mathf.Deg2Rad;

        matrix.SetRow(0, new Vector4(Mathf.Cos(radians), 0, Mathf.Sin(radians), 0));
        matrix.SetRow(1, new Vector4(0, 1, 0, 0));
        matrix.SetRow(2, new Vector4(-Mathf.Sin(radians), 0, Mathf.Cos(radians), 0));
        matrix.SetRow(3, new Vector4(0, 0, 0, 1));

        return matrix;
    }

    private Matrix4x4 Rz(float angle)
    {
        Matrix4x4 matrix = new();

        float radians = angle * Mathf.Deg2Rad;

        matrix.SetRow(0, new Vector4(Mathf.Cos(radians), -Mathf.Sin(radians), 0, 0));
        matrix.SetRow(1, new Vector4(Mathf.Sin(radians), Mathf.Cos(radians), 0, 0));
        matrix.SetRow(2, new Vector4(0, 0, 1, 0));
        matrix.SetRow(3, new Vector4(0, 0, 0, 1));

        return matrix;
    }
}
