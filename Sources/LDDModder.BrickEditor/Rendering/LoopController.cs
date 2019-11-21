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

        private System.Windows.Forms.Timer RenderTimer;
        private System.Threading.Timer RenderTimer2;

        public event UpdateFrameDelegate UpdateFrame;

        public event RenderFrameDelegate RenderFrame;

        public LoopController(GLControl control)
        {
            Control = control;
            TargetRenderFrequency = 30;
            TargetUpdateFrequency = 60;
            RenderWatch = new Stopwatch();
            UpdateWatch = new Stopwatch();
            WindowHandle = control.FindForm().Handle;

            RenderTimer2 = new System.Threading.Timer(OnRenderTick, null, 
                System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

            RenderTimer = new System.Windows.Forms.Timer();
            RenderTimer.Tick += RenderTimer_Tick;
        }

        public void ForceRender()
        {
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
                IsRunning = true;
                RenderWatch.Restart();
                UpdateWatch.Restart();
                LastRender = 0;
                LastUpdate = 0;
                RenderTimer.Interval = (int)(TargetRenderPeriod * 1000.0);
                RenderTimer.Start();
                //RenderTimer2.Change(0, (int)(TargetRenderPeriod * 1000.0));
                //Application.Idle += Application_Idle;
                UpdateThread = new Thread(UpdateLoop);
                UpdateThread.Start();
            }
            
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                RenderTimer.Stop();
                //RenderTimer2.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                //Application.Idle -= Application_Idle;
                if (UpdateThread != null)
                {
                    UpdateThread.Join();
                    UpdateThread = null;
                }
                RenderWatch.Stop();
                UpdateWatch.Stop();
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

        private void OnRenderTick(object state)
        {
            DispatchRenderFrame(true);
        }

        private void RenderTimer_Tick(object sender, EventArgs e)
        {
            DispatchRenderFrame(true);
        }

        private void DispatchRenderFrame(bool force = false)
        {
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
            foreach (Delegate del in RenderFrame.GetInvocationList())
            {
                if (del.Target is ISynchronizeInvoke sync)
                    sync.BeginInvoke(del, null);
            }
            //RenderFrame?.Invoke();
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
            while (IsRunning)
            {
                DispatchUpdateFrame();
                Thread.Sleep((int)(TargetUpdatePeriod * 1000));
            }
        }
    }
}
