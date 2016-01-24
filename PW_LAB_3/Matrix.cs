using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PW_LAB_3
{
    class Matrix
    {
        public int[,] matrix;
        Random rand;

        public Matrix()
        {
            this.matrix = new int[1024, 1024];
        }

        public void drawNumbers()
        {
            rand = new Random();

            for(int i=0; i<this.matrix.GetUpperBound(0); i++)
            {
                for(int j=0; j<this.matrix.GetUpperBound(1); j++)
                {
                    matrix[i, j] = rand.Next(0, 9);
                }
            }
        }
    }
}
