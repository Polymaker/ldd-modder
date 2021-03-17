using LDDModder.BrickEditor.Native;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LDDModder.BrickEditor.Rendering
{
    public delegate void UpdateFrameDelegate(double deltaMs);

    public delegate void RenderFrameDelegate();

    public class LoopController
    {

        private static readonly NLog.Logger Logger = NLog.LogManager.GetLogger("LoopController");

        public GLControl Control { get; private set; }

        public IntPtr WindowHandle { get; set; }

        public double TargetRenderFrequency { get; set; }

        public double TargetUpdateFrequency { get; set; }

        public double TargetRenderPeriod => 1f / TargetRenderFrequency;

        public double TargetUpdatePeriod => 1f / TargetUpdateFrequency;

        public bool IsRunning { get; private set; }

        public double RenderFrequency { get; private set; }

        public double UpdateFrequency { get; private set; }

        private Thread UpdateThread;
        private double LastRender;
        private double LastUpdate;

        private Stopwatch RenderWatch;
        private Stopwatch UpdateWatch;

        private System.Windows.Forms.Timer RT1;
        private System.Threading.Timer RT2;
        private System.Timers.Timer RT3;

        public event UpdateFrameDelegate UpdateFrame;

        public event RenderFrameDelegate RenderFrame;

        public enum TimerType
        {
            Winform,
            Threading,
            Timer
        }

        private TimerType _RenderTimerType;

        public TimerType RenderTimerType
        {
            get => _RenderTimerType;
            set
            {
                if (!IsRunning)
                    _RenderTimerType = value;
            }
        }

        public LoopController(GLControl control)
        {
            Control = control;
            TargetRenderFrequency = 30;
            TargetUpdateFrequency = 60;
            RenderWatch = new Stopwatch();
            UpdateWatch = new Stopwatch();
            WindowHandle = control.FindForm().Handle;

            RenderTimerType = TimerType.Winform;
        }

        

        public void ForceRender()
        {
            if (!IsRunning)
                RaiseRenderFrame();
            else
                DispatchRenderFrame(true);
        }

        public void ProcessEvent()
        {
            User32.MSG msg = default(User32.MSG);

            while (User32.PeekMessage(ref msg, WindowHandle, 0, 0, User32.PeekMessageFlags.Remove))
            {
                User32.TranslateMessage(ref msg);
                User32.DispatchMessage(ref msg);
            }
        }


        public void Start()
        {
            if (!IsRunning)
            {
                Logger.Debug("Starting GL loop");
                IsRunning = true;
                RenderWatch.Restart();
                UpdateWatch.Restart();
                LastRender = 0;
                LastUpdate = 0;

                StartRenderTimer();
                UpdateThread = new Thread(UpdateLoop);
                UpdateThread.Start();
            }
            
        }

        private void StartRenderTimer()
        {
            switch (RenderTimerType)
            {
                case TimerType.Winform:
                    {
                        RT1 = new System.Windows.Forms.Timer
                        {
                            Interval = (int)(TargetRenderPeriod * 1000.0)
                        };
                        RT1.Tick += RT1_Tick;
                        RT1.Start();
                        break;
                    }
                case TimerType.Threading:
                    {
                        RT2 = new System.Threading.Timer(RT2_Tick, null,  0, (int)(TargetRenderPeriod * 1000.0));

                        break;
                    }
                case TimerType.Timer:
                    {
                        RT3 = new System.Timers.Timer
                        {
                            AutoReset = true,
                            SynchronizingObject = Control,
                            Interval = (int)(TargetRenderPeriod * 1000.0)
                        };
                        RT3.Elapsed += RT3_Tick;
                        RT3.Start();
                        break;
                    }
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                Logger.Debug("Stoping GL loop");
                IsRunning = false;
                StopRenderTimer();
                if (UpdateThread != null)
                {
                    UpdateThread.Join();
                    UpdateThread = null;
                }
                RenderWatch.Stop();
                UpdateWatch.Stop();
            }
        }

        private void StopRenderTimer()
        {
            switch (RenderTimerType)
            {
                case TimerType.Winform:
                    {
                        RT1.Tick -= RT1_Tick;
                        RT1.Stop();
                        RT1.Dispose();
                        RT1 = null;
                        break;
                    }
                case TimerType.Threading:
                    {
                        RT2.Change(Timeout.Infinite, Timeout.Infinite);
                        RT2.Dispose();
                        RT2 = null;
                        break;
                    }
                case TimerType.Timer:
                    {
                        RT3.Elapsed -= RT3_Tick;
                        RT3.Stop();
                        RT3.Dispose();
                        RT3 = null;
                        break;
                    }
            }
        }

        //private void Application_Idle(object sender, EventArgs e)
        //{
        //    while (Control.IsIdle)
        //    {
        //        ProcessEvent();
        //        DispatchRenderFrame();
        //    }
        //}

        private void RT1_Tick(object sender, EventArgs e)
        {
            DispatchRenderFrame();
        }

        private void RT2_Tick(object state)
        {
            DispatchRenderFrame();
        }

        private void RT3_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            DispatchRenderFrame();
        }

        private void DispatchRenderFrame(bool force = false)
        {
            if (!IsRunning)
                return;
            double timestamp = RenderWatch.Elapsed.TotalSeconds;
            double elapsed = ClampElapsed(timestamp - LastRender);
            if (elapsed > 0.0 && (force || elapsed >= TargetRenderPeriod))
            {
                RenderFrequency = 1d / elapsed;
                LastRender = timestamp;
                RaiseRenderFrame();
            }
        }

        private void RaiseRenderFrame()
        {
            try
            {
                foreach (Delegate del in RenderFrame.GetInvocationList())
                {
                    if (del.Target is ISynchronizeInvoke sync)
                    {
                        //var test = sync.BeginInvoke(del, null);
                        //sync.EndInvoke(test);
                        sync.Invoke(del, null);
                    }
                }
            }
            catch
            {
                Logger.Warn("Error while render frame");
            }
        }

        private void DispatchUpdateFrame()
        {
            double timestamp = UpdateWatch.Elapsed.TotalSeconds;
            double elapsed = ClampElapsed(timestamp - LastUpdate);

            if (elapsed > 0.0/* && elapsed >= TargetUpdatePeriod*/)
            {
                UpdateFrequency = 1d / elapsed;
                LastUpdate = timestamp;
                UpdateFrame?.Invoke(elapsed / 1000.0);
            }
        }

        static double ClampElapsed(double elapsed) 
        {
            return elapsed < 0 ? 0 : (elapsed > 1 ? 1 : elapsed);
        }

        private void UpdateLoop()
        {
            //TODO: Maybe change for Threading.Timer
            while (IsRunning)
            {
                DispatchUpdateFrame();
                Thread.Sleep((int)(TargetUpdatePeriod * 1000));
            }
        }
    }
}
