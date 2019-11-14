using LDDModder.BrickEditor.Native;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public delegate void UpdateFrameDelegate(float deltaMs);

    public delegate void RenderFrameDelegate();

    public class LoopController
    {
        public GLControl Control { get; private set; }

        public IntPtr WindowHandle { get; set; }

        public double TargetRenderFrequency { get; set; }

        public double TargetUpdateFrequency { get; set; }

        public double TargetRenderPeriod => 1f / TargetRenderFrequency;

        public double TargetUpdatePeriod => 1f / TargetUpdateFrequency;

        private Thread UpdateThread;

        private Stopwatch RenderWatch;
        private Stopwatch UpdateWatch;

        public event UpdateFrameDelegate UpdateFrame;

        public event RenderFrameDelegate RenderFrame;

        public LoopController(GLControl control)
        {
            Control = control;
            TargetRenderFrequency = 30;
            TargetUpdateFrequency = 60;
            RenderWatch = new Stopwatch();
            UpdateWatch = new Stopwatch();
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
            UpdateThread = new Thread(RenderLoop);
            UpdateThread.Start();
        }

        public void Stop()
        {

        }

        private void DispatchRenderFrame()
        {
            double timestamp = RenderWatch.Elapsed.TotalSeconds;
            //double elapsed = ClampElapsed(timestamp - render_timestamp);
            //if (elapsed > 0.0 && elapsed >= TargetRenderPeriod)
            //{
            //    //RaiseRenderFrame(elapsed, ref timestamp);
            //}
        }

        static double ClampElapsed(double elapsed) 
        {
            return elapsed < 0 ? 0 : (elapsed > 1 ? 1 : elapsed);
        }

        private void RenderLoop()
        {

        }
    }
}
