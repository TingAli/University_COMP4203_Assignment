﻿using COMP4203.Web.Controllers;
using System.Collections.Generic;

namespace COMP4203.Web.Models
{
    public class Route
    {
        private List<MobileNode> nodeRoute;

        private double sdp;

		public Route()
		{
			nodeRoute = new List<MobileNode>();
            sdp = 0;
		}

        // Use to retrieve route's SDP
        public double GetSDP()
        {
            return sdp;
        }

        // Used to calculate route's SDP
        public double CalcSDP()
        {
            double total = 0;
            foreach (MobileNode node in nodeRoute) {
                total += node.GetForwardingProbability();
            }
            return total / nodeRoute.Count;
        }

        public double CalcModifiedSDP()
        {
            double total = 0;
            foreach (MobileNode node in nodeRoute)
            {
                total += node.GetForwardingProbability();
            }
            double sdp = total / nodeRoute.Count;
            new ComponentController().PrintToOutputPane("DEBUG_SDP", "Entered Method 1");
            for (int i = nodeRoute.Count-2; i >= 0; i--)
            {
                if (nodeRoute[i].blackList.Contains(nodeRoute[i+1]))
                {
                    double start = sdp;
                    sdp *= 0.9;
                    double end = sdp;
                    new ComponentController().PrintToOutputPane("DEBUG_SDP", "Decreased SDP from " + start + " to " + end);
                }
            }

            return sdp;
        }

        public List<MobileNode> GetNodeRoute() => nodeRoute;

        public Route Copy()
        {
            Route packet = new Route();
            foreach (MobileNode node in nodeRoute)
            {
                packet.AddNodeToRoute(node);
            }
            return packet;
        }

        public void AddNodeToRoute(MobileNode node)
        {
            nodeRoute.Add(node);
        }

        public void AddNodesToRoute(List<MobileNode> nodes)
        {
            nodeRoute.AddRange(nodes);
        }

        public bool IsInRouteAlready(MobileNode node)
        {
            return nodeRoute.Contains(node);
        }

        public bool RouteCompare(Route route)
        {
            if (nodeRoute.Count != route.GetNodeRoute().Count)
            {
                return false;
            } else
            {
                for (int i = 0; i < nodeRoute.Count; i++)
                {
                    if (!nodeRoute[i].Equals(route.GetNodeRoute()[i]))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public int GetRouteLength()
        {
            return nodeRoute.Count;
        }
        
        public string GetRouteAsString()
        {
            string output = "";
            foreach (MobileNode node in nodeRoute)
            {
                output += node.GetNodeID() + " ";
            }
            return output;
        }

        public double GetTransmissionTime()
        {
            double transmissionTime = 0;
            for (int i = 0; i < nodeRoute.Count - 1; i++)
            {
                transmissionTime += nodeRoute[i].GetTransmissionTimeToNode(nodeRoute[i+1]);
            }
            return transmissionTime;
        }
    }
}
