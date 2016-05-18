using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// GameObject로 강제 형변환 할 필요가 있는지?

public class Path
{
    public Path Parent; // 서칭 시작 hex의 위치로 현재의 Path가 hex의 이웃임
    public Hex CurHex;

    public int F; // H + G = 비용
    public int G; // 시작점부터 현재까지의 거리값
    public int H; // 현재부터 도착점까지의 거리값

    public Path(Path parent, Hex hex, int g, int h)
    {
        Parent = parent;
        CurHex = hex;
        G = g;
        H = h;
        F = H + G;
    }
}

public class MapManager {

    private static MapManager inst = null;
    public GameObject GO_Hex; // todo : Unity에서 드래그로 설정한 프리팹. 나중에는 코드에서 적용해야 됨

    public float HexW;  // Awake에서 불러옴
    public float HexH;  // Awake에서 불러옴

    public int MapSizeX = 5;
    public int MapSizeY = 5;
    public int MapSizeZ = 5;

    public Point[] Dirs;

    Hex[][][] Map;

    public static MapManager GetInst()
    {
        if (inst == null)
        {
            inst = new MapManager();
            inst.Init();
        }

        return inst;
    }

    public void Init()
    {
        GO_Hex = (GameObject)Resources.Load("Prefabs/Map/Hex");

        SetHexSize();
        initDirs();
    }

    // 인접한 Hex의 Point 정보 저장
    public void initDirs()
    {
        Dirs = new Point[6]; // Hex는 인접한 것이 6개

        Dirs[0] = new Point(1, -1, 0);   // right
        Dirs[1] = new Point(1, 0, -1);   // up right
        Dirs[2] = new Point(0, 1, -1);   // up left
        Dirs[3] = new Point(-1, 1, 0);   // left
        Dirs[4] = new Point(-1, 0, 1);   // down left
        Dirs[5] = new Point(0, -1, 1);   // down right
    }

    public void SetHexSize()
    {
        HexW = GO_Hex.transform.GetComponent<Renderer>().bounds.size.x;
        HexH = GO_Hex.transform.GetComponent<Renderer>().bounds.size.z;
    }

    // 전역 좌표
    public Vector3 GetWorldPos(int x, int y, int z)
    {
        float X = 0.0f;
        float Z = 0.0f;

        X = x * HexW + (z * HexW * 0.5f);
        Z = (-z) * HexH * 0.75f;

        return new Vector3(X, 0, Z);
    }

    // 헥사곤 맵의 특징은 -를 이용한 좌표가 있어서 -부터 사이즈까지 루프를 돌아야 정상적으로 만들어짐
    // 기준점을 중심으로 밖으로 퍼져 나감
    public void CreateMap()
    {
        // 첫 번째 좌표에 대한 배열이 생성됨
        // +1은 xyz가 전부 0인 경우를 위해서 만듬
        Map = new Hex[MapSizeX * 2 + 1][][];
        GameObject map = new GameObject("Map");

        for (int x = -MapSizeX; x <= MapSizeX; x++)
        {
            // 배열에는 -가 들어갈 수 없기 때문에 + MapSizeX
            Map[x + MapSizeX] = new Hex[MapSizeY * 2 + 1][];

            for (int y = -MapSizeY; y <= MapSizeY; y++)
            {
                Map[x + MapSizeX][y + MapSizeY] = new Hex[MapSizeZ * 2 + 1];

                for (int z = -MapSizeZ; z <= MapSizeZ; z++)
                {
                    // 육각형 HexGrid는 x + y + z = 0
                    if (x + y + z == 0)
                    {
                        // Hex 클래스의 정보를 담아야하기 때문에 사용
                        // ((GameObject) Instantiate(GO_Hex)).GetComponent<Hex>();
                        // 인스턴트로 캐스팅 함
                        GameObject hex = (GameObject)GameObject.Instantiate(GO_Hex);
                        //hex.AddComponent<Hex>();

                        hex.transform.parent = map.transform;
                        //hex.GetComponentInParent<GameObject>().transform.parent = map.GetComponent<GameObject>().transform;

                        Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ] = hex.GetComponent<Hex>();

                        Vector3 pos = GetWorldPos(x, y, z);

                        Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].transform.position = pos;
                        Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].SetMapPos(x, y, z);
                    }
                }
            }
        }
    }

    // 완료
    // 플레이어의 Hex 정보를 구함
    public Hex GetPlayerHex(int x, int y, int z)
    {
        return Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ];
    }

    // 완료
    // 이동 거리 색칠
    public bool HighLightMoveRange(Hex start, int moveRange)
    {
        int highLightedCount = 0;

        for (int x = -MapSizeX; x <= MapSizeX; x++)
        {
            for (int y = -MapSizeY; y <= MapSizeY; y++)
            {
                for (int z = -MapSizeZ; z <= MapSizeZ; z++)
                {
                    if (x + y + z == 0 && Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].Passable == true)
                    {
                        int distance = GetDistance(start, Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ]);

                        if (distance <= moveRange && distance != 0)
                        {
                            if (isReachAble(start, Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ], moveRange))
                            {
                                if (Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].isExist == false)
                                {
                                    Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].transform.GetComponent<Renderer>().material.color = Color.green;
                                    highLightedCount++;
                                }
                            }
                        }
                    }
                }
            }
        }

        if (highLightedCount == 0)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    // 공격 범위 색칠
    // 빈 셀에는 공격이 안 되게 하려고 분리
    public bool HighLightAtkRange(Hex start, int atkRange)
    {
        PlayerManager pm = PlayerManager.GetInst();
        int highLightedCount = 0;

        for (int x = -MapSizeX; x <= MapSizeX; x++)
        {
            for (int y = -MapSizeY; y <= MapSizeY; y++)
            {
                for (int z = -MapSizeZ; z <= MapSizeZ; z++)
                {
                    if (x + y + z == 0 && Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].Passable == true)
                    {
                        int distance = GetDistance(start, Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ]);

                        if (distance <= atkRange && distance != 0)
                        {
                            bool isExist = false;

                            foreach (PlayerBase pb in pm.Players)
                            {
                                if (pb is AIPlayer)
                                {
                                    if (pb.CurHex.MapPos == Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].MapPos)
                                    {
                                        isExist = true;
                                        break;
                                    }
                                }
                            }

                            if (isExist == true)
                            {
                                // isReachAble가 무거운 함수다. 왜?
                                if (isReachAble(start, Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ], atkRange))
                                {
                                    if (Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].isExist == false)
                                    {
                                        Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].transform.GetComponent<Renderer>().material.color = Color.red;
                                        highLightedCount++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (highLightedCount == 0)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    // 완료
    // 이동하고 나면 Hilight 없앰
    public void ResetMapColor()
    {
        for (int x = -MapSizeX; x <= MapSizeX; x++)
        {
            for (int y = -MapSizeY; y <= MapSizeY; y++)
            {
                for (int z = -MapSizeZ; z <= MapSizeZ; z++)
                {
                    if (x + y + z == 0)
                    {
                        Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].transform.GetComponent<Renderer>().material.color = Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ].OriColor;
                    }
                }
            }
        }
    }

    public void ResetMapColor(Point pos)
    {
        Map[pos.X + MapSizeX][pos.Y + MapSizeY][pos.Z + MapSizeZ].transform.GetComponent<Renderer>().material.color = Map[pos.X + MapSizeX][pos.Y + MapSizeY][pos.Z + MapSizeZ].OriColor;
    }

    // 완료
    // 두 지점 사이의 거리를 구함
    public int GetDistance(Hex h1, Hex h2)
    {
        Point pos1 = h1.MapPos;
        Point pos2 = h2.MapPos;

        return (Mathf.Abs(pos1.X - pos2.X) + Mathf.Abs(pos1.Y - pos2.Y) + Mathf.Abs(pos1.Z - pos2.Z)) / 2;
    }

    List<Path> OpenList;
    List<Path> ClosedList;

    public bool isReachAble(Hex start, Hex dest, int moveRange)
    {
        List<Hex> path = GetPath(start, dest);

        if (path.Count == 0 || path.Count > moveRange)
        {
            return false;
        }

        return true;
    }

    // 완료
    public List<Hex> GetPath(Hex start, Hex dest)
    {
        OpenList = new List<Path>();
        ClosedList = new List<Path>();

        List<Hex> rtnVal = new List<Hex>();

        int H = GetDistance(start, dest);
        Path p = new Path(null, start, 0, H);

        ClosedList.Add(p);

        Path result = Recursive_FindPath(p, dest);

        if (result == null)
        {
            return rtnVal;
        }

        while (result.Parent != null)
        {
            rtnVal.Insert(0, result.CurHex); // 역순으로 들어감
            result = result.Parent;
        }

        return rtnVal;
    }

    // 완료
    // Recursive 반복되는
    // 반복적으로 호출
    public Path Recursive_FindPath(Path parent, Hex dest)
    {
        // 목적지를 찾음
        if (parent.CurHex.MapPos == dest.MapPos)
        {
            return parent; // 목적지를 찾은 경우
        }

        List<Hex> neibhors = GetNeibhors(parent.CurHex); // 현재 start Hex 기준으로 주변 Hex를 구함

        foreach (Hex h in neibhors)
        {
            Path newP = new Path(parent, h, parent.G + 1, GetDistance(h, dest));
            AddToOpenList(newP);
        }

        Path bestP;

        if (OpenList.Count == 0)
        {
            return null; // 목적지까지 길이 없는 경우
        }

        bestP = OpenList[0];

        foreach (Path p in OpenList)
        {
            if (p.F < bestP.F)
            {
                bestP = p;
            }
        }

        OpenList.Remove(bestP);
        ClosedList.Add(bestP);

        return Recursive_FindPath(bestP, dest);
    }

    // 완료
    // 값이 큰 값은 버리고 작은 값으로 들어감
    public void AddToOpenList(Path p)
    {
        foreach (Path inP2 in ClosedList)
        {
            if (p.CurHex.MapPos == inP2.CurHex.MapPos)
            {
                return;
            }
        }

        foreach (Path inP in OpenList)
        {
            if (p.CurHex.MapPos == inP.CurHex.MapPos)
            {
                if (p.F < inP.F)
                {
                    OpenList.Remove(inP);
                    OpenList.Add(p);
                    return;
                }
            }
        }

        OpenList.Add(p);
    }

    // 완료
    // 인접한 Hex를 구해오는 함수
    // 6개가 있는데 현재 위치를 기준으로 x, y, z의 좌표가 0과 1로 증감할 때 이 값들이
    // 이웃한 근접 Hex
    public List<Hex> GetNeibhors(Hex pos)
    {
        List<Hex> rtn = new List<Hex>();
        Point cur = pos.MapPos;

        if (pos.Passable == false)
        {
            return rtn;
        }

        foreach (Point p in Dirs)
        {
            Point tmp = p + cur;

            if (tmp.X + tmp.Y + tmp.Z == 0)
            {
                if (Mathf.Abs(tmp.X) <= MapSizeX)
                {
                    if (Mathf.Abs(tmp.Y) <= MapSizeY)
                    {
                        if (Mathf.Abs(tmp.Z) <= MapSizeZ)
                        {
                            rtn.Add(GetPlayerHex(tmp.X, tmp.Y, tmp.Z));
                        }
                    }
                }
            }
        }

        return rtn;
    }

    // 완료
    public Hex GetHex(int x, int y, int z)
    {
        return Map[x + MapSizeX][y + MapSizeY][z + MapSizeZ];
    }

    public void SetHexColor(Hex hex, Color color)
    {
        hex.transform.GetComponent<Renderer>().material.color = color;
    }
}
