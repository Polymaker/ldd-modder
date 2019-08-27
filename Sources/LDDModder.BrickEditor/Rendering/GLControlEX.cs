using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LDDModder.BrickEditor.Rendering
{
    public class GLControlEX : GLControl
    {
        private MSG msg;
        private const double MaxFrequency = 500.0;
        private readonly Stopwatch watchRender = new Stopwatch();
        private readonly Stopwatch watchUpdate = new Stopwatch();
        private Thread updateThread;
        private bool isSingleThreaded;
        private bool isExiting;

        private double update_period;

        private double render_period;

        private double target_update_period;

        private double target_render_period;

        private double update_time;

        private double render_time;

        private double update_timestamp;

        private double render_timestamp;

        private double update_epsilon;

        private bool is_running_slowly;

        private FrameEventArgs update_args = new FrameEventArgs();

        private FrameEventArgs render_args = new FrameEventArgs();

        public bool IsExiting
        {
            get
            {
                EnsureUndisposed();
                return isExiting;
            }
        }

        public double RenderFrequency
        {
            get
            {
                EnsureUndisposed();
                if (render_period == 0.0)
                {
                    return 1.0;
                }
                return 1.0 / render_period;
            }
        }

        public double RenderPeriod
        {
            get
            {
                EnsureUndisposed();
                return render_period;
            }
        }

        public double RenderTime
        {
            get
            {
                EnsureUndisposed();
                return render_time;
            }
            protected set
            {
                EnsureUndisposed();
                render_time = value;
            }
        }

        public double TargetRenderFrequency
        {
            get
            {
                EnsureUndisposed();
                if (TargetRenderPeriod == 0.0)
                {
                    return 0.0;
                }
                return 1.0 / TargetRenderPeriod;
            }
            set
            {
                EnsureUndisposed();
                if (value < 1.0)
                {
                    TargetRenderPeriod = 0.0;
                }
                else if (value <= 500.0)
                {
                    TargetRenderPeriod = 1.0 / value;
                }
            }
        }

        public double TargetRenderPeriod
        {
            get
            {
                EnsureUndisposed();
                return target_render_period;
            }
            set
            {
                EnsureUndisposed();
                if (value <= 0.002)
                {
                    target_render_period = 0.0;
                }
                else if (value <= 1.0)
                {
                    target_render_period = value;
                }
            }
        }

        public double TargetUpdateFrequency
        {
            get
            {
                EnsureUndisposed();
                if (TargetUpdatePeriod == 0.0)
                {
                    return 0.0;
                }
                return 1.0 / TargetUpdatePeriod;
            }
            set
            {
                EnsureUndisposed();
                if (value < 1.0)
                {
                    TargetUpdatePeriod = 0.0;
                }
                else if (value <= 500.0)
                {
                    TargetUpdatePeriod = 1.0 / value;
                }
            }
        }

        public double TargetUpdatePeriod
        {
            get
            {
                EnsureUndisposed();
                return target_update_period;
            }
            set
            {
                EnsureUndisposed();
                if (value <= 0.002)
                {
                    target_update_period = 0.0;
                }
                else if (value <= 1.0)
                {
                    target_update_period = value;
                }
            }
        }

        public double UpdateFrequency
        {
            get
            {
                EnsureUndisposed();
                if (update_period == 0.0)
                {
                    return 1.0;
                }
                return 1.0 / update_period;
            }
        }

        public double UpdatePeriod
        {
            get
            {
                EnsureUndisposed();
                return update_period;
            }
        }

        public double UpdateTime
        {
            get
            {
                EnsureUndisposed();
                return update_time;
            }
        }

        public void ProcessEvents()
        {
            while (PeekMessage(ref msg, IntPtr.Zero, 0, 0, PeekMessageFlags.Remove))
            {
               TranslateMessage(ref msg);
               DispatchMessage(ref msg);
            }
        }

        public void Run(double updates_per_second, double frames_per_second)
        {
            EnsureUndisposed();
            try
            {
                if (!isSingleThreaded)
                {
                    //updateThread = new Thread(UpdateThread);
                    updateThread.Start();
                }


            }
            catch
            {

            }
        }

        protected void EnsureUndisposed()
        {
            if (!IsDisposed)
            {
                return;
            }
            throw new ObjectDisposedException(GetType().Name);
        }


        private struct POINT
        {
            public int X;

            public int Y;

            public POINT(int x, int y)
            {
                X = x;
                Y = y;
            }

            public Point ToPoint()
            {
                return new Point(X, Y);
            }

            public override string ToString()
            {
                return "Point {" + X.ToString() + ", " + Y.ToString() + ")";
            }
        }

        private struct MSG
        {
            public IntPtr HWnd;

            public uint Message;

            public IntPtr WParam;

            public IntPtr LParam;

            public uint Time;

            public POINT Point;

            public override string ToString()
            {
                return $"msg=0x{(int)Message:x} ({Message.ToString()}) hwnd=0x{HWnd.ToInt32():x} wparam=0x{WParam.ToInt32():x} lparam=0x{LParam.ToInt32():x} pt=0x{Point:x}";
            }
        }

        [Flags]
        private enum PeekMessageFlags : uint
        {
            NoRemove = 0x0,
            Remove = 0x1,
            NoYield = 0x2
        }

        [DllImport("User32.dll")]
        private static extern bool PeekMessage(ref MSG msg, IntPtr hWnd, int messageFilterMin, int messageFilterMax, PeekMessageFlags flags);

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern bool TranslateMessage(ref MSG lpMsg);

        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr DispatchMessage(ref MSG msg);

    }
}
