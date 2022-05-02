﻿namespace DAA_VRP
{
    public class GVNS
    {
        Problem problem;
        int numberOfNodes = -1;

        List<ILocalSearch> neighborhoodStructures = new List<ILocalSearch>{
            new MultiInsertion(),
            new SingleInsertion(),
            new MultiSwap(),
            new SingleSwap(),
            };

        public GVNS(Problem problem)
        {
            this.problem = problem;
            this.numberOfNodes = problem.numberOfClients;
        }

        private GvnsSolution Shaking(GvnsSolution solution, int currentNeighborIndex)
        {
            return (GvnsSolution)neighborhoodStructures[currentNeighborIndex].Shake(problem, (Solution)solution);
        }

        private GvnsSolution VND(GvnsSolution solution)
        {
            int currentNeighborIndex = 0;
            GvnsSolution bestSolution = new GvnsSolution(solution.problemId, solution.numberOfClients, solution.GetRclSize());
            bestSolution.SetPaths(solution.paths);
            bestSolution.rclSize = solution.GetRclSize();
            bestSolution.totalDistance = solution.totalDistance;


            GvnsSolution currentSolution = new GvnsSolution(solution.problemId, solution.numberOfClients, solution.GetRclSize());
            currentSolution.SetPaths(solution.paths);
            currentSolution.rclSize = solution.GetRclSize();
            currentSolution.totalDistance = solution.totalDistance;


            for (int i = 0; i <= currentNeighborIndex; i++)
            {
                currentSolution = (GvnsSolution)neighborhoodStructures[i].Search(problem, (Solution)currentSolution);
                if (currentSolution.totalDistance < bestSolution.totalDistance)
                {
                    bestSolution = currentSolution;
                    currentNeighborIndex = 0;
                }
            }

            return bestSolution;
        }

        private GvnsSolution GvnsConstructivePhase(int rclSize)
        {
            GreedyRCL greedy = new GreedyRCL(problem);

            GreedySolution greedySolution = greedy.Solve(rclSize);

            GvnsSolution solution = new GvnsSolution(problem.sourceFilename, numberOfNodes, rclSize);
            solution.SetPaths(greedySolution.paths);
            solution.totalDistance = greedySolution.totalDistance;
            return solution;
        }

        public GvnsSolution Solve(int rclSize)
        {
            GvnsSolution bestSolution = GvnsConstructivePhase(rclSize);

            int k = 0;
            for (int i = 0; i < 5000; i++)
            {
                GvnsSolution candidate = Shaking(bestSolution, k);
                if(candidate.totalDistance != CalculateDistance(candidate.paths))
                {
                    throw new Exception("Distance is not equal");
                }
                candidate = VND(candidate);
                if (candidate.totalDistance != CalculateDistance(candidate.paths))
                {
                    throw new Exception("Distance is not equal");
                }
                if (candidate.totalDistance < bestSolution.totalDistance)
                {
                    bestSolution = candidate;
                    k = 0;
                    i = 0;
                }
                else
                {
                    k++;
                    if (k >= neighborhoodStructures.Count)
                    {
                        k = 0;
                    }
                }
            }
            return bestSolution;
        }

        public int CalculateDistance(List<List<int>> paths)
        {
            int distance = 0;
            for (int i = 0; i < paths.Count; i++)
            {
                for (int j = 0; j < paths[i].Count - 1; j++)
                {
                    distance += problem.distanceMatrix[paths[i][j]][paths[i][j + 1]];
                }
            }
            return distance;
        }
    }
}
