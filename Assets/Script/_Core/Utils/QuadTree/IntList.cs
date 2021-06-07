using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntList
{
    int[] data = new int[128];
    int numFields;
    int num;
    int cap = 128;
    int freeElement = -1;

    public int Count => num;


    public IntList(int startNumFields)
    {
        this.numFields = startNumFields;
    }

    public int Get(int n, int field)
    {
        return data[n * numFields + field];
    }


    public void Set(int n, int field, int value)
    {
        data[n * numFields + field] = value;
    }


    public void Clear()
    {
        num = 0;
        freeElement = -1;
    }


    public int PushBack()
    {
        int newPos = (num + 1) * numFields;

        if(newPos > cap)
        {
            int newCap = newPos * 2;

            int[] newArray = new int[newCap];

            System.Array.Copy(data, newArray, cap);
            data = newArray;

            cap = newCap;
        }
        return num++;
    }


    public void PopBack()
    {
        --num;
    }


    public int Insert()
    {
        if(freeElement != -1)
        {
            int index = freeElement;
            int pos = index * numFields;


            freeElement = data[pos];

            return index;
        }
        return PushBack();
    }


    public void Erase(int n)
    {
        int pos = n * numFields;
        data[pos] = freeElement;
        freeElement = n;
    }
}
