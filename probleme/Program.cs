using System;
using System.Collections.Generic;
using System.IO;

namespace probleme
{
    class Program
    {
        static void Main(string[] args)
        {
            List<User> data = read_csv(@"D:\dev\vs repos\probleme\probleme\data.csv");
            Console.WriteLine(pred(data[1], 0, data));
            Console.Read();
        }

        static List<User> read_csv(string path)
        {
            var lines = File.ReadAllLines(path);
            var users = new List<User>();
            foreach (var line in lines)
            {
                var values = line.Split(',');
                var user = new User() { products = values };
                users.Add(user);
            }
            return users;
        }

        static double SC(User u, User v)
        {
            double c = cov(u, v);
            double va = total_var(u, v);
            return c/va;
        }

        static double cov(User u, User v)
        {
            double cov = 0;
            for (int i = 0; i < u.products.Length; i++)
            {
                if (u.products[i].Length>0 && v.products[i].Length>0)
                {
                    cov += (Convert.ToInt32(u.products[i]) - u.getMean()) * (Convert.ToInt32(v.products[i]) - v.getMean());
                }
            }
            return cov;
        }

        static double total_var(User u, User v)
        {
            double vi = 0;
            double vj = 0;
            for (int i = 0; i < u.products.Length; i++)
            {
                if (u.products[i].Length > 0 && v.products[i].Length > 0)
                {
                    vi += Math.Pow((Convert.ToInt32(u.products[i]) - u.getMean()), 2);
                    vj += Math.Pow((Convert.ToInt32(v.products[i]) - v.getMean()), 2);
                }
            }
            return Math.Sqrt(vi) * Math.Sqrt(vj);
        }

        static double pred(User u, int p, List<User> users)
        {
            double dividend = 0;
            double divisor = 0;
            foreach (User user  in users)
            {
                if (user != u)
                {
                    if (user.products[p].Length>0)
                    {
                        dividend += SC(u, user) * (Convert.ToInt32(user.products[p]) - user.getMean());
                        divisor += Math.Abs(SC(u, user));
                    }
                }
            }
            return u.getMean() + (dividend / divisor);
        }
    }

    public class User
    {
        public string[] products { get; set; }
        public double getMean()
        {
            double sum = 0;
            int count = 0;
            foreach (var i in products)
            {
                if (i.Length > 0)
                {
                    sum += Convert.ToInt32(i);
                    count++;
                }
            }
            return sum / count;
        }
    }
}
