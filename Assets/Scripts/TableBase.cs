using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.ComponentModel;

[System.Serializable]
public class TableRowBase
{
    [ReadOnly(true)]
    public int Id;

    public TableRowBase()
    {
    }

    public TableRowBase(int id)
    {
        Id = id;
    }
}


public class TableBase<SomeTableRow> : MonoBehaviour where SomeTableRow: TableRowBase, new ()
{

    [SerializeField]
    public List<SomeTableRow> Dict = new List<SomeTableRow>();
    public int GetCount()
    {
        return Dict.Count;
    }

    public SomeTableRow GetValue(int Id)
    {
        if(Dict.Count>Id && Id>=0 && Dict[Id].Id == Id)
        return Dict[Id];
        return null;
    }

    public virtual void AddOnEditor()
    {
        SomeTableRow S = new SomeTableRow();
        Dict.Add(S);
        RefreshOnEditor();
    }
    public virtual void RefreshOnEditor()
    {
        for (int k = 0; k < Dict.Count; k++)
        {
            Dict[k].Id = k;
        }

    }

    // Use this for initialization
    public virtual void Start () {
        RefreshOnEditor();
	}

    // Update is called once per frame
    public virtual void Update () {
		
	}
}
