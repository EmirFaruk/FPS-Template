using System.Collections.Generic;
using UnityEngine;

public class PathControl : MonoBehaviour
{
    private List<byte> pathSeries = new List<byte>();
    public List<byte> PathSeries { get; private set; }

    [SerializeField, Tooltip("The number of points in the series")]
    private byte pointsCount = 11;

    [SerializeField, Tooltip("The final point of the series")]
    private byte pathRange = 121;

    [SerializeField, Tooltip("territory for point groups")]
    private byte territoryCount = 5;

    private byte pathGroupCount;

    private byte currentPoint;
    public byte CurrentPoint => currentPoint;

    private void Start()
    {
        CreatePathSeries();
    }


    // CreatePathSeries method creates a series of points that will be used to create the path
    // The method creates a series of points that will be used to create the path
    public void CreatePathSeries()
    {
        pathGroupCount = ((byte)(pathRange / territoryCount));
        var pointsPerTerritory = pointsCount / territoryCount;
        var selectedTerritory = 0;

        while (pathSeries.Count < pointsCount)
        {
            //int currentTerritory = (pathRange * (pathSeries.Count % pointsPerTerritory)) / territoryCount;
            //currentTerritory = Mathf.Clamp(currentTerritory, 0, pathRange + 1);

            byte point = (byte)Random.Range(selectedTerritory, pathGroupCount + selectedTerritory);
            if (pathSeries.Count == pointsCount - 1) point = pathRange;

            if (!pathSeries.Contains(point))
            {
                pathSeries.Add(point);
            }

            //print("pointsPerTerritory : " + pathSeries.Count + " / " + pointsPerTerritory
            //      + " ~~ " + selectedTerritory + "-" + (selectedTerritory + pathGroupCount));

            if (pathSeries.Count % pointsPerTerritory == 0)
            {
                selectedTerritory += pathGroupCount;
                selectedTerritory = Mathf.Clamp(selectedTerritory, 0, pathRange - pathGroupCount + 1);

                pointsPerTerritory = Random.Range(1, (pointsCount / territoryCount) + 2) + pathSeries.Count;
            }
        }
        PathSeries = pathSeries;
        print("Path Series: " + string.Join(", ", PathSeries));
    }
}
