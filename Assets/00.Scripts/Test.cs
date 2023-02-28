using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Data
{
    public int a;

    public Data(int k)
    {
        a = k;
    }
}

public class Test : MonoBehaviour
{
    List<Data> list = new List<Data>();

    // Start is called before the first frame update
    void Start()
    {
        Data a = new Data(1);
        list.Add(a);

        a.a = 2;

        Debug.Log(list[0].a);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
