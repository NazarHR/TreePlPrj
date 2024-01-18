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
    internal partial class SaveManager
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
