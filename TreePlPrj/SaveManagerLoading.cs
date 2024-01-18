using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TreePlPrj
{
    internal partial class SaveManager
    {
        private static GoalsConnectionsSaveData LoadFromFile(string fileName)
        {
            GoalsConnectionsSaveData data = new GoalsConnectionsSaveData();
            if (File.Exists(fileName))
            {
                try
                {
                    using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                    {
                        var formatter = new BinaryFormatter();
                        data = (GoalsConnectionsSaveData)
                            formatter.Deserialize(stream);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return data;
        }
        public static void Load(string path, out List<GoalControl> loadedGoalControls, out List<Connection> loadedConnnections)
        {
            GoalsConnectionsSaveData loadedData = LoadFromFile(path);
            List<GoalControl> unpackedGoalControls;
            List<Connection> unpackedConnnections;
            Unpack(loadedData, out unpackedGoalControls, out unpackedConnnections);
            loadedGoalControls = unpackedGoalControls;
            loadedConnnections = unpackedConnnections;
        }
        private static void Unpack(GoalsConnectionsSaveData packedData, out List<GoalControl> unpackedGoalControls, out List<Connection> unpackedConnection)
        {
            unpackedGoalControls = UnpackGoals(packedData.goalList);
            unpackedConnection = UnpackConnetions(packedData.connectionsList, unpackedGoalControls);
        }
        private static List<GoalControl> UnpackGoals(List<PackedGoal> packedGoals)
        {
            List<GoalControl> unpackedGoals = new List<GoalControl>();
            foreach (var goal in packedGoals)
            {
                GoalControl unpackedGoal = GoalCreator.CreateGoalFromParams(goal.heading, goal.subgoals, goal.x, goal.y, goal.width, goal.height);
                unpackedGoals.Add(unpackedGoal);
            }
            Console.WriteLine(unpackedGoals.Count);
            return unpackedGoals;
        }
        private static List<Connection> UnpackConnetions(List<PackedConnection> packedConnetions, List<GoalControl> unpackedGoalControls)
        {
            List<Connection> unpackedConnections = new List<Connection>();

            foreach (var packedConnection in packedConnetions)
            {
                Connector start = unpackedGoalControls[packedConnection.startId].getConnector(packedConnection.starOrientation);
                Connector sink = unpackedGoalControls[packedConnection.sinkId].getConnector(packedConnection.sinkOrientation);
                Connection connection = new Connection(start);
                connection.Add_Sink(sink);
                unpackedConnections.Add(connection);
            }
            return unpackedConnections;
        }
    }
}
