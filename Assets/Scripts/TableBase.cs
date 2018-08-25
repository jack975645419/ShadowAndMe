using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.ComponentModel;

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

    public virtual void AddOnEditor()
    {
        SomeTableRow S = new SomeTableRow();
        S.Id = Dict.Count;
        Dict.Add(S);
    }

    // Use this for initialization
    public virtual void Start () {
		
	}

    // Update is called once per frame
    public virtual void Update () {
		
	}
}
