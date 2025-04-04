using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadTile
{
    public byte expectResLow, expectResMid, expectResHi, expectRetail;
    public short demandRetail;
    public byte retailCov;
    public int trafficVolume;//actual
    public int trafficCapacity;//maxium
    public byte noiseLevel, airPolLevel;
    public byte congetion;
    public byte policeCov, healthcareCov, fireCov, EduICov, EduIICov, amusementCov, parkCov;//Cov = coverage, Edu = Education
	public bool RC,LC,UC,DC;//R=right L=left U=up D=down c=Connection
}
