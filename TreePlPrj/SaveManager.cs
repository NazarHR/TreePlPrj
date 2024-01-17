using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml;
using static TreePlPrj.Connector;

namespace TreePlPrj
{
    internal class SaveManager
    {
        [Serializable]
        struct PackedGoal
        {
            public double x;
            public double y;
            public double width;
            public double height;
            public int id;
            public string heading;
            public List<Tuple<string, bool>> subgoals;
        }
        [Serializable]
        struct PackedConnection
        {
            public int startId;
            public ConnectorOrientation starOrientation;
            public int sinkId;
            public ConnectorOrientation sinkOrientation;
        }
        [Serializable]
        struct GoalsConnectionsSaveData
        {
            public List<PackedGoal> goalList;
            public List<PackedConnection> connectionsList;

        }

        private static void GoalConnectionSplit(UIElementCollection canvaElements, out List<GoalControl> goalControls, out List<Connection> connections)
        {
            List<GoalControl> _goalControls = new List<GoalControl>();
            List<Connection> _connections = new List<Connection>();
            foreach (UIElement item in canvaElements)
            {
                if (item is GoalControl)
                {
                    _goalControls.Add(item as GoalControl);
                }
                else if (item is Connection)
                {
                    _connections.Add(item as Connection);
                }
            }
            goalControls = _goalControls;
            connections = _connections;
        }
        private static PackedGoal Pack(GoalControl goalControl, int id)
        {
            Point point = goalControl.getPosition();
            double x = point.X;
            double y = point.Y;
            double width = goalControl.DesiredSize.Width;
            double height = goalControl.DesiredSize.Height;
            string heading = goalControl.getHeading();
            List<Tuple<string, bool>> subgoals = goalControl.getGoals();
            PackedGoal packedGoal = new PackedGoal() { x = x, y = y, width = width, height = height, id = id, heading = heading, subgoals = subgoals };
            return packedGoal;
        }
        private static PackedConnection Pack(Connection connection, List<GoalControl> goalControls)
        {
            Connector start = connection.getStart();
            ConnectorOrientation startOrientation = start.Orientation;
            int startId = goalControls.IndexOf(start.ParentGoal);

            Connector sink = connection.getSink();
            ConnectorOrientation sinkOrientation = connection.getSink().Orientation;
            int sinkId = goalControls.IndexOf(sink.ParentGoal);

            PackedConnection packedConnection = new PackedConnection() { startId = startId, starOrientation = startOrientation, sinkId = sinkId, sinkOrientation = sinkOrientation };
            return packedConnection;
        }
        private static void SaveToFile(string fileName, GoalsConnectionsSaveData data)
        {
            try
            {
                using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, data);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public static void save(UIElementCollection canvaElements, string filename)
        {
            List<GoalControl> _goalControls = new List<GoalControl>();
            List<Connection> _connections = new List<Connection>();
            GoalConnectionSplit(canvaElements, out _goalControls, out _connections);
            List<PackedGoal> packedGoals = new List<PackedGoal>();
            for (int i = 0; i < _goalControls.Count; i++)
            {
                packedGoals.Add(Pack(_goalControls[i], i));
            }
            List<PackedConnection> packedConnections = new List<PackedConnection>();
            foreach (Connection connection in _connections)
            {
                packedConnections.Add(Pack(connection, _goalControls));
            }
            GoalsConnectionsSaveData goalsConnectionsSaveData = new GoalsConnectionsSaveData() { goalList = packedGoals, connectionsList = packedConnections };
            SaveToFile(filename, goalsConnectionsSaveData);
        }
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
            path = " C:\\Users\\nazar\\Desktop\\plan.plan";
            GoalsConnectionsSaveData loadedData = LoadFromFile(path);
            List<GoalControl>unpackedGoalControls;
            List<Connection> unpackedConnnections;
            Unpack(loadedData, out unpackedGoalControls, out unpackedConnnections);
            loadedGoalControls=unpackedGoalControls;
            loadedConnnections=unpackedConnnections;
            //Console.WriteLine( "here");
            //printPakedGoals(loadedData.goalList);
            //PrintPakedConnections(loadedData.connectionsList);
        }
        private static void Unpack(GoalsConnectionsSaveData packedData, out List<GoalControl> unpackedGoalControls, out List<Connection> unpackedConnection )
        {
            //Unpacking Goals
            unpackedGoalControls = UnpackGoals(packedData.goalList);
            //unpacking Connetions
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
        private static void printPakedGoals(List<PackedGoal> packedGoals)
        {
            foreach (PackedGoal goal in packedGoals)
            {
                Console.WriteLine("{0} {1} ", goal.x, goal.y);
                Console.WriteLine("{0} {1}", goal.width, goal.heading);
                Console.WriteLine("{0} {1}", goal.heading, goal.id);
                foreach (var item in goal.subgoals)
                {
                    Console.Write(" {0} {1} ", item.Item1, item.Item2);
                }
                Console.WriteLine("------------------------");
            }
        }
        private static void PrintPakedConnections(List<PackedConnection> packedConnections)
        {
            foreach (var packedConnection in packedConnections)
            {
                Console.WriteLine("{0} {1} {2} {3}", packedConnection.starOrientation, packedConnection.startId, packedConnection.sinkOrientation, packedConnection.sinkId);
            }
        }
    }
}
