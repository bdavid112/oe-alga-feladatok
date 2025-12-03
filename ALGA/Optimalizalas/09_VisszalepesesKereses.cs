using System;

namespace OE.ALGA.Optimalizalas
{
    public class VisszalepesesOptimalizacio<T>(
        int n,
        int[] m,
        T[,] r,
        Func<int, T, bool> ft,
        Func<int, T, T[], bool> fk,
        Func<T[], float> josag
    )
    {
        protected int n = n;
        protected int[] m = m;
        protected T[,] r = r;
        protected Func<int, T, bool> ft = ft;
        protected Func<int, T, T[], bool> fk = fk;
        protected Func<T[], float> josag = josag;
        public int LepesSzam { get; protected set; }

        protected virtual void BackTrack(int szint, T[] e, ref bool van, T[] o)
        {
            for (var i = 0; i < m[szint]; i++)
            {
                LepesSzam++;
                e[szint] = r[szint, i];

                if (!ft(szint, r[szint, i]) || !fk(szint, r[szint, i], e)) continue;

                if (szint == n - 1)
                {
                    if (!van || josag(e) > josag(o)) Array.Copy(e, o, n);
                    van = true;
                }
                else BackTrack(szint + 1, e, ref van, o);

                e[szint] = default;
            }
        }

        public T[] OptimalisMegoldas()
        {
            var o = new T[n];
            var van = false;

            BackTrack(0, new T[n], ref van, o);
            return o;
        }
    }

    public class VisszalepesesHatizsakPakolas(HatizsakProblema problema)
    {
        protected HatizsakProblema problema = problema;
        public int LepesSzam { get; protected set; }

        public virtual bool[] OptimalisMegoldas()
        {
            var n = problema.N;

            var m = new int[n];
            Array.Fill(m, 2);

            var r = new bool[n, 2];
            for (var i = 0; i < n; i++)
            {
                r[i, 0] = true;
                r[i, 1] = false;
            }

            Func<int, bool, bool> ft = (_, _) => true;
            Func<int, bool, bool[], bool> fk = (_, _, e) => problema.OsszSuly(e) <= problema.Wmax;
            Func<bool[], float> josag = e => problema.OsszErtek(e);

            var vo = new VisszalepesesOptimalizacio<bool>(n, m, r, ft, fk, josag);
            var o = vo.OptimalisMegoldas();
            LepesSzam = vo.LepesSzam;

            return o;
        }

        public float OptimalisErtek()
        {
            return problema.OsszErtek(OptimalisMegoldas());
        }
    }

    public class SzetvalasztasEsKorlatozasOptimalizacio<T>(
        int n,
        int[] m,
        T[,] r,
        Func<int, T, bool> ft,
        Func<int, T, T[], bool> fk,
        Func<int, T[], float> fb,
        Func<T[], float> josag) : VisszalepesesOptimalizacio<T>(n, m, r, ft, fk, josag)
    {
        protected Func<int, T[], float> fb = fb;

        protected override void BackTrack(int szint, T[] e, ref bool van, T[] o)
        {
            for (var i = 0; i < m[szint]; i++)
            {
                LepesSzam++;
                e[szint] = r[szint, i];

                if (!ft(szint, r[szint, i]) || !fk(szint, r[szint, i], e)) continue;

                if (szint == n - 1)
                {
                    if (!van || josag(e) > josag(o)) Array.Copy(e, o, n);
                    van = true;
                }
                else if (josag(e) + fb(szint, e) > josag(o)) BackTrack(szint + 1, e, ref van, o);

                e[szint] = default!;
            }
        }
    }

    public class SzetvalasztasEsKorlatozasHatizsakPakolas(HatizsakProblema problema)
        : VisszalepesesHatizsakPakolas(problema)
    {
        public override bool[] OptimalisMegoldas()
        {
            var n = problema.N;

            var m = new int[n];
            Array.Fill(m, 2);

            var r = new bool[n, 2];
            for (var i = 0; i < n; i++)
            {
                r[i, 0] = true;
                r[i, 1] = false;
            }

            Func<int, bool, bool> ft = (_, _) => true;
            Func<int, bool, bool[], bool> fk = (_, _, e) => problema.OsszSuly(e) <= problema.Wmax;
            Func<int, bool[], float> fb = (szint, e) =>
            {
                var b = 0f;
                for (var i = szint; i < n; i++)
                {
                    if (problema.OsszSuly(e) + problema.W[i] <= problema.Wmax) b += problema.P[i];
                }

                return b;
            };
            Func<bool[], float> josag = e => problema.OsszErtek(e);

            var szo = new SzetvalasztasEsKorlatozasOptimalizacio<bool>(n, m, r, ft, fk, fb, josag);
            var o = szo.OptimalisMegoldas();
            LepesSzam = szo.LepesSzam;

            return o;
        }
    }
}