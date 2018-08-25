using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TableRow_Touzhiwu:TableRowBase
{
    [Tooltip("投掷物类")]
    public GameObject Prefab_Ammu;

    [Tooltip("攻击力")]
    public float AttackValue;
}

public class Table_Touzhiwu : TableBase<TableRow_Touzhiwu> {
    


}
