using LiveSplit.Model;
using LiveSplit.TimeFormatters;
using LiveSplit.UI.Components;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LiveSplit.UI.Components
{
    public class LoadTimer : IComponent
    {
        protected InfoTimeComponent InternalComponent { get; set; }
        public AlternateTimingMethodSettings Settings { get; set; }
        private PossibleTimeSaveFormatter Formatter { get; set; }

        public GraphicsCache Cache { get; set; }

        public float PaddingTop { get { return InternalComponent.PaddingTop; } }
        public float PaddingLeft { get { return InternalComponent.PaddingLeft; } }
        public float PaddingBottom { get { return InternalComponent.PaddingBottom; } }
        public float PaddingRight { get { return InternalComponent.PaddingRight; } }

        public IDictionary<string, Action> ContextMenuControls
        {
            get { return null; }
        }

        public LoadTimer(LiveSplitState state)
        {
            Formatter = new PossibleTimeSaveFormatter();
            InternalComponent = new InfoTimeComponent(null, null, Formatter);
            Cache = new GraphicsCache();
            Settings = new AlternateTimingMethodSettings()
            {
                CurrentState = state
            };
        }

        private void PrepareDraw(LiveSplitState state)
        {
            InternalComponent.DisplayTwoRows = Settings.Display2Rows;

            InternalComponent.NameLabel.HasShadow 
                = InternalComponent.ValueLabel.HasShadow
                = state.LayoutSettings.DropShadows;

            Formatter.Accuracy = Settings.Accuracy;

            InternalComponent.NameLabel.ForeColor = Settings.OverrideTextColor ? Settings.TextColor : state.LayoutSettings.TextColor;
            InternalComponent.ValueLabel.ForeColor = Settings.OverrideTimeColor ? Settings.TimeColor : state.LayoutSettings.TextColor;
        }

        private void DrawBackground(Graphics g, LiveSplitState state, float width, float height)
        {
            if (Settings.BackgroundColor.ToArgb() != Color.Transparent.ToArgb()
                || Settings.BackgroundGradient != GradientType.Plain
                && Settings.BackgroundColor2.ToArgb() != Color.Transparent.ToArgb())
            {
                var gradientBrush = new LinearGradientBrush(
                            new PointF(0, 0),
                            Settings.BackgroundGradient == GradientType.Horizontal
                            ? new PointF(width, 0)
                            : new PointF(0, height),
                            Settings.BackgroundColor,
                            Settings.BackgroundGradient == GradientType.Plain
                            ? Settings.BackgroundColor
                            : Settings.BackgroundColor2);
                g.FillRectangle(gradientBrush, 0, 0, width, height);
            }
        }

        public void DrawVertical(Graphics g, LiveSplitState state, float width, Region clipRegion)
        {
            DrawBackground(g, state, width, VerticalHeight);
            PrepareDraw(state);
            InternalComponent.DrawVertical(g, state, width, clipRegion);
        }

        public void DrawHorizontal(Graphics g, LiveSplitState state, float height, Region clipRegion)
        {
            DrawBackground(g, state, HorizontalWidth, height);
            PrepareDraw(state);
            InternalComponent.DrawHorizontal(g, state, height, clipRegion);
        }

        public float VerticalHeight
        {
            get { return InternalComponent.VerticalHeight; }
        }

        public float MinimumWidth
        {
            get { return InternalComponent.MinimumWidth; }
        }

        public float HorizontalWidth
        {
            get { return InternalComponent.HorizontalWidth; }
        }

        public float MinimumHeight
        {
            get { return InternalComponent.MinimumHeight; }
        }

        public string ComponentName
        {
            get { return "Load Timer"; }
        }

        public Control GetSettingsControl(LayoutMode mode)
        {
            Settings.Mode = mode;
            return Settings;
        }

        public void SetSettings(System.Xml.XmlNode settings)
        {
            Settings.SetSettings(settings);
        }

        public System.Xml.XmlNode GetSettings(System.Xml.XmlDocument document)
        {
            return Settings.GetSettings(document);
        }

        public void RenameComparison(string oldName, string newName)
        {
        }

        public void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode)
        {
            var componentName = Settings.LoadTimeText;
            InternalComponent.LongestString = componentName;
            InternalComponent.NameLabel.Text = componentName;
            InternalComponent.TimeValue = state.CurrentTime[TimingMethod.RealTime] - state.CurrentTime[TimingMethod.GameTime];

            Cache.Restart();
            Cache["NameValue"] = InternalComponent.NameLabel.Text;
            Cache["TimeValue"] = InternalComponent.ValueLabel.Text;

            if (invalidator != null && Cache.HasChanged)
            {
                invalidator.Invalidate(0, 0, width, height);
            }
        }

        public void Dispose()
        {
        }
    }
}
