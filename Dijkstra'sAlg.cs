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
            Queue<label> myQueue = forwardPass(adjacencyMat);
            PrintQueue(myQueue, adjacencyMat);
            Console.ReadKey();
        }
        static void PrintQueue(Queue<label> queue, int[,] adjacencyMat)
        {
            for (int i = 0; i < queue.Count; i++)
            {
                label temp = queue.Dequeue();
                Console.WriteLine(temp.node + "has distance: " + temp.tempDist);
            }
        }
        static Dictionary<int, int> CreateDict(int[,] adjacencyMat)
        {
            Dictionary<int, int> tempLabels = new Dictionary<int, int>(); //instatiate a dictionary of labels to store the labels that have not been chosen yet
            for (int i = 0; i < dim; i++)
            {
                tempLabels.Add(i + 1, adjacencyMat[0, i]);
            }//adds all of the nodes to the dictionary
            tempLabels.Remove(0);
            return tempLabels;
        }
        static void checkVal(ref int temp, ref int tempNode, int[,] adjacencyMat, Dictionary<int, int> tempLabels, List<int> layersToCheck, int i, int j)
        {
            if (adjacencyMat[i, j] != 0) //ensures the edge is not null
            {
                if (tempLabels.ContainsKey(j))// ensures the label is contained in the dict before calling it
                {
                    /*Console.WriteLine("\t" + tempLabels[j]);*/
                    if (adjacencyMat[layersToCheck[i], j] < tempLabels[j])//checks if the weight at (current layer , current node) is less than the temporary label for the current node
                    {
                        tempLabels[j] = adjacencyMat[layersToCheck[i], j]; //if the weight is less, it changs the temporary label's weight to it
                    }
                    if (tempLabels[j] <= temp) //now that we have the new distance we can check if its the minimum edge connected to the node now rather than waiting until after
                    {
                        tempNode = j + 1;
                        temp = tempLabels[j]; //if it is the minimum, the minimum edge is changed to it
                    }
                }
            }
        }
        static Queue<label> forwardPass(int[,] adjacencyMat) //take in an adjacency matrix representing the graph
        {
            Queue<label> labels = new Queue<label>(); //initialise a queue of labels to store the labels in the order in which they are added
            label startNode = new label(1, 0); labels.Enqueue(startNode);//enque the start node for ease
            Dictionary<int, int> tempLabels = CreateDict(adjacencyMat);
            List<int> layersToCheck = new List<int>();
            layersToCheck.Add(0);
            int counter = tempLabels.Count;
            while (counter > 1) //loops until the amount of labels left to add to the list is 0
            {
                int temp = int.MaxValue; //making a maximum
                int tempNode = 0; //giving the maximum a node
                bool throughLoop = false;
                for (int i = 0; i < layersToCheck.Count; i++) //loops through all of the layers to check that increases with each new node added
                {
                    for (int j = 0; j < dim; j++) //loops through all of the values in the layer
                    {
                        checkVal(ref temp, ref tempNode, adjacencyMat, tempLabels, layersToCheck, i, j); //abstracting checking to a function
                    }
                    throughLoop = true;
                }
                if (throughLoop)
                {
                    Console.WriteLine();
                    layersToCheck.Add(tempNode);
                    label finalTemp = new label(tempNode, temp);
                    labels.Enqueue(finalTemp);
                    /*Console.WriteLine("labels dict: ");
                    foreach (var l in tempLabels)
                    {
                        Console.WriteLine("node: " + l.Key + " dist: " + l.Value);
                    }*/
                    tempLabels.Remove(tempNode);
                    for (int i = 0; i < dim; i++)
                    {
                        adjacencyMat[i, tempNode] = int.MaxValue;
                    }
                }
                counter--;
            }
            return labels;
        }
    }
    class label
    {
        public int node;
        public int tempDist;
        public label(int nodeIn, int tempDistIn)
        {
            node = nodeIn;
            tempDist = tempDistIn;
        }
    }
    class PriorityQueue
    {
        struct priorityInt //structure for the items
        {
            public int Value;
            public int Priority;
            public priorityInt(int ValueIn, int PriorityIn)
            {
                Value = ValueIn;
                Priority = PriorityIn;
            }
        }
        int rear;
        int front;
        public int length;
        priorityInt[] contents; //array of structures to store items
        int itemCount;
        public PriorityQueue(int size)
        {
            contents = new priorityInt[size];
            rear = 0;
            front = 0;
            length = size;
            itemCount = 0;
        }
        public void Enqueue(int input, int priority)
        {
            rear++;
            itemCount++;
            contents[rear] = new priorityInt(input, priority);
            int numsToChange = contents.Length;
            while (numsToChange > 1)
            {
                for (int i = numsToChange; i < 0; i--)
                {
                    if (contents[i + 1].Priority > contents[i].Priority)
                    {
                        int temp = contents[i + 1].Priority;
                        int tempVal = contents[i + 1].Value;
                        contents[i + 1].Priority = contents[i].Priority;
                        contents[i + 1].Value = contents[i].Value;
                        contents[i].Priority = temp;
                        contents[i].Value = tempVal;
                        Console.WriteLine(contents[i + 1] + " " + temp);
                    }
                }
                numsToChange--;
            }
        }
    }

}
