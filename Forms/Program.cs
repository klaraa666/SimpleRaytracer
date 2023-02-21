using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms
{


    public class vec3
    {
        public double x; public double y; public double z;
        public void normalize()
        {
            double nor2 = x * x + y * y + z * z;
            if (nor2 > 0)
            {
                double invNor = 1 / Math.Sqrt(nor2);
                x *= invNor;
                y *= invNor;
                z *= invNor;
            }
        }
    }
    public class sphere
    {
        public vec3 pos = new vec3(); // position
        public double radius { get; set; }
        public uint color { get; set; }
        public uint emission { get; set; } // ljus som den utsänder, ljuskälla.
        public void set(double x1, double y1, double z1, double r, uint c, uint e)
        {
            this.pos.x = x1;
            this.pos.y = y1;
            this.pos.z = z1;
            this.radius = r;
            this.color = c;
            this.emission = e;
        }
    }
    
    internal static class Program
    {
        [STAThread]

        static double dot(vec3 a, vec3 b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        static uint sphere_intersect(vec3 ray_origin, vec3 ray_dir, ref sphere s)
        {
            //(bx^2 + by^2)t^2 + (2(axbx + ayby))t + (ax^2 + ay^2 - r^2) = 0
            // a = rayorgin 
            // b = ray direction
         
            double a = dot(ray_dir, ray_dir);
            double b = 2.0f * dot(ray_dir, ray_origin);
            double c = dot(ray_origin, ray_origin) - (s.radius * s.radius);
            // quadratic formula discriminant
            // b^2 - 4ac

            double discriminant = b * b - 4.0f * a * c;
            if (discriminant >= 0.0f)
            {
                return 0xffBF33FF;
            }
            return 0xff000000;
        }


        public static void loop(ref System.Drawing.Bitmap bitmap)
        {
            vec3 camgirl = new vec3();
            camgirl.x = 0; camgirl.y = 0; camgirl.z = -1;
            sphere s = new sphere();
            s.pos.x = 0;
            s.pos.y = 0;
            s.pos.z = 0;
            s.radius = 0.5;
            s.color = 0xff808080; s.emission = 0x0;

            vec3 ray_dir = new vec3();
            
            for (int y = 1; y < 600; y++)
            {
                for (int x = 1; x < 800; x++)
                {
                    ray_dir.x = ((double)x / 600) * 2 - 1;
                    ray_dir.y = ((double)y / 600) * 2 - 1;
                    ray_dir.z = -1.0f;
                    bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb((int)sphere_intersect(camgirl, ray_dir, ref s)));
                }
            }
        }
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }

    }
}


