using System;

namespace OE.ALGA.Optimalizalas
{
    public class DinamikusHatizsakPakolas(HatizsakProblema problema)
    {
        private HatizsakProblema problema = problema;
        public int LepesSzam { get; private set; }

        private float[,] TablazatFeltoltes()
        {
            var t = problema.N;
            var h = problema.Wmax;
            var f = new float[t + 1, h + 1];

            for (var i = 1; i <= t; i++)
            {
                for (var j = 1; j <= h; j++)
                {
                    if (j < problema.W[i - 1]) f[i, j] = f[i - 1, j];
                    else f[i, j] = Math.Max(f[i - 1, j], f[i - 1, j - problema.W[i - 1]] + problema.P[i - 1]);
                    LepesSzam++;
                }
            }

            return f;
        }

        public float OptimalisErtek()
        {
            var tablazat = TablazatFeltoltes();
            return tablazat[problema.N, problema.Wmax];
        }

        public bool[] OptimalisMegoldas()
        {
            var f = TablazatFeltoltes();
            var o = new bool[problema.N];
            var t = problema.N;
            var h = problema.Wmax;

            while (t > 0 && h > 0)
            {
                if (f[t, h] != f[t - 1, h])
                {
                    o[t - 1] = true;
                    h -= problema.W[t - 1];
                }

                t--;
            }

            return o;
        }
    }
}