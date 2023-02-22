using System;
using System.Collections.Generic;
using System.Drawing;
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

    public class vec4
    {
        public double r; public double g; public double b; public double a;

        public uint ConvertToRGBA()
        {
            uint red = (uint)(r * 255);
            uint green = (uint)(g * 255);
            uint blue = (uint)(b * 255);
            uint alpha = (uint)(a * 255);

            uint result = (alpha << 24) | (red << 16) | (green << 8) | blue;
            return result;
        }


    }

    public class sphere
    {
        public vec3 pos = new vec3(); // position
        public double radius { get; set; }

        public vec4 color = new vec4();
        public uint emission { get; set; } // ljus som den utsänder, ljuskälla.
        public void set(double x1, double y1, double z1, double r, vec4 c, uint e)
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

        static vec4 sphere_intersect(vec3 ray_origin, vec3 ray_dir, ref sphere s)
        {
            //(bx^2 + by^2)t^2 + (2(axbx + ayby))t + (ax^2 + ay^2 - r^2) = 0
            // a = rayorgin 
            // b = ray direction
            // r = radius
            // t = hitdistance

            double a = dot(ray_dir, ray_dir);
            double b = 2.0f * dot(ray_dir, ray_origin);
            double c = dot(ray_origin, ray_origin) - (s.radius * s.radius);
            // quadratic formula discriminant
            // b^2 - 4ac

            // (-b +- sqrt(discriminant) /2a)

            double discriminant = b * b - 4.0f * a * c;
            vec4 color = new vec4();
            if (discriminant <= 0.0f)
            {
                color.r = 0;
                color.g = 0;
                color.b = 0;
                color.a = 1;
                return color;
            }

            double t0 = (-b + Math.Sqrt(discriminant)) / (2 * a);
            double closestT = (-b - Math.Sqrt(discriminant)) / (2 * a);

            vec3 h0 = new vec3();
            h0.x = ray_origin.x + ray_dir.x * t0;
            h0.y = ray_origin.y + ray_dir.y * t0;
            h0.z = ray_origin.z + ray_dir.z * t0;
            vec3 h1 = new vec3();
            h1.x = ray_origin.x + ray_dir.x * closestT;
            h1.y = ray_origin.y + ray_dir.y * closestT;
            h1.z = ray_origin.z + ray_dir.z * closestT;


            vec4 sphereColor = new vec4();
            sphereColor.r = 0.5 - h1.x;
            sphereColor.g = 0.5 - h1.y;
            sphereColor.b = 0.5 - h1.z;
            sphereColor.a = 1;

            return sphereColor;
        }


        public static void loop(ref System.Drawing.Bitmap bitmap)
        {
            vec3 camera = new vec3();
            camera.x = 0; camera.y = 0; camera.z = -1;
            sphere s = new sphere();
            s.pos.x = 0;
            s.pos.y = 0;
            s.pos.z = 0;
            s.radius = 0.5;
            s.color.r = 0;
            s.color.g = 0;
            s.color.b = 0;
            s.color.a = 0;
            s.emission = 0x0;

            vec3 ray_dir = new vec3();

            for (int y = 1; y < 600; y++)
            {
                for (int x = 1; x < 800; x++)
                {
                    ray_dir.x = ((double)x / 600) * 2 - 1;
                    ray_dir.y = ((double)y / 600) * 2 - 1;
                    ray_dir.z = -1.0f;
                    bitmap.SetPixel(x, y, (System.Drawing.Color)System.Drawing.Color.FromArgb((int)sphere_intersect(camera, ray_dir, ref s).ConvertToRGBA()));
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


