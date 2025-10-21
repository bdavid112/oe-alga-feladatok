using System;

namespace OE.ALGA.Optimalizalas
{
    public class HatizsakProblema
    {
        public int N { get; }
        public int Wmax { get; }
        public int[] W { get; }
        public float[] P { get; }

        public HatizsakProblema(int n, int wmax, int[] w, float[] p)
        {
            N = n;
            Wmax = wmax;
            W = w;
            P = p;
        }

        public int OsszSuly(bool[] pakolas)
        {
            var ossz = 0;
            for (var i = 0; i < N; i++)
            {
                if (pakolas[i]) ossz += W[i];
            }

            return ossz;
        }

        public float OsszErtek(bool[] pakolas)
        {
            var ossz = 0f;
            for (var i = 0; i < N; i++)
            {
                if (pakolas[i]) ossz += P[i];
            }

            return ossz;
        }

        public bool Ervenyes(bool[] pakolas)
        {
            return OsszSuly(pakolas) <= Wmax;
        }
    }

    public class NyersEro<T>
    {
        private int m;
        private Func<int, T> generator;
        private Func<T, float> josag;
        public int LepesSzam { get; private set; }

        public NyersEro(int m, Func<int, T> generator, Func<T, float> josag)
        {
            this.m = m;
            this.generator = generator;
            this.josag = josag;
        }

        public T OptimalisMegoldas()
        {
            var o = generator(1);
            var legjobb = josag(o);

            for (var i = 2; i <= m; i++)
            {
                var x = generator(i);
                var ertek = josag(x);

                LepesSzam++;
                if (!(ertek > legjobb)) continue;

                o = x;
                legjobb = ertek;
            }

            return o;
        }
    }

    public class NyersEroHatizsakPakolas
    {
        private HatizsakProblema problema;
        public int LepesSzam { get; private set; }

        public NyersEroHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public bool[] Generator(int i)
        {
            if (problema.N <= 0)
            {
                return [];
            }

            var megoldas = new bool[problema.N];

            for (var j = 0; j < problema.N; j++)
            {
                megoldas[j] = (i & 1) == 1;
                i >>= 1;
            }

            return megoldas;
        }

        public float Josag(bool[] pakolas)
        {
            if (!problema.Ervenyes(pakolas)) return -1;
            return problema.OsszErtek(pakolas);
        }

        public bool[] OptimalisMegoldas()
        {
            var nyE = new NyersEro<bool[]>((int)Math.Pow(2, problema.N), Generator, Josag);
            var o = nyE.OptimalisMegoldas();
            LepesSzam = nyE.LepesSzam;
            return o;
        }

        public float OptimalisErtek()
        {
            var o = OptimalisMegoldas();
            return problema.OsszErtek(o);
        }
    }
}