﻿namespace DAA_VRP
{
    /// <summary>
    /// Abstract class to store solutions
    /// </summary>
    public abstract class Solution
    {
        public string problemId = "unspecified";
        public int numberOfClients = -1;
        public int totalDistance = -1;
        public long elapsedMilliseconds = -1;
        public List<List<int>> paths = new List<List<int>>();

        /// <summary>
        /// Returns a string representation of the paths
        /// </summary>
        /// <returns></returns>
        public string GetPathsString()
        {
            string output = "";
            for (int i = 0; i < paths.Count; i++)
            {
                foreach (int node in paths[i])
                {
                    output += node + ", ";
                }

                output += "}\n";
            }
            output = output.Substring(0, output.Length - 2);
            output += "}\n";
            return output;
        }

        public void SetPaths(List<List<int>> paths)
        {
            this.paths = new List<List<int>>();
            for (int i = 0; i < paths.Count; i++)
            {
                this.paths.Add(new List<int>(paths[i]));
            }
        }

        public virtual int GetRclSize() {
            return 1;
        }
    }
}
