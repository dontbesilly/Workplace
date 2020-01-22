using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Windows;

namespace Workplace1c
{
    public class Server
    {
        private string serverRef;
        private string adminUser;
        private string adminPass;

        public Server(string serverRef, string adminUser, string adminPass)
        {
            this.serverRef = serverRef;
            this.adminUser = adminUser;
            this.adminPass = adminPass;
        }

        public void ClearSessions(string baseName, int delay)
        {
            V83.IInfoBaseShort currentBase = null;

            var agent = new V83.COMConnector().ConnectAgent($"tcp://{serverRef}");
            V83.IClusterInfo cluster = (V83.IClusterInfo)agent.GetClusters().GetValue(0);
            agent.Authenticate(cluster, adminUser, adminPass);
            var bases = agent.GetInfoBases(cluster);

            foreach (V83.IInfoBaseShort item in bases)
            {
                if (item.Name.ToString().ToLower() == baseName.ToLower())
                {
                    currentBase = item;
                    break;
                }
            }

            if (currentBase != null)
            {
                var sessions = agent.GetInfoBaseSessions(cluster, currentBase);

                short i = 0;
                foreach (var s in sessions) { i++; }

                if (i > 0)
                {
                    Thread.Sleep(delay);
                }

                foreach (V83.ISessionInfo session in sessions)
                {
                    ClearSession(agent, cluster, session);
                }
            }
            else
            {
                throw new System.Exception("Не найдена база на сервере!");
            }
        }

        public IEnumerable<string> GetBases()
        {
            var agent = new V83.COMConnector().ConnectAgent($"tcp://{serverRef}");
            V83.IClusterInfo cluster = (V83.IClusterInfo)agent.GetClusters().GetValue(0);
            agent.Authenticate(cluster, adminUser, adminPass);
            var bases = agent.GetInfoBases(cluster);

            List<string> listBases = new List<string>();
            foreach (V83.IInfoBaseShort item in bases)
            {
                listBases.Add(item.Name);
            }

            return listBases;
        }

        [HandleProcessCorruptedStateExceptions]
        public void ClearSession(V83.IServerAgentConnection agent, V83.IClusterInfo cluster, V83.ISessionInfo session)
        {
            try
            {
                agent.TerminateSession(cluster, session);
            }
            catch (System.Exception err)
            {
                MessageBox.Show(err.ToString());
            }
        }
    }
}
