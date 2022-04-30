using System;
using System.Collections.Generic;

namespace djikstraAlg
{
    class Program
    {
        public static int dim; //caching array dimensions
        static void Main(string[] args)
        {
            int[,] adjacencyMat = {
                { 0, 6, 8, 10, 0},
                { 6, 0, 5, 0, 10},
                { 8, 5, 0, 5, 9},
                { 10, 0, 5, 0, 6},
                { 0, 10, 9, 6, 0},
            }; //initialise an adjacency matrix to represent a graph
            dim = adjacencyMat.GetLength(0);
            Dictionary<char, int> distances = dijkstra.forwardPass(adjacencyMat);
            foreach(var s in distances)
            {
                Console.WriteLine("node " + s.Key + " has distance from A of " + s.Value);
            }
            Console.ReadKey();
        }
    }
    class dijkstra
    {
        public static Dictionary<char, int> forwardPass(int[,] adjacencyMat)
        {
            //creates a priority queue to store the distance
            //values of each node in order while they are
            //still in the process of being found
            PriorityQueue labels = new PriorityQueue(adjacencyMat.GetLength(0)+10);

            //creates a dictionary to pass the node and
            //distance values to return
            Dictionary<char, int> result = new Dictionary<char,int>();

            //creates a list of visited nodes
            List<int> visited = new List<int> ();

            //enqueues the 1st node
            labels.Enqueue(1, 0);

            //loops until there are no more nodes to check
            while(labels.GetLength() > 0)
            {
                //dequeues the item with the shortest distance and
                //adds it to the visited list and distance dictionary
                priorityItem temp = labels.Dequeue();
                visited.Add(temp.Value);
                //changes the integer value to a UTF capital letter
                char key = (char)(65 + temp.Value);
                result.Add(key, temp.Priority);

                //loops through the child nodes of the node we just dequeued
                for(int i = 0; i < adjacencyMat.GetLength(1); i++)
                {
                    //conditional to check if there is a connection and
                    //if the minimum distance has not already been found
                    if(adjacencyMat[temp.Value, i] != 0 && !visited.Contains(i))
                    {
                        //checking if the node is already in the label queue
                        if (labels.Contains(i))
                        {
                            //if the new distance is less than the old one
                            if(temp.Priority + adjacencyMat[temp.Value, i] < labels.GetPriority(i))
                            {
                                //changing the distance to the new, shorter distance
                                labels.changePriority(i, temp.Priority + adjacencyMat[temp.Value, i]);
                            }
                        }
                        else
                        {
                            //adding the new node and distance to the queue
                            labels.Enqueue(i, temp.Priority + adjacencyMat[temp.Value, i]);
                        }
                    }
                }
            }
            return result;
        }
    }
    public struct priorityItem //structure for the items
    {
        public int Value;
        public int Priority;
        public priorityItem(int ValueIn, int PriorityIn)
        {
            Value = ValueIn;
            Priority = PriorityIn;
        }
    }
    public class PriorityQueue
    {
        int rear;
        int front;
        priorityItem[] contents; //array of structures to store items
        int itemCount;
        public PriorityQueue(int size)
        {
            contents = new priorityItem[size];
            rear = -1;
            front = 0;
            itemCount = 0;
        }
        public int GetPriority(int item)
        {
            return contents[item].Priority;
        }
        public void changePriority(int item, int newPriority)
        {
            contents[item].Priority = newPriority;
            int numsToChange = itemCount;
            while (numsToChange > 0)
            {
                for (int i = front + numsToChange - 1; i > 0; i--)
                {
                    if (contents[i].Priority < contents[i - 1].Priority)
                    {
                        priorityItem temp = contents[i];
                        contents[i] = contents[i - 1];
                        contents[i - 1] = temp;
                    }
                }
                numsToChange--;
            }
        }
        public bool Contains(int item)
        {
            for(int i = front; i <= rear; i++)
            {
                if(contents[i].Value == item)
                {
                    return true;
                }
            }
            return false;
        }
        public priorityItem Peek()
        {
            if (itemCount == 0) { throw new Exception("no items in queue"); }
            return contents[front];
        }
        public int GetLength()
        {
            return itemCount;
        }
        public void Enqueue(int input, int priority)
        {
            rear++;
            itemCount++;
            contents[rear] = new priorityItem(input, priority);
            int numsToChange = itemCount;
            while (numsToChange > 0)
            {
                for (int i = front + numsToChange - 1; i > 0; i--)
                {
                    if (contents[i].Priority < contents[i - 1].Priority)
                    {
                        priorityItem temp = contents[i];
                        contents[i] = contents[i - 1];
                        contents[i - 1] = temp;
                    }
                }
                numsToChange--;
            }
        }
        public priorityItem Dequeue()
        {
            if (itemCount == 0) { throw new Exception("no items in queue"); }
            front++;
            itemCount--;
            return contents[front - 1];
        }
    }
}
