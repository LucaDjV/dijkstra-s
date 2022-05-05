using System;
using System.Collections.Generic;

namespace BinaryTreesMethods
{
    public class BinaryTreeWithJagged
    {
        public TreeNode root;
        public TreeNode[][] tempBinaryTree;
        public BinaryTreeWithJagged(int[] binTree)
        {
            tempBinaryTree = CreateTree(binTree);
            int level = GetDepth(binTree);
            for (int depth = 0; depth < level - 1; depth++)
            {
                for (int i = 0; i < tempBinaryTree[depth].Length; i++)
                {
                    AssignChildNodes(depth, i);
                }
            }
            DisplayTree(tempBinaryTree);
            root = tempBinaryTree[0][0];
        }
        public BinaryTreeWithJagged(TreeNode Root)
        {
            root = Root;
        }
        public int GetDepth(int[] binTree)
        {
            int level = 0;
            int lengthRemaing = binTree.Length - 1;
            while (lengthRemaing > 0)
            {
                for (int i = 0; i < Math.Pow(2, level); i++)
                {
                    lengthRemaing--;
                }
                level++;
            }
            return level;
        }
        public void DisplayTree(TreeNode[][] tempBinaryTree)
        {
            foreach (TreeNode[] a in tempBinaryTree)
            {
                for (int i = 0; i < a.Length; i++)
                {
                    Console.Write(" " + a[i].val);
                }
                Console.WriteLine();
            }
        }
        public void AssignChildNodes(int row, int col)
        {
            int nextRowCount = 2 * col;
            TreeNode leftNode = tempBinaryTree[row + 1][nextRowCount];
            TreeNode rightNode = tempBinaryTree[row + 1][nextRowCount + 1];

            tempBinaryTree[row][col].left = leftNode;
            /*Console.WriteLine("object refs equal = " + object.ReferenceEquals(leftNode, tempBinaryTree[row][col].left));*/
            tempBinaryTree[row][col].right = rightNode;
        }
        public TreeNode[][] CreateTree(int[] binTree)
        {
            TreeNode rootVal = new TreeNode(binTree[0]);
            TreeNode[] rootTemp = { rootVal }; // initialised an array of length 1 to store the root so we can add that the the final array
            int level = GetDepth(binTree); // getting the amount of levels of the binary tree
            TreeNode[][] tempBinaryTree = new TreeNode[level][]; // declaring a jagged array to store the levels with their nodes in order
            double pointer = 0;
            for (int levelTemp = 0; levelTemp < level; levelTemp++)
            {
                List<TreeNode> levelList = new List<TreeNode>();
                double i = 0;
                while (i < Math.Pow(2, levelTemp))
                {
                    try
                    {
                        TreeNode temp = new TreeNode(binTree[(int)(pointer + i)]);
                        levelList.Add(temp);

                        i++;
                    }
                    catch
                    {
                        TreeNode temp = new TreeNode(-1);
                        levelList.Add(temp);
                    }
                }
                pointer += Math.Pow(2, levelTemp);
                tempBinaryTree[levelTemp] = levelList.ToArray();
            }
            return tempBinaryTree;
        }
    }
    public class TreeNode
    {
        public int val;
        public TreeNode left;
        public TreeNode right;
        public TreeNode(int val = 0, TreeNode left = null, TreeNode right = null)
        {
            this.val = val;
            this.left = left;
            this.right = right;
        }
    }
    public class InvertBinTree
    {
        public static TreeNode InvertTree(TreeNode root)
        {
            if (root == null)
            {
                return root;
            }
            TreeNode left = InvertTree(root.left);
            TreeNode right = InvertTree(root.right);
            TreeNode temp = root.left;
            root.left = root.right;
            root.right = temp;
            return root;
        }
    } //perfection
    public class MaximumPath
    {
        public static int MaxPathSum(TreeNode root)
        {
            int maxPath = 0;
            List<TreeNode> vistited = new List<TreeNode>();
            List<int> pathVals = new List<int>();
            int pathCount = 0;
            dfs(root, vistited, pathVals, pathCount);
            foreach (var path in pathVals)
            {
                if (path > maxPath)
                {
                    maxPath = path;
                }
            }
            return maxPath;

        }
        /*        public static bool dfs(TreeNode root, List<TreeNode> visited, List<int> pathVals, int pathCount, bool backtracking)
                {
                    if (backtracking)
                    {
                        pathCount--;
                    }
                    if ((root.left != null) || (root.left != null))
                    {
                        if (!visited.Contains(root.left) && root.left != null)
                        {
                            visited.Add(root.left);
                            backtracking = dfs(root.left, visited, pathVals, pathCount, false);
                            if (!visited.Contains(root.right))
                            {
                                backtracking = dfs(root.right, visited, pathVals, pathCount + 1, false);
                            }
                            else
                            {
                                pathVals[pathCount] += root.val;
                                return true;
                            }
                        }
                        else if (!visited.Contains(root.right) && root.right != null)
                        {
                            visited.Add(root.right);
                            backtracking = dfs(root.right, visited, pathVals, pathCount, false);
                            if (!visited.Contains(root.left))
                            {
                                backtracking = dfs(root.left, visited, pathVals, pathCount + 1, false);
                            }
                            else
                            {
                                pathVals[pathCount] += root.val;
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else { return true; }
                }*/
        public static void dfs(TreeNode root, List<TreeNode> visited, List<int> pathVals, int pathCount)
        {
            if (pathCount == 0)
            {
                pathVals.Add(root.val);
                visited.Add(root);
            }
            else if (!visited.Contains(root))
            {
                if (pathCount < pathVals.Count - 1)
                {
                    pathVals.Insert(pathCount, pathVals[pathCount - 1] + root.val);
                    visited.Add(root);
                }
                else
                {
                    pathVals.Add(pathVals[pathCount - 1] + root.val);
                    visited.Add(root);
                }
            }
            if (root.right != null && !visited.Contains(root.right))
            {
                dfs(root.right, visited, pathVals, pathCount + 1);
                if (root.left != null && !visited.Contains(root.left))
                {
                    dfs(root.left, visited, pathVals, pathCount + 1);
                }
            }
            else if (root.left != null && !visited.Contains(root.left))
            {
                dfs(root.left, visited, pathVals, pathCount + 1);
                if (root.right != null && !visited.Contains(root.right))
                {
                    dfs(root.right, visited, pathVals, pathCount + 1);
                }
            }
            if (root.left != null && root.right != null)
            {
                if (pathCount > 0)
                {
                    if (pathVals[pathCount + 1] + pathVals[pathCount + 2] - pathVals[pathCount - 1] * 2 - root.val > pathVals[pathCount + 1])
                    {
                        pathVals.Add(pathVals[pathCount + 1] + pathVals[pathCount + 2] - pathVals[pathCount - 1] * 2 - root.val);
                    }
                }
                else
                {
                    if (pathVals[pathCount + 1] + pathVals[pathCount + 2] - root.val > pathVals[pathCount + 1])
                    {
                        pathVals.Add(pathVals[pathCount + 1] + pathVals[pathCount + 2]);
                    }
                }
            }
        } //doesnt work quite right. passed 25/94 tests on leetcode so works in a lot of cases...

        public static int MaxSum(TreeNode node, List<int> pathVals)
        {
            int[] temp = { MaxVoid(node.left, pathVals), MaxVoid(node.right, pathVals) };
            int totSubTree = temp[0] + temp[1];
            if (temp[0] > temp[1])
            {
                pathVals.Add(temp[0]);
            }
            else
            {
                pathVals.Add(temp[1]);
            }
            if(pathVals[pathVals.Count] < totSubTree)
            {
                pathVals.Add(totSubTree);
            }
        }
        public static void MaxVoid(TreeNode node, List<int> pathVals)
        {
            MaxVoid(node.left, pathVals);
            MaxVoid(node.right, pathVals);
            int temp = pathVals[pathVals.Count - 1];
            int totSubTree = temp[0] + temp[1];
            if (temp < totSubTree)
            {
                pathVals.Add(totSubTree);
            }
        }
    }  
}
