using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using static TreePlPrj.Connector;

namespace TreePlPrj
{
    internal class SaveManager
    {
        public struct PackedGoal
        {
            double x;
            double y;
            double width;
            double height;
            public int id;
            public Tuple<string, bool>[] goals;
        }

        struct PackedConnection
        {
            int startId;
            ConnectorOrientation starOrientation;
            int sinkId;
            ConnectorOrientation sinkOrientation;
        }
        static public PackedGoal Pack(GoalControl goalControl, int id)
        {
            Point point = goalControl.getPosition();
            double x= point.X;
            double y= point.Y;
            double width = goalControl.DesiredSize.Width;
            double height = goalControl.DesiredSize.Height;

            PackedGoal packedGoal = new PackedGoal();
            return packedGoal;
        }
    }
}
