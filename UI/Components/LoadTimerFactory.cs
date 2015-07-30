using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveSplit.UI.Components
{
    public class LoadTimerFactory : IComponentFactory
    {
        public string ComponentName
        {
            get { return "Load Timer"; }
        }

        public string Description
        {
            get { return "Displays amount of (load) time removed from Game Time by the active autosplitter."; }
        }

        public ComponentCategory Category
        {
            get { return ComponentCategory.Information; }
        }

        public IComponent Create(LiveSplitState state)
        {
            return new LoadTimer(state);
        }

        public string UpdateName
        {
            get { return ComponentName; }
        }

        public string UpdateURL
        {
            get { return "https://raw.githubusercontent.com/glasnonck/LiveSplit.LoadTimer/master/"; }
        }

        public string XMLURL
        {
            get { return this.UpdateURL + "Components/update.LiveSplit.LoadTimer.xml"; }
        }

        public Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }
    }
}
