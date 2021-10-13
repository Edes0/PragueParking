using System;

public class Vehichle
{
    private string regNum;
    private byte size;
    private DateTime parkTime;

    public Car()
    {

    }
    public MC()
    {

    }

    public Car(string regNum, DateTime parkTime)
    {
        this.regNum = regNum;
        this.size = 2;
        this.parkTime = parkTime;
    }
    public MC(string regNum, DateTime parkTime)
    {
        this.regNum = regNum;
        this.size = 1;
        this.parkTime = parkTime;
    }
    public string RegNum
    {
        get { return this.regNum; }
        set { this.regNum = value; }
    }
    public DateTime ParkTime
    {
        get { return this.parkTime; }
        set { this.parkTime = value; }
    }
}
