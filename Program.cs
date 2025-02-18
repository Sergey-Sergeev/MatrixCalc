

namespace MatrixCalc
{
    static class MatrixCalc
    {
        static private MatrixCalculator matrixCalculator;
        static private ComandManager comandManager;
        static int Main()
        {
            matrixCalculator = new MatrixCalculator();
            comandManager = new ComandManager();

            /*Double r1 = 1;
            Double r2 = 2;
            Double r3 = 3;
            Double r4 = 4;
            Double r5 = 5;
            Double r6 = 6;
            Double r7 = 7;
            Double r8 = 8;

            Double u1 = 8;
            Double u2 = 10;
            Double j = 0.005d;

            Double[][] data = {
                new Double[] { 1,0,1,-1,0,0,0 },
                new Double[] { 0,1,-1,0,1,0,0 },
                new Double[] { -1,0,0,0,0,0,1 },
                new Double[] { 0,-1,0,0,0,-1,0 },

                new Double[] { 0,  r2, r3, r4, 0,  -r7, 0 },
                new Double[] { 0,  0,  r3, r4, r6, 0,   0 },
                new Double[] { -r1,0,  r3, 0,  r6, 0,  -r8 }

                //new Double[] { 0,r2,0,0,-r5,-r6,0,r8 },
            };



            ExtendedMatrix exm = new ExtendedMatrix(7, 7, data);
            exm.ExtendData = new Double[] {
                0,
                0,
                -j,
                 j,
                u2,
                u2,
                u2 - u1

            };



            ComandManager._matrixMemory.Add(exm, "a");

            comandManager.ProccessCmd("show");
            comandManager.ProccessCmd("getzs a");
            Console.WriteLine("|\tI1\tI2\tI3\tI4\tI5\tI6\tI7\tI8\t|");*/


            while (true)
            {
                string str = Console.ReadLine();

                if (comandManager.ProccessCmd(str) != 0)
                {
                    Console.WriteLine("error");
                }
            }

        }
    }
}
