using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        double d, q;
        double _u, _v, _w;
        double gain = 100.0;
        string str = "angle\tu\tv\tw\r\n";

        public Form1()
        {
            InitializeComponent();
            for (var i = 0; i < 360; i++)
            {
                var rad = i * Math.PI / 180.0;
                var a = Math.Cos(rad);
                var b = Math.Sin(rad);

                var s0 = (i / 60);
                var s1 = sector(a, b);
                while (s0 != s1)
                {
                    Console.Write("err");
                    s1 = sector(a, b);
                }

                var z = Math.Sqrt(3) / 1.5; //115.4%
            }

            for (var i = 0; i < 360; i++)
            {
                var rad = i * Math.PI / 180.0;
                var a = Math.Cos(rad);
                var b = Math.Sin(rad);

                var sec = sector(a, b);
                var rad1 = sec * 60.0 * Math.PI / 180.0;
                var x = (a * Math.Cos(rad1) + b * Math.Sin(rad1));
                var y = (b * Math.Cos(rad1) - a * Math.Sin(rad1));

                var A = Math.Cos((i % 60) * Math.PI / 180.0);
                var B = Math.Sin((i % 60) * Math.PI / 180.0);

                var da = Math.Round(A - x, 4);
                var db = Math.Round(B - y, 4);
                if (0 != da || 0 != db)
                {
                    Console.Write("");
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            init();
            for (var x = 0; x < 360; x++)
            {
                d = 1.0; q = 0.0;
                uvw0(x);
                chart1.Series[0].Points.AddXY(x, _u);
                chart1.Series[1].Points.AddXY(x, _v);
                chart1.Series[2].Points.AddXY(x, _w);

                uvw1(x);
                chart2.Series[0].Points.AddXY(x, _u);
                chart2.Series[1].Points.AddXY(x, _v);
                chart2.Series[2].Points.AddXY(x, _w);

                d = 0.0; q = +1.0;
                uvw0(x);
                chart3.Series[0].Points.AddXY(x, _u);
                chart3.Series[1].Points.AddXY(x, _v);
                chart3.Series[2].Points.AddXY(x, _w);

                uvw1(x);
                chart4.Series[0].Points.AddXY(x, _u);
                chart4.Series[1].Points.AddXY(x, _v);
                chart4.Series[2].Points.AddXY(x, _w);
                str += $"{x}\t{_u}\t{_v}\t{_w}\r\n";

                d = 0.0; q = -1.0;
                uvw0(-x);
                chart5.Series[0].Points.AddXY(x, _u);
                chart5.Series[1].Points.AddXY(x, _v);
                chart5.Series[2].Points.AddXY(x, _w);

                uvw1(-x);
                chart6.Series[0].Points.AddXY(x, _u);
                chart6.Series[1].Points.AddXY(x, _v);
                chart6.Series[2].Points.AddXY(x, _w);
            }
            Clipboard.SetText(str);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            init();
            for (var x = 0; x < 360; x++)
            {
                d = 1.0; q = 0.0;
                uvw0(x);
                tocurrent();
                chart1.Series[0].Points.AddXY(x, _u);
                chart1.Series[1].Points.AddXY(x, _v);
                chart1.Series[2].Points.AddXY(x, _w);

                uvw1(x);
                tocurrent();
                chart2.Series[0].Points.AddXY(x, _u);
                chart2.Series[1].Points.AddXY(x, _v);
                chart2.Series[2].Points.AddXY(x, _w);

                d = 0.0; q = +1.0;
                uvw0(x);
                tocurrent();
                chart3.Series[0].Points.AddXY(x, _u);
                chart3.Series[1].Points.AddXY(x, _v);
                chart3.Series[2].Points.AddXY(x, _w);

                uvw1(x);
                tocurrent();
                chart4.Series[0].Points.AddXY(x, _u);
                chart4.Series[1].Points.AddXY(x, _v);
                chart4.Series[2].Points.AddXY(x, _w);
                str += $"{x}\t{_u}\t{_v}\t{_w}\r\n";

                d = 0.0; q = -1.0;
                uvw0(-x);
                tocurrent();
                chart5.Series[0].Points.AddXY(x, _u);
                chart5.Series[1].Points.AddXY(x, _v);
                chart5.Series[2].Points.AddXY(x, _w);

                uvw1(-x);
                tocurrent();
                chart6.Series[0].Points.AddXY(x, _u);
                chart6.Series[1].Points.AddXY(x, _v);
                chart6.Series[2].Points.AddXY(x, _w);
            }
            Clipboard.SetText(str);
        }
        void uvw0(double angle)
        {
            var rad = angle * Math.PI / 180.0;
#if true
#if false
            //つっかかるような反転(現在)
            var a = +d * Math.Cos(rad) + q * Math.Sin(rad);
            var b = -d * Math.Sin(rad) + q * Math.Cos(rad);
#else
            //ちゃんとした反転
            var a = +d * Math.Cos(rad) - q * Math.Sin(rad);
            var b = -d * Math.Sin(rad) - q * Math.Cos(rad);
#endif
#else
            //合わせた波形(正転)　
            var a = +d * Math.Cos(rad) - q * Math.Sin(rad);
            var b = +d * Math.Sin(rad) + q * Math.Cos(rad);
#endif
            var u = a;
            var v = (Math.Sqrt(3) * b - a) / 2;
            var w = -(u + v);
            _u = (1.0 - u) * gain / 2;
            _v = (1.0 - v) * gain / 2;
            _w = (1.0 - w) * gain / 2;
        }
        void uvw1(int angle)
        {
            var deg = angle * Math.PI / 180.0;
#if true
#if false
            //つっかかるような反転
            var a0 = (+d * Math.Cos(deg) + q * Math.Sin(deg));
            var b0 = (-d * Math.Sin(deg) + q * Math.Cos(deg));
#else
            //反転
            var a0 = (+d * Math.Cos(deg) - q * Math.Sin(deg));
            var b0 = (-d * Math.Sin(deg) - q * Math.Cos(deg));
#endif
#else
            //正転
            var a0 = (+d * Math.Cos(deg) - q * Math.Sin(deg));
            var b0 = (+d * Math.Sin(deg) + q * Math.Cos(deg));
#endif
            var sec = sector(a0, b0);
            var rad0 = sec * 60 * Math.PI / 180.0;
            var a = gain * ((a0 * Math.Cos(rad0) + b0 * Math.Sin(rad0)));        // cosA*cosB + sinA*sinB
            var b = gain * ((b0 * Math.Cos(rad0) - a0 * Math.Sin(rad0)));        // sinA*cosB - cosA*sinB

            var t1 = (Math.Sqrt(3) * a - b) / 2.0f;
            var t2 = b;
            var t3 = gain - (t1 + t2);
            var t3a = t3 / 2.0f;
            var t23a = t2 + t3a;
            var t123a = t1 + t23a;
            var t3b = gain - t3a;
            var t23b = gain - t23a;
            var t123b = gain - t123a;

            //反転のときつっかかるような反転
            if (2 == sec)      { _u = t123a; _w = t23b; _v = t3a; }
            else if (3 == sec) { _w = t123b; _v = t23a; _u = t3b; }
            else if (4 == sec) { _v = t123a; _u = t23b; _w = t3a; }
            else if (5 == sec) { _u = t123b; _w = t23a; _v = t3b; }
            else if (0 == sec) { _w = t123a; _v = t23b; _u = t3a; }
            else               { _v = t123b; _u = t23a; _w = t3b; }
        }
        void tocurrent()
        {
            int u = 0, v = 0, w = 0;
            for (var i = 0; i < gain * 2; i++)
            {
                var bu = (_u <= i);
                var bv = (_v <= i);
                var bw = (_w <= i);

                if (!bu && !bv && !bw) { u -= 0; v -= 0; w -= 0; }
                if (!bu && !bv &&  bw) { u -= 1; v -= 1; w += 2; }
                if (!bu &&  bv && !bw) { u -= 1; v += 2; w -= 1; }
                if (!bu &&  bv &&  bw) { u -= 2; v += 1; w += 1; }
                if ( bu && !bv && !bw) { u += 2; v -= 1; w -= 1; }
                if ( bu && !bv &&  bw) { u += 1; v -= 2; w += 1; }
                if ( bu &&  bv && !bw) { u += 1; v += 1; w -= 2; }
                if ( bu &&  bv &&  bw) { u += 0; v += 0; w += 0; }
            }
            _u = u;
            _v = v;
            _w = w;
        }

        int sector(double x, double y)
        {
            x = Math.Round(x, 3);
            y = Math.Round(y, 3);
            if (0 <= x)
            {
                if (0 <= y)
                    return (0.5 >= x) ? 1 : 0;
                else
                    return (0.5 <= x) ? 5 : 4;
            }
            else
            {
                if (0 < y)
                    return (-0.5 >= x) ? 2 : 1;
                else
                    return (-0.5 <= x) ? 4 : 3;
            }
        }

        void init()
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            chart1.Series[2].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart2.Series[1].Points.Clear();
            chart2.Series[2].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart3.Series[1].Points.Clear();
            chart3.Series[2].Points.Clear();
            chart4.Series[0].Points.Clear();
            chart4.Series[1].Points.Clear();
            chart4.Series[2].Points.Clear();
            chart5.Series[0].Points.Clear();
            chart5.Series[1].Points.Clear();
            chart5.Series[2].Points.Clear();
            chart6.Series[0].Points.Clear();
            chart6.Series[1].Points.Clear();
            chart6.Series[2].Points.Clear();
            str = "angle\tu\tv\tw\r\n";
        }
    }
}
