using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Playlistofy.Utils.AlgorithmicOperations
{
    public class RandomString
    {

        public static string GetRandomString()
        {
            Random ran = new Random();

            string b = "a1b2c3d4e5f6g7h8i9jklmnopqrstuvwxyz";
            int length = 25;
            string randomId = "";

            for (int i = 0; i < length; i++)
            {
                int a = ran.Next(35);
                randomId = randomId + b.ElementAt(a);
            }
            return randomId;
        }
    }
}
