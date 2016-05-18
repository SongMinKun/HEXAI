using UnityEngine;
using System.Collections;

// 완료
public class Point
{
    public int X;
    public int Y;
    public int Z;


    public Point(int x, int y, int z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public override string ToString()
    {
        return "x : " + X + " y : " + Y + " z : " + Z;
    }

    public static Point operator + (Point p1, Point p2)
    {
        return new Point(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
    }

    public static bool operator == (Point p1, Point p2)
    {
        return (p1.X == p2.X && p1.Y == p2.Y && p1.Z == p2.Z);
    }

    public static bool operator != (Point p1, Point p2)
    {
        return (p1.X != p2.X || p1.Y != p2.Y || p1.Z != p2.Z);
    }
}

// 완료
// todo : isExist 없애자
public class Hex : MonoBehaviour {

    public Point MapPos;
    public bool isExist = false;
    public bool Passable = true;
    public Color OriColor = Color.white;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetMapPos(Point pos)
    {
        MapPos = pos;
    }

    public void SetMapPos(int x, int y, int z)
    {
        MapPos = new Point(x, y, z);
    }

    void OnMouseDown()
    {
        PlayerManager pm = PlayerManager.GetInst();
        PlayerBase pb = pm.Players[pm.CurTurnIdx];

        Debug.Log(MapPos + " OnMouseDown");

        if (pb.act == ACT.IDLE)
        {
            if (Passable == true)
            {
                transform.GetComponent<Renderer>().material.color = Color.yellow;
                OriColor = Color.yellow;
                Passable = false;

                c = OriColor;
            }

            else
            {
                transform.GetComponent<Renderer>().material.color = Color.white;
                OriColor = Color.white;
                Passable = true;

                c = OriColor;
            }
        }

        else if(pb.act == ACT.MOVEHIGHLIGHT) {
            if (Passable == true)
            {
                OriColor = Color.white;
                c = OriColor;
            }
        }

        pm.MovePlayer(pm.Players[pm.CurTurnIdx].CurHex , this); 
    }

    public Color c;

    void OnMouseEnter()
    {
        c = transform.GetComponent<Renderer>().material.color;
        transform.GetComponent<Renderer>().material.color = Color.cyan;
    }

    void OnMouseExit()
    {
        if (transform.GetComponent<Renderer>().material.color != Color.yellow)
        {
            transform.GetComponent<Renderer>().material.color = c;
        }
	}
}
