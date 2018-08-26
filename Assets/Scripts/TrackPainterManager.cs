using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackPainterManager : Singleton<TrackPainterManager> {

    public const int NumOfLinesForTracks = 10;
    public LineRenderer[] LinesForTracks = new LineRenderer[NumOfLinesForTracks];

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        for(int k = 0; k< NumOfLinesForTracks; k++)
        {
            Logger.Instance.Assert(LinesForTracks[k] != null);
        }
    }

    public void ReconstructAllLines(Msg msg = null)
    {
        var table = GameManager.Instance.m_TablePaowuxian;
        for(int k = 0; k<table.GetCount(); k++)
        {
            var tr = table.GetValue(k);
            Vector3[] _v = tr.GetPointsByX(-0.2f, 1, GameManager.Instance.PrecisionOfDebugPaintTrack);
            _v = MTool.NormalizedToScreen(_v);
            Logger.Instance.DrawLine(LinesForTracks[k], _v);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
