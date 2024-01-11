using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class GameEngine : MonoBehaviour
{
    // Start is called before the first frame update

    static Vector2 convert1Dto2D(int x) {
        return new Vector2(x / 20, x % 20);
    }
    static int convert2Dto1D(int row,int col)
    {
        return row*20 + col;
    }

    static int[] dx = { 1, 1, -1, -1, 1, -1, 0, 0 };
    static int[] dy = { -1, 1, 1, -1, 0, 0, 1, -1 };
    static bool IsValidIndex(int row, int col)
    {
        return (row >= 0 && col >= 0 && row < 20 && col < 20);
    }
    public GameObject[] MapEvents;

    public class DSU {
        public class Node
        {
            public bool isAlignLeft = false;
            public bool isAlignRight = false;
        }
        public Node[] data;
        public int[] parent;

        public DSU(int n = 400)
        {
            data = new Node[n];
            parent = new int[n];
            for (int i = 0; i < n; ++i)
            {
                parent[i] = i;
                data[i] = new Node();
            }


        }
        public int Find(int x)
        {
            if (parent[x] != x)
            {
                parent[x] = Find(parent[x]);
            }
            data[parent[x]].isAlignLeft |= data[x].isAlignLeft;
            data[parent[x]].isAlignRight |= data[x].isAlignRight;

            return parent[x];
        }
        public void Union(int x, int y)
        {
            int rootX = Find(x);
            int rootY = Find(y);
            if (rootX == rootY)
            {
                return;
            }

            data[rootX].isAlignLeft = data[rootX].isAlignLeft | data[rootY].isAlignLeft;
            data[rootX].isAlignRight = data[rootX].isAlignRight | data[rootY].isAlignRight;

            data[rootY].isAlignLeft = data[rootX].isAlignLeft;
            data[rootY].isAlignRight = data[rootX].isAlignRight;


            parent[rootX] = rootY;

        }
        public bool isUnion(int x, int y)
        {
            int rootX = Find(x);
            int rootY = Find(y);

            if ((data[rootX].isAlignLeft && data[rootY].isAlignRight) || (data[rootY].isAlignLeft && data[rootX].isAlignRight))
            {
                return false;
            }
            return true;

        }
        public bool isUnionALL(int row, int col, GameObject[] MapEvents)
        {

            int index = convert2Dto1D(row, col);
            int root = Find(index);
            bool isAlignL = data[root].isAlignLeft, isAlignR = data[root].isAlignRight;
      
            for (int i = 0; i < 8; ++i)
            {
                int u = row + dx[i];
                int v = col + dy[i];

                if (u >= 0 && v >= 0 && v < 20 && u < 20)
                {
                    index = convert2Dto1D(u, v);
                    MapEventHandler mehl = MapEvents[index].GetComponent<MapEventHandler>();
                    if (!mehl.GetCanHover() || mehl.heroObj)
                    {
                        root = Find(index);
                        isAlignL |= data[root].isAlignLeft;
                        isAlignR |= data[root].isAlignRight;

                    }
                }

            }
            return isAlignL & isAlignR;
        }
        public void UnionALL(int row,int col, GameObject[] MapEvents)
        {
       
            for (int i = 0; i < 8; ++i)
            {
                int u = row + dx[i];
                int v = col + dy[i];

                if (u >= 0 && v >= 0 && v < 20 && u < 20) { 
                    int index = convert2Dto1D(u, v);
                    MapEventHandler mehl = MapEvents[index].GetComponent<MapEventHandler>();
                    if (!mehl.GetCanHover() || mehl.heroObj)
                    {
                        Union(index, convert2Dto1D(row, col));
                    }

                }
            }

        }
        public void BuildFromScratch(GameObject[] MapEvents,int n=400)
        {
            for (int i = 0; i < n; i++)
            {
                
                parent[i] = i;
         
                data[i].isAlignLeft =false;
                data[i].isAlignRight = false ;
                
                if (MapEvents[i].GetComponent<MapEventHandler>().isAlignRight) data[i].isAlignRight = true;
                if (MapEvents[i].GetComponent<MapEventHandler>().isAlignLeft) data[i].isAlignLeft = true;
        
            }

            for (int row = 0; row < 20; row++)
            {
                for (int col = 0; col < 20; ++col)
                {
                    int index = convert2Dto1D(row, col);
                    MapEventHandler mehl = MapEvents[index].GetComponent<MapEventHandler>();
                    if (! mehl.GetCanHover() || mehl.heroObj)
                    {
                        //Debug.Log("DSU" + row + " " + col + index);

                        UnionALL(row, col, MapEvents);
                    }

                }
            }

        }

    }
    public class BFS {
        Queue queue = new Queue();
        public GameObject[] Next;
        public BFS(GameObject[] MapEvents,int n=400)
        {
            Next = new GameObject[n];
            for (int i = 0; i < 400; ++i) Next[i] = null;
            for (int i = 7; i < 7 + 6; ++i)
            {
               
                int index = convert2Dto1D(19, i);
                queue.Enqueue(MapEvents[index]);
                Next[index] = MapEvents[index];
            }
            
        }
        
        public void BuildFromScratch(GameObject[] MapEvents, int n = 400)
        {
            for (int i = 0; i < 400; ++i) Next[i] = null;
            queue.Clear();
            for (int i = 7; i < 7 + 6; ++i)
            {
                int index = convert2Dto1D(19, i);
                queue.Enqueue(MapEvents[index]);
                Next[index] = MapEvents[index];
            }
        }
        public void PerformBFS(GameObject[] MapEvents)
        {
            int[] dxx = { -1,1, 0, 0};
            int[] dyy = { 0,0, 1, -1};
            while (queue.Count > 0)
            {
                GameObject currentNode = (GameObject) queue.Dequeue();

                MapEventHandler mehl = currentNode.GetComponent<MapEventHandler>();

                int row = mehl.row;
                int col = mehl.col;
               
                for (int i = 0; i < 4; ++i)
                {
                    int u = row + dxx[i];
                    int v = col + dyy[i];

                    if (IsValidIndex(u,v))
                    {
                        int index = convert2Dto1D(u, v);
                        MapEventHandler meh = MapEvents[index].GetComponent<MapEventHandler>();
                        if (meh.GetCanHover() && !meh.heroObj && !Next[index])
                        {
                            Next[index] = currentNode;
                            queue.Enqueue(MapEvents[index]);
                            //Debug.Log(row + " " + col + " " + u + " " + v);

                        }
                    }

                }

            }
        }
    }



    public static GameEngine Ins;
    public DSU dsu;
    public BFS bfs;
    private System.Random random =new System.Random();
    public void UnionALL(int row, int col)
    {
        dsu.UnionALL(row, col, MapEvents);
        //Debug.Log(dsu.data[convert2Dto1D(row, col)].isAlignLeft + " " + dsu.data[convert2Dto1D(row, col)].isAlignRight);
    }
    public bool isUnion(int row,int col)
    {
        //Debug.Log(dsu.isUnionALL(row, col, MapEvents));
        return !dsu.isUnionALL(row, col,MapEvents);
    }
    public void InitialGrid()
    {
        MapEvents = Create2DGridMapEvents.Ins.MapEvents;
        for (int i = 0; i < 7; i++) // col 0->6 ,row=0, 19  align left
        {
            int indexTop = convert2Dto1D(0, i);
            int indexBottom = convert2Dto1D(19, i);
            //MapEvents[indexTop].GetComponent<MapEventHandler>().SetTestHover(false);
            //MapEvents[indexBottom].GetComponent<MapEventHandler>().SetTestHover(false);

            MapEvents[indexTop].GetComponent<MapEventHandler>().isAlignLeft=true;
            MapEvents[indexBottom].GetComponent<MapEventHandler>().isAlignLeft = true;


            indexTop = convert2Dto1D(0, 19-i);
            indexBottom = convert2Dto1D(19, 19-i);
            //MapEvents[indexTop].GetComponent<MapEventHandler>().SetTestHover(false);
            //MapEvents[indexBottom].GetComponent<MapEventHandler>().SetTestHover(false);

            MapEvents[indexTop].GetComponent<MapEventHandler>().isAlignRight = true;
            MapEvents[indexBottom].GetComponent<MapEventHandler>().isAlignRight = true;

        }

        for (int i = 0; i < 20; i++) // col 0 ,row=0, 19  align left
        {
            int indexLeft = convert2Dto1D(i, 0);
            int indexRight = convert2Dto1D(i, 19);
            //MapEvents[indexLeft].GetComponent<MapEventHandler>().SetTestHover(false);
            //MapEvents[indexRight].GetComponent<MapEventHandler>().SetTestHover(false);

            MapEvents[indexLeft].GetComponent<MapEventHandler>().isAlignLeft = true;
            MapEvents[indexRight].GetComponent<MapEventHandler>().isAlignRight = true;
        }
        for (int row=0; row < 20; row++) {
            for (int col = 0; col < 20; ++col)
            {
                int index = convert2Dto1D(row, col);
                MapEvents[index].GetComponent<MapEventHandler>().row = row;
                MapEvents[index].GetComponent<MapEventHandler>().col = col;


            }
        }
        dsu = new DSU(400);
        dsu.BuildFromScratch(MapEvents);
        bfs=new BFS(MapEvents);
    }

    public void BFSPath()
    {
        bfs.BuildFromScratch(MapEvents);
        bfs.PerformBFS(MapEvents);
    }
    public void DSUReset()
    {
        dsu.BuildFromScratch(MapEvents);
    }
    private float stepChangePath=3.0f;
    public GameObject NextPath(int row,int col, float step,int cnt=0)
    {
        int index=convert2Dto1D(row, col);
        if (step > stepChangePath ) return bfs.Next[index];

        int[] dxx = { 1, 0, 0 ,-1 };
        int[] dyy = { 0, 1, -1, 0 };

        int counter = 0;
        while (true)
        {
            //60% 17% 17% 6%
            int num = 0;

            double randomNumber = random.NextDouble();
            if (randomNumber <= 0.6)
            {
                num = cnt%3;
            }    
            else if (randomNumber <= 0.6+0.17)
            {
                num = (cnt+1)%3;
            }
            else if(randomNumber <= 1-0.17)
            {
                num = (cnt+2)%3;
            }
            else
            {
                num = 3;
            }
            if (cnt % 3 == 1 || cnt % 3 == 2)
            {
                if (num != cnt % 3)
                {
                    counter++;
                    if (num == 1 || num == 2) 
                        if (counter<2) continue;
                    
                }
            }

            int u = row+ dxx[num];
            int v= col+ dyy[num]; 
            if (IsValidIndex(u, v))
            {
                index = convert2Dto1D(u, v);
                MapEventHandler meh = MapEvents[index].GetComponent<MapEventHandler>();
                if (meh.GetCanHover() && !meh.heroObj)
                {
                    return MapEvents[index];
                }
            }
        }


    }
    private void Awake()
    {
       
        Ins = this;
    }
    
}
