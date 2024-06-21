using NetworkService.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace NetworkService.Helper
{
    public enum Dock 
    {
        Left,
        Right
    }
    public class NodeLine
    {
        public NodeLine(Line line, bool isStartPoint, int? startServerId, int? endServerId)
        {
            Line = line;
            IsStartPoint = isStartPoint;
            StartServerId = startServerId;
            EndServerId = endServerId;
        }
         public NodeLine()
        {
          
        }


        public Line Line { get; set; }
        public bool IsStartPoint {  get; set; }
        public Dock Dock {  get; set; }
        

        public int? StartServerId { get; set; }
        public int? EndServerId { get; set; }

        public void MoveLine(double x, double y)
        {
            if(IsStartPoint)
            {
                Line.X1 = x;
                Line.Y1 = y;
            }
            else
            {
                Line.X2 = x;
                Line.Y2 = y;
            }
        }
        public override string ToString()
        {
            return $"StartServerId: {StartServerId}\nEndServerId: {EndServerId}\nIsStartPoint: {IsStartPoint}\nDock: {Dock}";
        }

    }
}
